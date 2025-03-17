using System;
using System.Windows.Input;
using Xamarin.Forms;
using Osprey3.Services;
using Osprey3.Views;
using System.Threading.Tasks;

namespace Osprey3.ViewModels
{
    public class VerifyCodeViewModel : BaseViewModel
    {
        private string _verificationCode;
        private string _email;
        private string _expectedCode;

        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                _verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
            }
        }

        public ICommand VerifyCodeCommand { get; }
        public ICommand BackCommand { get; }

        private readonly IUserService _userService;
        private readonly INavigation _navigation;
        private readonly EmailService _emailService;

        public VerifyCodeViewModel(IUserService userService, INavigation navigation, string email, string expectedCode)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _expectedCode = expectedCode ?? throw new ArgumentNullException(nameof(expectedCode));

            // Initialize EmailService
            _emailService = new EmailService(
                smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                smtpUsername: "joinosprey@gmail.com",
                smtpPassword: "zpmn eppn xkll wind",
                enableSsl: true
            );

            VerifyCodeCommand = new Command(async () => await OnVerifyCodeAsync());
            BackCommand = new Command(async () => await OnBackAsync());

            // Send the verification code via email when the ViewModel is initialized
            SendVerificationCodeEmail().ConfigureAwait(false);
        }

        private async Task SendVerificationCodeEmail()
        {
            try
            {
                // Send the verification code via email
                await _emailService.SendEmailAsync(_email, "Verification Code", $"Your verification code is: {_expectedCode}");

                await Application.Current.MainPage.DisplayAlert("Success", "A verification code has been sent to your email.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to send verification code: {ex.Message}", "OK");
            }
        }

        private async Task OnVerifyCodeAsync()
        {
            if (string.IsNullOrEmpty(VerificationCode))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter the verification code.", "OK");
                return;
            }

            if (VerificationCode != _expectedCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid verification code.", "OK");
                return;
            }

            try
            {
                // Fetch the user by email
                var user = await _userService.GetUserByEmailAsync(_email);
                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User not found.", "OK");
                    return;
                }

                // Navigate to the ResetPasswordPage
                await _navigation.PushAsync(new ResetPasswordPage(new ResetPasswordViewModel(_userService, _navigation, user.Id)));
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
    }
}