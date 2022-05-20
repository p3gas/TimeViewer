using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TimeViewer1.Service;
using TimeViewer1.Exceptions;
using Microsoft.Extensions.Configuration;

namespace TimeViewer1.Pages
{
    public class IndexModel : PageModel
    {
        public string TimeString { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Locations { get; set; }
        public string Location { get; private set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly ITimeProviderService _timeProvider;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _timeProvider = new TimeProviderService(httpClientFactory);

            Location = configuration.GetValue<string>("TimeLocation");
            Locations = new List<string>();
        }

        public async Task OnGet()
        {
            if (string.IsNullOrEmpty(Location))
            {
                await PrintTimeLocations("Aplikacja nie jest poprawnie skonfigurowana. Przypisz nazwę lokalizacji, dla której ma zostać pobrany czas.");
                return;
            }
            try
            {
                var response = await _timeProvider.GetTime(Location);
                TimeString = response.ToString("HH:mm:ss");
            }
            catch(ApiUnavailableException e)
            {
                ErrorMessage = e.Message;
            }
            catch(UnknownAreaException e)
            {
                await PrintTimeLocations(e.Message);
            }
        }

        private async Task PrintTimeLocations(string errorMessage)
        {
            try
            {
                Locations = await _timeProvider.GetLocationNames();
                ErrorMessage = errorMessage + " Poniżej znajdują się dostępne nazwy lokalizacji:";
            }
            catch (ApiUnavailableException e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
