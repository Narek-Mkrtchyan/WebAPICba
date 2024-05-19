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

        public string GetCbaUrl()
        {
            return _configuration["SoapSettings:CbaUrl"];
        }

        public string GetSoapAction()
        {
            return _configuration["SoapSettings:SoapAction"];
        }
    }
}
