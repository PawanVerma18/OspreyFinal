using Xamarin.Forms;
using Osprey3.ViewModels;

namespace Osprey3.Views
{
    public partial class VerifyCodePage : ContentPage
    {
        public VerifyCodePage(VerifyCodeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; // Set the BindingContext to the ViewModel
        }
    }
}