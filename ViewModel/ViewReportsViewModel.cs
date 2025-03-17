using Osprey3.Models;
using Osprey3.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Osprey3.ViewModels
{
    public class ViewReportsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public ObservableCollection<TalentApplication> TalentApplications { get; set; } = new ObservableCollection<TalentApplication>();

        public ICommand LoadTalentApplicationsCommand { get; }

        public ViewReportsViewModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LoadTalentApplicationsCommand = new Command(async () => await LoadTalentApplicationsAsync());
        }

        private async Task LoadTalentApplicationsAsync()
        {
            IsBusy = true;

            try
            {
                // Fetch talent applications from the API
                var applications = await _userService.GetTalentApplicationsAsync();

                // Clear existing data and add new data
                TalentApplications.Clear();
                foreach (var application in applications)
                {
                    TalentApplications.Add(application);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network or API errors
                Debug.WriteLine($"Network Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to fetch data. Please check your internet connection.", "OK");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Debug.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}