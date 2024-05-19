using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using WebAPICba.Data;
using WebAPICba.Models;

namespace WebAPICba.Services
{
    public class ExchangeRateService
    {
        private readonly ExchangeRateContext _context;
        private readonly ConfigurationService _configurationService;

        public ExchangeRateService(ExchangeRateContext context, ConfigurationService configurationService)
        {
            _context = context;
            _configurationService = configurationService;
        }

        public async Task FetchAndSaveExchangeRates(DateTime startDate, DateTime endDate, string[] isoCodes)
        {
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var soapTemplate = _configurationService.GetSoapTemplate("ExchangeRatesByDate");
                var soapEnvelope = string.Format(soapTemplate, date);

                using var httpClient = new HttpClient();
                var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "http://www.cba.am/ExchangeRatesByDate");

                var response = await httpClient.PostAsync("https://api.cba.am/exchangerates.asmx", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var xmlDoc = XDocument.Parse(responseContent);

                var rates = xmlDoc.Descendants(XName.Get("ExchangeRate", "http://www.cba.am/"))
                    .Where(x => isoCodes.Contains(x.Element(XName.Get("ISO", "http://www.cba.am/"))?.Value))
                    .Select(x => new ExchangeRate
                    {
                        Currency = x.Element(XName.Get("ISO", "http://www.cba.am/"))?.Value,
                        Date = date,
                        Rate = decimal.Parse(x.Element(XName.Get("Rate", "http://www.cba.am/"))?.Value ?? "0")
                    })
                    .ToList();

                var existingRates = await _context.ExchangeRates
                    .Where(r => r.Date == date && isoCodes.Contains(r.Currency))
                    .ToListAsync();

                foreach (var rate in rates)
                {
                    var existingRate = existingRates.FirstOrDefault(r => r.Currency == rate.Currency);
                    if (existingRate != null)
                    {
                        existingRate.Rate = rate.Rate;
                        _context.ExchangeRates.Update(existingRate);
                    }
                    else
                    {
                        _context.ExchangeRates.Add(rate);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ExchangeRate>> GetExchangeRates(DateTime startDate, DateTime endDate, string[] isoCodes)
        {
            return await _context.ExchangeRates
                .Where(r => r.Date >= startDate && r.Date <= endDate && isoCodes.Contains(r.Currency))
                .ToListAsync();
        }
    }
}
