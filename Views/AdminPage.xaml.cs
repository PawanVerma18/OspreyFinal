using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Osprey3.ViewModels;
using Osprey3.Services;
using System.Net.Http;

namespace Osprey3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : ContentPage
    {
        public AdminPage()
        {
            InitializeComponent();

            // Create HttpClient with custom handler to bypass SSL validation
            var httpClient = HttpClientFactory.CreateHttpClient();

            // Initialize services
            var eventService = new EventService(httpClient);
            var promotionService = new PromotionService(httpClient);
            var fundingService = new FundingService(httpClient);

            // Pass the Navigation property (INavigation) to the ViewModel
            BindingContext = new AdminPageViewModel(Navigation, eventService, promotionService, fundingService);
        }
    }
}