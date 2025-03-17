using Newtonsoft.Json;
using Osprey3.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly HttpClient _httpClient;

        public PromotionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://192.168.56.1:7035/api/"); // Replace with your API URL
        }

        public async Task<List<Promotion>> GetPromotionsAsync()
        {
            var response = await _httpClient.GetStringAsync("Promotions");
            return JsonConvert.DeserializeObject<List<Promotion>>(response);
        }

        public async Task<Promotion> GetPromotionAsync(int id)
        {
            var response = await _httpClient.GetStringAsync($"Promotions/{id}");
            return JsonConvert.DeserializeObject<Promotion>(response);
        }

        public async Task AddPromotionAsync(Promotion promotion)
        {
            var json = JsonConvert.SerializeObject(promotion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("Promotions", content);
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            var json = JsonConvert.SerializeObject(promotion);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"Promotions/{promotion.Id}", content);
        }

        public async Task DeletePromotionAsync(int id)
        {
            await _httpClient.DeleteAsync($"Promotions/{id}");
        }
    }
}