using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Osprey3.Models;
using Osprey3.Services;
using Osprey3.Views;
using System.Net.Http;

namespace Osprey3.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private string _fullName;
        private string _email;
        private string _password;
        private string _phoneNumber;
        private DateTime _dateOfBirth = DateTime.Now; // Default to current date
        private string _gender;
        private string _address;

        private readonly IUserService _userService;
        private readonly INavigation _navigation;

        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                }
            }
        }

        public string Gender
        {
            get => _gender;
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged(nameof(Gender));
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        // Register Command
        public ICommand RegisterCommand { get; }

        // Back Command
        public ICommand BackCommand { get; }

        // Constructor to initialize the view model
        public RegistrationViewModel(IUserService userService, INavigation navigation)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            // Initialize Commands
            RegisterCommand = new Command(async () => await OnRegisterAsync());
            BackCommand = new Command(async () => await OnBackAsync());
        }

        // Method to handle registration process
        private async Task OnRegisterAsync()
        {
            // Create a new user object
            var newUser = new User
            {
                Name = FullName,
                Email = Email,
                Password = Password,
                PhoneNumber = PhoneNumber,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                Address = Address
            };

            try
            {
                // Call RegisterUserAsync method from IUserService to register the user
                bool isRegistered = await _userService.RegisterUserAsync(newUser);
                if (isRegistered)
                {
                    // Show success message
                    await Application.Current.MainPage.DisplayAlert("Success", "Registration successful", "OK");

                    // Navigate to MainScreen after successful registration
                    await NavigateToMainScreen();
                }
                else
                {
                    // Handle case where registration failed (e.g., user already exists)
                    await Application.Current.MainPage.DisplayAlert("Error", "User already exists. Please try again.", "OK");
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

        // Method to handle back button logic
        private async Task OnBackAsync()
        {
            // Navigate back to the previous page (MainScreen)
            await _navigation.PopAsync();
        }

        // Method to navigate to MainScreen
        private async Task NavigateToMainScreen()
        {
            try
            {
                // Resolve IUserService from the dependency injection container
                var userService = Startup.Resolve<IUserService>();

                // Create an instance of MainScreen with the required IUserService parameter
                var mainScreen = new MainScreen(userService);

                // Navigate to MainScreen
                await _navigation.PushAsync(mainScreen);

                // Optionally, remove the Registration page from the navigation stack
                var existingPages = _navigation.NavigationStack;
                if (existingPages.Count > 1)
                {
                    _navigation.RemovePage(existingPages[existingPages.Count - 2]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to MainScreen: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to navigate to the main screen.", "OK");
            }
        }

        // Implement INotifyPropertyChanged interface to update UI bindings
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}