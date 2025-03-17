using Microsoft.Extensions.DependencyInjection;
using Osprey3.Services;
using Osprey3.ViewModels;
using System;
using System.Net.Http;

namespace Osprey3
{
    public static class Startup
    {
        private static IServiceProvider _serviceProvider;

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register HttpClient with base address and headers
            services.AddHttpClient<IUserService, ApiUserService>(client =>
            {
                client.BaseAddress = new Uri("https://192.168.56.1:7035/api/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .ConfigurePrimaryHttpMessageHandler(() => HttpClientFactory.CreateHandler());
            // Register other services
            services.AddSingleton<EmailService>(sp =>
                new EmailService(
                   smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                smtpUsername: "joinosprey@gmail.com",
                smtpPassword: "zpmn eppn xkll wind",
                enableSsl: true
                ));

            // Register other services
            services.AddSingleton<EmailService>();
            services.AddSingleton<ICustomerHelpRequestService, CustomerHelpRequestService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IPromotionService, PromotionService>();
            services.AddSingleton<IFundingService, FundingService>();

            // Register ViewModels
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<CustomerHelpRequestViewModel>();
            services.AddTransient<ApplyForTalentViewModel>();
            services.AddTransient<CustomerHelpRequestViewModel>();
            services.AddTransient<AdminPageViewModel>();

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => _serviceProvider.GetRequiredService<T>();
    }
}