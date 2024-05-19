using Microsoft.AspNetCore.Mvc;
using WebAPICba.Services;

namespace WebAPICba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;

        // Constructor to inject ExchangeRateService instance
        public ExchangeRateController(ExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        // Action to fetch and save exchange rates based on provided parameters
        [HttpGet("fetch")]
        public async Task<IActionResult> FetchExchangeRates([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] string isoCodes)
        {
            // Split ISO codes string into an array
            var isoCodeArray = isoCodes.Split(',');

            // Call ExchangeRateService method to fetch and save exchange rates asynchronously
            await _exchangeRateService.FetchAndSaveExchangeRates(dateFrom, dateTo, isoCodeArray);

            // Return OK response with a message indicating success
            return Ok("Exchange rates fetched and saved.");
        }

        // Action to get exchange rates based on provided parameters
        [HttpGet]
        public async Task<IActionResult> GetExchangeRates([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo, [FromQuery] string isoCodes)
        {
            // Split ISO codes string into an array
            var isoCodeArray = isoCodes.Split(',');

            // Call ExchangeRateService method to get exchange rates asynchronously
            var rates = await _exchangeRateService.GetExchangeRates(dateFrom, dateTo, isoCodeArray);

            // Return OK response with the exchange rates retrieved from the service
            return Ok(rates);
        }
    }
}
