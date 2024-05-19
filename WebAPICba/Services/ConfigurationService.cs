using Microsoft.Extensions.Configuration;

namespace WebAPICba.Services
{
    // Service class for accessing configuration settings
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        // Constructor to inject IConfiguration dependency
        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method to retrieve SOAP template from configuration based on key
        public string GetSoapTemplate(string key)
        {
            // Retrieve SOAP template from configuration using specified key
            // The SOAP template is expected to be stored in the "SoapTemplates" section of the appsettings.json file
            return _configuration[$"SoapTemplates:{key}"];
        }
    }
}
