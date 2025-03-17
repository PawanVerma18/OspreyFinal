using Xamarin.Forms;
using Osprey3.ViewModels;

namespace Osprey3.Views
{
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage(ResetPasswordViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}