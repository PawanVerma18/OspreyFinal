using Xamarin.Forms;
using Osprey3.Models;
using System;
using System.Diagnostics;

namespace Osprey3.Views
{
    public partial class UserDetailsPage : ContentPage
    {
        public UserDetailsPage(User user)
        {
            InitializeComponent();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");
            }

            BindingContext = user; // Set BindingContext
        }

       
    }

}