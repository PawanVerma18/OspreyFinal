using Osprey3.Services;
using Osprey3.ViewModels;
using Xamarin.Forms;

namespace Osprey3.Views
{
    public partial class ViewReportsPage : ContentPage
    {
        public ViewReportsPage()
        {
            InitializeComponent();

            // Resolve ViewModel with dependencies
            var userService = Startup.Resolve<IUserService>();
            BindingContext = new ViewReportsViewModel(userService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load data when the page appears
            if (BindingContext is ViewReportsViewModel viewModel)
            {
                viewModel.LoadTalentApplicationsCommand.Execute(null);
            }
        }
    }
}