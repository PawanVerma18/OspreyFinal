using System;
using System.Windows.Input;
using Xamarin.Forms;
using Osprey3.Services;
using Osprey3.Views;
using System.Threading.Tasks;

namespace Osprey3.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private string _email;
        private bool _isBusy;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ICommand CheckEmailCommand { get; }
        public ICommand BackCommand { get; }

        private readonly IUserService _userService;
        private readonly INavigation _navigation;
        private readonly EmailService _emailService;
        private static readonly Random _random = new Random();

        public ForgotPasswordViewModel(IUserService userService, INavigation navigation, EmailService emailService)
        {
            _userService = userService;
            _navigation = navigation;
            _emailService = emailService;

            CheckEmailCommand = new Command(async () => await OnCheckEmailAsync());
            BackCommand = new Command(async () => await OnBackAsync());
        }

        private async Task OnCheckEmailAsync()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please enter your email.", "OK");
                    return;
                }

                if (!IsValidEmail(Email))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid email address.", "OK");
                    return;
                }

                // Check if the email exists via API
                var emailExists = await _userService.CheckEmailAsync(Email);
                if (!emailExists)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No user found with this email.", "OK");
                    return;
                }

                // Generate a verification code
                var verificationCode = GenerateVerificationCode();

                // Send the verification code via email (directly from the mobile app)
                await _emailService.SendEmailAsync(Email, "Verification Code", $"Your verification code is: {verificationCode}");

                // Navigate to the VerifyCodePage
                await _navigation.PushAsync(new VerifyCodePage(new VerifyCodeViewModel(_userService, _navigation, Email, verificationCode)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnCheckEmailAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnBackAsync()
        {
            await _navigation.PopAsync();
        }

        private string GenerateVerificationCode()
        {
            // Generate a random 6-digit code
            return _random.Next(100000, 999999).ToString();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}