using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Osprey3.Models;
using System;

namespace Osprey3.Services
{
    public class EventService : IEventService
    {
        private readonly HttpClient _httpClient;

        public EventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://192.168.56.1:7035/api/"); // Replace with your API URL
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            var response = await _httpClient.GetStringAsync("Events");
            return JsonConvert.DeserializeObject<List<Event>>(response);
        }

        public async Task<Event> GetEventAsync(int id)
        {
            var response = await _httpClient.GetStringAsync($"Events/{id}");
            return JsonConvert.DeserializeObject<Event>(response);
        }

        public async Task AddEventAsync(Event eventItem)
        {
            var json = JsonConvert.SerializeObject(eventItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("Events", content);
        }

        public async Task UpdateEventAsync(Event eventItem)
        {
            var json = JsonConvert.SerializeObject(eventItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"Events/{eventItem.Id}", content);
        }

        public async Task DeleteEventAsync(int id)
        {
            await _httpClient.DeleteAsync($"Events/{id}");
        }
    }
}