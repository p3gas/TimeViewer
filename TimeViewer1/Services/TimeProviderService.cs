using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TimeViewer1.Dtos;
using TimeViewer1.Exceptions;

namespace TimeViewer1.Service
{
    public class TimeProviderService : ITimeProviderService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string apiUrl = "http://worldtimeapi.org/api/timezone/";
        private readonly string apiUnavailableErrorMessage = "Ups... Nie udało się nam połączyć z naszym zegarem. Spróbuj ponownie za jakiś czas.";

        public TimeProviderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<DateTime> GetTime(string location)
        {
            string url = apiUrl + location;
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new UnknownAreaException("Upewnij się, że szukasz godziny dla poprawnego regionu.");
                }
                else if (!response.IsSuccessStatusCode)
                {
                    throw new ApiUnavailableException(apiUnavailableErrorMessage);
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<GetTimeDto>(responseJson);

                return DateTimeOffset.Parse(responseData.datetime).DateTime;
            }
            catch(HttpRequestException e)
            {
                throw new ApiUnavailableException(apiUnavailableErrorMessage, e);
            }
        }

        public async Task<List<string>> GetLocationNames()
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync(apiUrl);
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<List<string>>(responseJson);

                return responseData;
            }
            catch (HttpRequestException e)
            {
                throw new ApiUnavailableException(apiUnavailableErrorMessage, e);
            }
        }
    }
}
