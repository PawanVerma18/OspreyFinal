using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Osprey3.Models;

namespace Osprey3.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            // Use the custom handler to bypass SSL validation
            var handler = HttpClientFactory.CreateHandler();
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://192.168.56.1:7035/api/")
            };
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Users");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<User>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Users/{user.Id}", user);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user with ID {user.Id}: {ex.Message}");
                throw;
            }
        }
    }
}