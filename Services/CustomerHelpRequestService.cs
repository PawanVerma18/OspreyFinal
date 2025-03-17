using Newtonsoft.Json;
using Osprey3.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public class CustomerHelpRequestService : ICustomerHelpRequestService
    {
        private readonly HttpClient _httpClient;

        public CustomerHelpRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://192.168.56.1:7035/api/");
        }

        public async Task<List<CustomerHelpRequest>> GetCustomerHelpRequestsAsync()
        {
            var response = await _httpClient.GetStringAsync("CustomerHelpRequests");
            return JsonConvert.DeserializeObject<List<CustomerHelpRequest>>(response);
        }

        public async Task AddCustomerHelpRequestAsync(CustomerHelpRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("CustomerHelpRequests", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }
        }
    }
}