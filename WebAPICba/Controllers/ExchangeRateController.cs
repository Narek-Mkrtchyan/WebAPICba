using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPICba.Services; 

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

        [HttpGet("fetch")]
        public async Task<IActionResult> FetchExchangeRates([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] string isoCodes)
        {
            var isoCodeArray = isoCodes.Split(',');
            await _exchangeRateService.FetchAndSaveExchangeRates(dateFrom, dateTo, isoCodeArray);
            return Ok("Exchange rates fetched and saved.");
        }

        [HttpGet]
        public async Task<IActionResult> GetExchangeRates([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] string isoCodes)
        {
            var isoCodeArray = isoCodes.Split(',');
            var rates = await _exchangeRateService.GetExchangeRates(dateFrom, dateTo, isoCodeArray);
            return Ok(rates);
        }
    }
}
