using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Services
{
    public class HttpService
    {
        private readonly HttpClient _client;

        public HttpService()
        {
            _client = new HttpClient();
        }

        public async Task<string?> GetJsonAsync(string uri)
        {
            try
            {
                var endpoint = new Uri(uri);
                var result = await _client.GetAsync(endpoint);
                result.EnsureSuccessStatusCode();

                return await result.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"HTTP Request Error Occurred: {httpEx.Message}");
            }
        }
    }
}
