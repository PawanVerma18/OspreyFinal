using Newtonsoft.Json;
using Osprey3.Models;
using Osprey3.Services;
using Osprey3.ViewModels;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Osprey3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Homepage : ContentPage
    {
        private readonly IUserService _userService;
        private readonly CustomerHelpRequestViewModel _customerHelpRequestViewModel;

        public Homepage(IUserService userService)
        {
            InitializeComponent();
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));

            // Initialize HttpClient
            var httpClient = new HttpClient(HttpClientFactory.CreateHandler());

            // Initialize services
            var eventService = new EventService(httpClient);
            var promotionService = new PromotionService(httpClient);
            var fundingService = new FundingService(httpClient);

            // Initialize CustomerHelpRequestViewModel
            var customerHelpRequestService = new CustomerHelpRequestService(httpClient);
            var emailService = new EmailService(
                 smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                smtpUsername: "joinosprey@gmail.com",
                smtpPassword: "zpmn eppn xkll wind",
                enableSsl: true
            );
            _customerHelpRequestViewModel = new CustomerHelpRequestViewModel(customerHelpRequestService, emailService);

            // Set the BindingContext for the page
            BindingContext = new AdminPageViewModel(Navigation, eventService, promotionService, fundingService);
        }

        private async void OnViewUserDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the saved email
                if (Application.Current.Properties.ContainsKey("UserEmail"))
                {
                    string userEmail = Application.Current.Properties["UserEmail"] as string;

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        // Fetch user details using the email
                        var currentUser = await _userService.GetUserByEmailAsync(userEmail);

                        if (currentUser != null)
                        {
                            // Navigate to the UserDetailsPage and pass the user object
                            await Navigation.PushAsync(new UserDetailsPage(currentUser));
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to fetch user data.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "User email not found.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "User email not found.", "OK");
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"Failed to connect to the server: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnCustomerHelpClicked(object sender, EventArgs e)
        {
            // Navigate to the CustomerHelpRequestPage and pass the ViewModel
            await Navigation.PushAsync(new CustomerHelpRequestPage(_customerHelpRequestViewModel));
        }
    }
}