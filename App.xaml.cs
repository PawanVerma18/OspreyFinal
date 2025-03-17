using Osprey3.Services;
using Osprey3.ViewModels;
using Osprey3.Views;
using System.Net.Http;
using Xamarin.Forms;

namespace Osprey3
{
    public partial class App : Application
    {
        public static ICustomerHelpRequestService CustomerHelpRequestService { get; private set; }
        public static EmailService EmailService { get; private set; }

        public App()
        {
            InitializeComponent();

            // Configure services
            Startup.ConfigureServices();

            // Initialize services
            CustomerHelpRequestService = new CustomerHelpRequestService(HttpClientFactory.CreateHttpClient());
            EmailService = new EmailService(
               smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                smtpUsername: "joinosprey@gmail.com",
                smtpPassword: "zpmn eppn xkll wind",
                enableSsl: true
            );

            // Register services with DependencyService
            DependencyService.Register<EmailService>();
            DependencyService.Register<RegistrationViewModel>();
            DependencyService.Register<IEventService, EventService>();
            DependencyService.Register<ICustomerHelpRequestService, CustomerHelpRequestService>();

            // Resolve services
            var userService = Startup.Resolve<IUserService>();
            var eventService = Startup.Resolve<IEventService>();
            var promotionService = Startup.Resolve<IPromotionService>();
            var fundingService = Startup.Resolve<IFundingService>();
            var customerHelpRequestService = Startup.Resolve<ICustomerHelpRequestService>();

            // Set the main page
            MainPage = new NavigationPage(new MainScreen(userService));
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }
    }
}