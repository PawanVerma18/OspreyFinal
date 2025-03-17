using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Osprey3.Models;
using Osprey3.Services;

namespace Osprey3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageUsersPage : ContentPage
    {
        private readonly UserService _userService = new UserService();

        public ManageUsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private async void LoadUsers()
        {
            try
            {
                // Fetch users from the database
                var users = await _userService.GetUsersAsync();

                // Set the users as the ListView's ItemsSource
                UsersListView.ItemsSource = users;

                // Update the user counter
                UserCountLabel.Text = $"Total Users: {users.Count}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
            }
        }

        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            // Get the selected user
            var selectedUser = e.SelectedItem as User;

            // Deselect the item
            UsersListView.SelectedItem = null;

            // Navigate to the UserDetailsPage (or reuse EditUserPage as view-only)
            await Navigation.PushAsync(new EditUserPage(selectedUser));
        }
    }
}