using Newtonsoft.Json;
using Osprey3.Models;
using Osprey3.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Osprey3.ViewModels
{
    public class CustomerHelpRequestViewModel : BaseViewModel
    {
        private readonly ICustomerHelpRequestService _customerHelpRequestService;
        private readonly EmailService _emailService;

        public CustomerHelpRequest NewCustomerHelpRequest { get; set; } = new CustomerHelpRequest();
        public ObservableCollection<CustomerHelpRequest> CustomerHelpRequests { get; } = new ObservableCollection<CustomerHelpRequest>();

        public ICommand AddCustomerHelpRequestCommand { get; }

        public CustomerHelpRequestViewModel(ICustomerHelpRequestService customerHelpRequestService, EmailService emailService)
        {
            _customerHelpRequestService = customerHelpRequestService;
            _emailService = emailService;

            AddCustomerHelpRequestCommand = new Command(async () => await AddCustomerHelpRequestAsync());
        }

        private async Task AddCustomerHelpRequestAsync()
        {
            try
            {
                // Always show success message
                await Application.Current.MainPage.DisplayAlert("Success", "Request added successfully!", "OK");

                // Get the saved user email
                if (Application.Current.Properties.ContainsKey("UserEmail"))
                {
                    string userEmail = Application.Current.Properties["UserEmail"] as string;

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        // Send email with request details to the ADMIN email
                        string adminEmail = "joinosprey@gmail.com"; // Replace with the actual admin email
                        string emailBody = $"New Customer Help Request:\n\n" +
                                          $"Full Name: {NewCustomerHelpRequest.FullName}\n" +
                                          $"Email: {NewCustomerHelpRequest.Email}\n" +
                                          $"Phone Number: {NewCustomerHelpRequest.PhoneNumber}\n" +
                                          $"Issue Description: {NewCustomerHelpRequest.IssueDescription}\n\n" +
                                          $"Submitted by: {userEmail}"; // Include the user's email in the body

                        Console.WriteLine($"Sending email to: {adminEmail}");
                        await _emailService.SendEmailAsync(adminEmail, "New Customer Help Request", emailBody);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "User email not found.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User email not found.", "OK");
                }

                // Reset the form
                NewCustomerHelpRequest = new CustomerHelpRequest();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to send email: {ex.Message}", "OK");
            }
        }
    }
}