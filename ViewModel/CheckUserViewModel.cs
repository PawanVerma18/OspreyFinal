using System;
using System.Windows.Input;
using Xamarin.Forms;
using Osprey3.Services;
using Osprey3.Views;
using System.Threading.Tasks;

namespace Osprey3.ViewModels
{
    public class CheckUserViewModel : BaseViewModel
    {
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ICommand CheckEmailCommand { get; }
        public ICommand BackCommand { get; }

        private readonly IUserService _userService;
        private readonly INavigation _navigation;

        public CheckUserViewModel(IUserService userService, INavigation navigation)
        {
            _userService = userService;
            _navigation = navigation;

            CheckEmailCommand = new Command(async () => await OnCheckEmailAsync());
            BackCommand = new Command(async () => await OnBackAsync());
        }

        private async Task OnCheckEmailAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your email.", "OK");
                return;
            }

            try
            {
                // Check if the email exists
                var user = await _userService.GetUserByEmailAsync(Email);
                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No user found with this email.", "OK");
                    return;
                }

                // Generate a verification code and send it to the user's email
                var verificationCode = GenerateVerificationCode();
                await SendVerificationCodeToEmail(Email, verificationCode);

                // Navigate to the VerifyCodePage
                await _navigation.PushAsync(new VerifyCodePage(new VerifyCodeViewModel(_userService, _navigation, Email, verificationCode)));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task OnBackAsync()
        {
            await _navigation.PopAsync();
        }

        private string GenerateVerificationCode()
        {
            // Generate a random 6-digit code
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SendVerificationCodeToEmail(string email, string code)
        {
            // Simulate sending an email (replace with actual email sending logic)
            await Task.Delay(1000); // Simulate network delay
            Console.WriteLine($"Verification code sent to {email}: {code}");
        }
    }
}