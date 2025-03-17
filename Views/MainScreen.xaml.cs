using Osprey3.Services;
using Osprey3.ViewModels;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Osprey3.Views;
using Osprey3.ViewModel;

namespace Osprey3.Views
{
    public partial class MainScreen : ContentPage
    {
        private readonly IUserService _userService;

        // Constructor with dependency injection
        public MainScreen(IUserService userService)
        {
            InitializeComponent();
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));

            // Set BindingContext to MainScreenViewModel
            BindingContext = new MainScreenViewModel();
        }

        // Event handler for Signup button
        private async void OnSignupClicked(object sender, EventArgs e)
        {
            try
            {
                // Create RegistrationViewModel with IUserService and Navigation
                var registrationViewModel = new RegistrationViewModel(_userService, Navigation);

                // Navigate to Registration page and pass the RegistrationViewModel
                await Navigation.PushAsync(new Registration(registrationViewModel));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
            }
        }
        // Event handler for Forgot Password link
        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            try
            {
                // Navigate to CheckUserPage and pass IUserService
                await Navigation.PushAsync(new CheckUserPage(new CheckUserViewModel(_userService, Navigation)));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
            }
        }
       

        // Event handler for Login button
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                // Get the email and password from the ViewModel
                var viewModel = BindingContext as MainScreenViewModel;
                string email = viewModel?.Email;
                string password = viewModel?.Password;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Validation Error", "Please enter your email and password.", "OK");
                    return;
                }

                // Check for the admin email and password
                if (email == "admin@example.com" && password == "admin123")
                {
                    // Navigate to Admin Page
                    await Navigation.PushAsync(new AdminPage());
                }
                else
                {
                    // Check if the user exists in the database
                    bool userExists = await _userService.CheckUserExistsAsync(email, password);

                    if (userExists)
                    {
                        // Save the user's email after successful login
                        Application.Current.Properties["UserEmail"] = email;
                        await Application.Current.SavePropertiesAsync();

                        // Navigate to Homepage and pass the IUserService instance
                        await Navigation.PushAsync(new Homepage(_userService));
                    }
                    else
                    {
                        // Show an error message
                        await DisplayAlert("Error", "User does not exist or Username And password is Wrong.", "OK");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network or API errors
                await DisplayAlert("Error", $"Failed to connect to the server: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }
    }
}