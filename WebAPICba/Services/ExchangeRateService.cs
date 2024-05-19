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

        // Constructor to inject dependencies
        public ExchangeRateService(ExchangeRateContext context, ConfigurationService configurationService)
        {
            _context = context;
            _configurationService = configurationService;
        }

        // Method to fetch exchange rates from the CBA API and save them to the database
        public async Task FetchAndSaveExchangeRates(DateTime startDate, DateTime endDate, string[] isoCodes)
        {
            // Loop through each date in the specified range
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Construct SOAP envelope using template retrieved from configuration service
                var soapTemplate = _configurationService.GetSoapTemplate("ExchangeRatesByDate");
                var soapEnvelope = string.Format(soapTemplate, date);

                // Send SOAP request to CBA API
                using var httpClient = new HttpClient();
                var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "http://www.cba.am/ExchangeRatesByDate");
                var response = await httpClient.PostAsync("https://api.cba.am/exchangerates.asmx", content);
                response.EnsureSuccessStatusCode();

                // Parse response XML
                var responseContent = await response.Content.ReadAsStringAsync();
                var xmlDoc = XDocument.Parse(responseContent);

                // Extract exchange rates from XML response
                var rates = xmlDoc.Descendants(XName.Get("ExchangeRate", "http://www.cba.am/"))
                    .Where(x => isoCodes.Contains(x.Element(XName.Get("ISO", "http://www.cba.am/"))?.Value))
                    .Select(x => new ExchangeRate
                    {
                        Currency = x.Element(XName.Get("ISO", "http://www.cba.am/"))?.Value,
                        Date = date,
                        Rate = decimal.Parse(x.Element(XName.Get("Rate", "http://www.cba.am/"))?.Value ?? "0")
                    })
                    .ToList();

                // Retrieve existing rates from the database
                var existingRates = await _context.ExchangeRates
                    .Where(r => r.Date == date && isoCodes.Contains(r.Currency))
                    .ToListAsync();

                // Update or add new rates to the database
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

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }

        // Method to get exchange rates from the database
        public async Task<List<ExchangeRate>> GetExchangeRates(DateTime startDate, DateTime endDate, string[] isoCodes)
        {
            return await _context.ExchangeRates
                .Where(r => r.Date >= startDate && r.Date <= endDate && isoCodes.Contains(r.Currency))
                .ToListAsync();
        }
    }
}
