using Xamarin.Forms;
using Osprey3.ViewModels;

namespace Osprey3.Views
{
    public partial class CheckUserPage : ContentPage
    {
        public CheckUserPage(CheckUserViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // Set the BindingContext to the ViewModel
        }
    }
}