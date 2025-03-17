using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Osprey3.Models;
using System;

namespace Osprey3.Services
{
    public class FundingService : IFundingService
    {
        private readonly HttpClient _httpClient;

        public FundingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://192.168.56.1:7035/api/"); // Replace with your API URL
        }

        public async Task<decimal> GetTotalFundingAsync()
        {
            var response = await _httpClient.GetStringAsync("Fundings/Total");
            return JsonConvert.DeserializeObject<decimal>(response);
        }

        public async Task AddContributionAsync(Funding contribution)
        {
            var json = JsonConvert.SerializeObject(contribution);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("Fundings", content);
        }
    }
}