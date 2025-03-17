using System;
using Xamarin.Forms;

namespace Osprey3
{
    public partial class PaymentPage : ContentPage
    {
        public PaymentPage()
        {
            InitializeComponent();
        }

        private async void OnPaymentButtonClicked(object sender, EventArgs e)
        {
            // Validate Contributor Name
            if (string.IsNullOrWhiteSpace(ContributorNameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter your name.", "OK");
                return;
            }

            // Validate Amount
            if (!decimal.TryParse(AmountEntry.Text, out decimal amount) || amount <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid amount.", "OK");
                return;
            }

            // Validate Cardholder Name
            if (string.IsNullOrWhiteSpace(CardholderNameEntry.Text))
            {
                await DisplayAlert("Error", "Please enter the cardholder name.", "OK");
                return;
            }

            // Validate Card Number
            if (string.IsNullOrWhiteSpace(CardNumberEntry.Text) || CardNumberEntry.Text.Length != 16)
            {
                await DisplayAlert("Error", "Please enter a valid 16-digit card number.", "OK");
                return;
            }

            // Validate Expiry Date
            if (string.IsNullOrWhiteSpace(ExpiryDateEntry.Text) || ExpiryDateEntry.Text.Length != 5 || !ExpiryDateEntry.Text.Contains("/"))
            {
                await DisplayAlert("Error", "Please enter a valid expiry date (MM/YY).", "OK");
                return;
            }

            // Validate CVV
            if (string.IsNullOrWhiteSpace(CVVEntry.Text) || CVVEntry.Text.Length != 3)
            {
                await DisplayAlert("Error", "Please enter a valid 3-digit CVV.", "OK");
                return;
            }

            // Validate Terms and Conditions
            if (!TermsCheckBox.IsChecked)
            {
                await DisplayAlert("Error", "Please agree to the terms and conditions.", "OK");
                return;
            }

            // Simulate Payment Processing
            await DisplayAlert("Success", $"Payment of {amount:C} processed successfully!", "OK");

            // Update Total Funding (using MessagingCenter)
            MessagingCenter.Send(this, "UpdateFunding", amount);

            // Navigate back to the main page
            await Navigation.PopAsync();
        }
    }
}