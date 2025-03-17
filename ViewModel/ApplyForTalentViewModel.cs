using Osprey3.Models;
using Osprey3.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Osprey3.ViewModels
{
    public class ApplyForTalentViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public TalentApplication TalentApplication { get; set; } = new TalentApplication();

        public ICommand ApplyCommand { get; }
        public ICommand BackCommand { get; }

        public ApplyForTalentViewModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            ApplyCommand = new Command(async () => await OnApplyClicked());
            BackCommand = new Command(OnBackClicked);
        }

        private async Task OnApplyClicked()
        {
            try
            {
                // Remove validation and submit the talent application
                TalentApplication.AppliedOn = DateTime.UtcNow;

                bool isSuccess = await _userService.SubmitTalentApplicationAsync(TalentApplication);

                if (isSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Talent application submitted successfully!", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to submit talent application. Please try again.", "OK");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network or API errors
                Debug.WriteLine($"Network Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to connect to the server. Please check your internet connection.", "OK");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Debug.WriteLine($"Error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An unexpected error occurred. Please try again.", "OK");
            }
        }

        private void OnBackClicked()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}