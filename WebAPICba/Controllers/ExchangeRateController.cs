using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPICba.Services;
using WebAPICba.Models;

namespace WebAPICba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;

        public ExchangeRateController(ExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("exchange-rates")]
        public async Task<IActionResult> GetAndFetchExchangeRates([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] string isoCodes)
        {
            var isoCodeArray = isoCodes.Split(',');

            // Fetch and save exchange rates from the API
            await _exchangeRateService.FetchAndSaveExchangeRates(dateFrom, dateTo, isoCodeArray);

            // Get exchange rates from the database
            var rates = await _exchangeRateService.GetExchangeRates(dateFrom, dateTo, isoCodeArray);
            return Ok(rates);
        }
    }
}
