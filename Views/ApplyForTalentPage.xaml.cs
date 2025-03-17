using Osprey3.Services;
using Osprey3.ViewModels;
using Xamarin.Forms;

namespace Osprey3.Views
{
    public partial class ApplyForTalentPage : ContentPage
    {
        public ApplyForTalentPage()
        {
            InitializeComponent();

            // Resolve ViewModel with dependencies
            var userService = Startup.Resolve<IUserService>();
            BindingContext = new ApplyForTalentViewModel(userService);
        }
    }
}