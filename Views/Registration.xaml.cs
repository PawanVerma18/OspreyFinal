using Osprey3.ViewModels;
using Xamarin.Forms;

namespace Osprey3.Views
{
    public partial class Registration : ContentPage
    {
        public Registration(RegistrationViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // Set the BindingContext to the injected ViewModel
        }
    }
}