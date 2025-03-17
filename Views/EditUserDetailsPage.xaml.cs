using Xamarin.Forms;
using Osprey3.ViewModels;
using Osprey3.Models;
using Osprey3.Services;
using System.Diagnostics;

namespace Osprey3.Views
{
    public partial class EditUserDetailsPage : ContentPage
    {
        public EditUserDetailsPage(User user)
        {
            InitializeComponent();

            // Debugging: Check if the user object is passed correctly
            Debug.WriteLine($"Editing user: {user?.Name}");

            // Get the user service via dependency injection
            var userService = DependencyService.Get<IUserService>();

            // Initialize the ViewModel with the user object
            BindingContext = new EditUserDetailsViewModel(userService, Navigation, user);
        }
    }
}