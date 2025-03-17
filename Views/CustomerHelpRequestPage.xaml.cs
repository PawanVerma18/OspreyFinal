using Xamarin.Forms;
using Osprey3.ViewModels;

namespace Osprey3.Views
{
    public partial class CustomerHelpRequestPage : ContentPage
    {
        private readonly CustomerHelpRequestViewModel _viewModel;

        public CustomerHelpRequestPage(CustomerHelpRequestViewModel viewModel)
        {
            InitializeComponent();

            // Initialize the ViewModel
            _viewModel = viewModel;

            // Set the BindingContext to the ViewModel
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}