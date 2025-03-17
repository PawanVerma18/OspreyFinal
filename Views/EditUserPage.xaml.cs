using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Osprey3.Models;
using Osprey3.Services;
using System;

namespace Osprey3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserPage : ContentPage
    {


        public EditUserPage(User user)
        {
            InitializeComponent();
            BindingContext = user; // Set the selected user as the BindingContext
        }


    }
}