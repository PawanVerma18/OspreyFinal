using System;
using System.Windows.Input;
using Xamarin.Forms;
using Osprey3.Services;
using System.Threading.Tasks;

namespace Osprey3.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private string _newPassword;
        private string _confirmPassword;
        private int _userId;

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public ICommand UpdatePasswordCommand { get; }

        private readonly IUserService _userService;
        private readonly INavigation _navigation;

        public ResetPasswordViewModel(IUserService userService, INavigation navigation, int userId)
        {
            _userService = userService;
            _navigation = navigation;
            _userId = userId;

            UpdatePasswordCommand = new Command(async () => await OnUpdatePasswordAsync());
        }

        private async Task OnUpdatePasswordAsync()
        {
            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            try
            {
                // Update the user's password
                await _userService.UpdateUserPasswordAsync(_userId, NewPassword);

                await Application.Current.MainPage.DisplayAlert("Success", "Password updated successfully.", "OK");

                // Clear the navigation stack and return to the root page (LoginPage)
                await _navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}