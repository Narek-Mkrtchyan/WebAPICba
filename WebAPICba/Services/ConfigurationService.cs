using Microsoft.Extensions.Configuration;

namespace WebAPICba.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSoapTemplate(string key)
        {
            return _configuration[$"SoapTemplates:{key}"];
        }
    }
}
