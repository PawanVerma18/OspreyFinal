using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Osprey3.Models;
using System.Diagnostics;
using System.Text;
using System.Net.Http.Formatting; // Add this line

namespace Osprey3.Services
{
    public class ApiUserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public ApiUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ==================== User Methods ====================
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("Users");
                return JsonConvert.DeserializeObject<IEnumerable<User>>(response);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching users: {ex.Message}");
                throw new HttpRequestException("Failed to fetch users. Please check your network connection.", ex);
            }
        }

        public async Task<User> GetUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"Users/{id}");
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching user with ID {id}: {ex.Message}");
                throw new HttpRequestException($"Failed to fetch user with ID {id}. Please check your network connection.", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"Users/ByEmail/{email}");
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching user by email {email}: {ex.Message}");
                throw new HttpRequestException($"Failed to fetch user by email {email}. Please check your network connection.", ex);
            }
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {
                var userJson = JsonConvert.SerializeObject(user);
                var content = new StringContent(userJson, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Users", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return false; // User already exists
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Registration failed: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var userJson = JsonConvert.SerializeObject(user);
                var content = new StringContent(userJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"Users/{user.Id}", content);
                response.EnsureSuccessStatusCode();
                return true; // Return true if successful
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating user with ID {user.Id}: {ex.Message}");
                return false; // Return false if an error occurs
            }
        }
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Users/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting user with ID {id}: {ex.Message}");
                throw new HttpRequestException($"Failed to delete user with ID {id}. Please check your network connection.", ex);
            }
        }

        // ==================== Authentication and Verification ====================

        public async Task<bool> CheckUserExistsAsync(string email, string password)
        {
            try
            {
                var request = new { Email = email, Password = password };
                var requestJson = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Users/CheckUser", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(responseContent);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to check user: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking user: {ex.Message}");
                throw new HttpRequestException("Failed to check user. Please try again.", ex);
            }
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Users/CheckEmail?email={email}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking email: {ex.Message}");
                throw new HttpRequestException("Failed to check email. Please try again.", ex);
            }
        }

        public async Task<bool> SendVerificationCodeAsync(string email, string code)
        {
            try
            {
                var request = new { Email = email, Code = code };
                var requestJson = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Users/SendVerificationCode", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Failed to send verification code: {errorMessage}");
                    throw new HttpRequestException($"Failed to send verification code: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error sending verification code to {email}: {ex.Message}");
                throw new HttpRequestException($"Failed to send verification code to {email}. Please check your network connection.", ex);
            }
        }

        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            try
            {
                var request = new { Email = email, Code = code };
                var requestJson = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

                Debug.WriteLine($"Request: {requestJson}");

                var response = await _httpClient.PostAsync("Users/VerifyCode", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response: {responseContent}");
                    return JsonConvert.DeserializeObject<bool>(responseContent);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Failed to verify code: {errorMessage}");
                    throw new HttpRequestException($"Failed to verify code: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error verifying code for {email}: {ex.Message}");
                throw new HttpRequestException($"Failed to verify code for {email}. Please check your network connection.", ex);
            }
        }

        // ==================== Password Management ====================

        public async Task UpdateUserPasswordAsync(int userId, string newPassword)
        {
            try
            {
                var request = new { UserId = userId, NewPassword = newPassword };
                var requestJson = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"Users/{userId}/UpdatePassword", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating password for user with ID {userId}: {ex.Message}");
                throw new HttpRequestException($"Failed to update password for user with ID {userId}. Please check your network connection.", ex);
            }
        }

        // ==================== Utility Methods ====================

        public async Task SaveUserAsync(User user)
        {
            try
            {
                if (user.Id == 0)
                {
                    await RegisterUserAsync(user);
                }
                else
                {
                    await UpdateUserAsync(user);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving user: {ex.Message}");
                throw new HttpRequestException("Failed to save user. Please try again.", ex);
            }
        }

        // ==================== Talent Application Methods ====================
        public async Task<IEnumerable<TalentApplication>> GetTalentApplicationsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("TalentApplications");
                return JsonConvert.DeserializeObject<IEnumerable<TalentApplication>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching talent applications: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> SubmitTalentApplicationAsync(TalentApplication talentApplication)
        {
            try
            {
                var json = JsonConvert.SerializeObject(talentApplication);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                Debug.WriteLine($"Sending request to: {_httpClient.BaseAddress}TalentApplications");
                Debug.WriteLine($"Request payload: {json}");

                var response = await _httpClient.PostAsync("TalentApplications", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Talent application submitted successfully.");
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Failed to submit talent application. Status: {response.StatusCode}, Error: {errorMessage}");
                    throw new HttpRequestException($"Failed to submit talent application: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error submitting talent application: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateTalentApplicationAsync(TalentApplication talentApplication)
        {
            try
            {
                var json = JsonConvert.SerializeObject(talentApplication);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"TalentApplications/{talentApplication.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating talent application: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteTalentApplicationAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"TalentApplications/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting talent application: {ex.Message}");
                throw;
            }
        }

       

        // ==================== Customer Help Request Methods ====================
        public async Task<IEnumerable<CustomerHelpRequest>> GetCustomerHelpRequestsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("CustomerHelpRequests");
                return JsonConvert.DeserializeObject<IEnumerable<CustomerHelpRequest>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer help requests: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> SubmitCustomerHelpRequestAsync(CustomerHelpRequest customerHelpRequest)
        {
            try
            {
                var json = JsonConvert.SerializeObject(customerHelpRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                Debug.WriteLine($"Sending request to: {_httpClient.BaseAddress}CustomerHelpRequests");
                Debug.WriteLine($"Request payload: {json}");

                var response = await _httpClient.PostAsync("CustomerHelpRequests", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Customer help request submitted successfully.");
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Failed to submit customer help request. Status: {response.StatusCode}, Error: {errorMessage}");
                    throw new HttpRequestException($"Failed to submit customer help request: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error submitting customer help request: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateCustomerHelpRequestAsync(CustomerHelpRequest customerHelpRequest)
        {
            try
            {
                var json = JsonConvert.SerializeObject(customerHelpRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"CustomerHelpRequests/{customerHelpRequest.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating customer help request: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteCustomerHelpRequestAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"CustomerHelpRequests/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer help request: {ex.Message}");
                throw;
            }
        }
    }
}