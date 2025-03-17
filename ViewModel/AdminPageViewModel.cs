using Osprey3.Models;
using Osprey3.Services;
using Osprey3.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Osprey3.ViewModels
{
    public class AdminPageViewModel : BaseViewModel
    {
        public ICommand ManageUsersCommand { get; }
        public ICommand ViewReportsCommand { get; }
        public ICommand BackCommand { get; }

        private readonly INavigation _navigation;

        // Services
        private readonly IEventService _eventService;
        private readonly IPromotionService _promotionService;
        private readonly IFundingService _fundingService;

        // Collections
        public ObservableCollection<Event> Events { get; }
        public ObservableCollection<Promotion> Promotions { get; }
        public ObservableCollection<Contributor> Contributors { get; } // New: For funding contributors

        // Properties
        public Event NewEvent { get; set; } = new Event();
        public Promotion NewPromotion { get; set; } = new Promotion();
        public decimal TotalFunding { get; private set; }
        public decimal FundingGoal { get; private set; } = 10000; // New: Example funding goal
        public decimal FundingProgress => FundingGoal > 0 ? TotalFunding / FundingGoal : 0; // New: Progress calculation

        // Commands
        public ICommand LoadEventsCommand { get; }
        public ICommand LoadPromotionsCommand { get; }
        public ICommand LoadFundingCommand { get; }
        public ICommand AddEventCommand { get; }
        public ICommand AddPromotionCommand { get; }
        public Command<Event> EditEventCommand { get; }
        public Command<Promotion> EditPromotionCommand { get; }
        public Command<Event> DeleteEventCommand { get; }
        public Command<Promotion> DeletePromotionCommand { get; }
        public ICommand AddContributionCommand { get; } // New: Command to add a contribution

        public AdminPageViewModel(INavigation navigation, IEventService eventService, IPromotionService promotionService, IFundingService fundingService)
        {
            _navigation = navigation;
            _eventService = eventService;
            _promotionService = promotionService;
            _fundingService = fundingService;

            // Initialize collections
            Events = new ObservableCollection<Event>();
            Promotions = new ObservableCollection<Promotion>();
            Contributors = new ObservableCollection<Contributor>(); // New: Initialize contributors list

            // Initialize navigation commands
            ManageUsersCommand = new Command(OnManageUsersClicked);
            ViewReportsCommand = new Command(OnViewReportsClicked);
            BackCommand = new Command(OnBackClicked);

            // Initialize service-related commands
            LoadEventsCommand = new Command(async () => await LoadEventsAsync());
            LoadPromotionsCommand = new Command(async () => await LoadPromotionsAsync());
            LoadFundingCommand = new Command(async () => await LoadFundingAsync());
            AddEventCommand = new Command(async () => await AddEventAsync());
            AddPromotionCommand = new Command(async () => await AddPromotionAsync());
            DeleteEventCommand = new Command<Event>(async (e) => await DeleteEventAsync(e));
            DeletePromotionCommand = new Command<Promotion>(async (p) => await DeletePromotionAsync(p));
            AddContributionCommand = new Command(async () => await AddContributionAsync()); // New: Initialize contribution command

            // Load initial data
            LoadData();
        }

        private async void OnManageUsersClicked()
        {
            // Navigate to ManageUsersPage
            await _navigation.PushAsync(new ManageUsersPage());
        }

        private async void OnViewReportsClicked()
        {
            // Navigate to ViewReportsPage
            await _navigation.PushAsync(new ViewReportsPage());
        }

        private async void OnBackClicked()
        {
            // Navigate back to MainScreen
            await _navigation.PopAsync();
        }

        private async Task LoadData()
        {
            await LoadEventsAsync();
            await LoadPromotionsAsync();
            await LoadFundingAsync();
        }

        private async Task LoadEventsAsync()
        {
            var events = await _eventService.GetEventsAsync();
            Events.Clear();
            foreach (var e in events)
            {
                Events.Add(e);
            }
        }

        private async Task LoadPromotionsAsync()
        {
            var promotions = await _promotionService.GetPromotionsAsync();
            Promotions.Clear();
            foreach (var p in promotions)
            {
                Promotions.Add(p);
            }
        }

        private async Task LoadFundingAsync()
        {
            TotalFunding = await _fundingService.GetTotalFundingAsync();
            OnPropertyChanged(nameof(TotalFunding));
            OnPropertyChanged(nameof(FundingProgress)); // New: Notify UI of progress update
        }

        private async Task AddEventAsync()
        {
            if (string.IsNullOrWhiteSpace(NewEvent.EventName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Event name is required.", "OK");
                return;
            }

            try
            {
                await _eventService.AddEventAsync(NewEvent); // Assuming this method is void
                await Application.Current.MainPage.DisplayAlert("Success", "Event added successfully!", "OK");
                await LoadEventsAsync();

                // Reset the form
                NewEvent = new Event();
                OnPropertyChanged(nameof(NewEvent));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add the event: {ex.Message}", "OK");
            }
        }

        private async Task AddPromotionAsync()
        {
            if (string.IsNullOrWhiteSpace(NewPromotion.PromotionName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Promotion name is required.", "OK");
                return;
            }

            try
            {
                await _promotionService.AddPromotionAsync(NewPromotion); // Assuming this method is void
                await Application.Current.MainPage.DisplayAlert("Success", "Promotion added successfully!", "OK");
                await LoadPromotionsAsync();

                // Reset the form
                NewPromotion = new Promotion();
                OnPropertyChanged(nameof(NewPromotion));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add the promotion: {ex.Message}", "OK");
            }
        }

        private async Task DeleteEventAsync(Event eventItem)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this event?", "Yes", "No");
            if (confirm)
            {
                await _eventService.DeleteEventAsync(eventItem.Id);
                await LoadEventsAsync();
            }
        }

        private async Task DeletePromotionAsync(Promotion promotion)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this promotion?", "Yes", "No");
            if (confirm)
            {
                await _promotionService.DeletePromotionAsync(promotion.Id);
                await LoadPromotionsAsync();
            }
        }

        // New: Method to add a contribution
        private async Task AddContributionAsync()
        {
            string contributorName = await Application.Current.MainPage.DisplayPromptAsync("Contribute", "Enter your name:");
            if (string.IsNullOrWhiteSpace(contributorName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Name is required.", "OK");
                return;
            }

            string amountInput = await Application.Current.MainPage.DisplayPromptAsync("Contribute", "Enter amount to contribute:", keyboard: Keyboard.Numeric);
            if (!decimal.TryParse(amountInput, out decimal amount) || amount <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid amount.", "OK");
                return;
            }

            // Add the contribution
            TotalFunding += amount;
            Contributors.Add(new Contributor
            {
                ContributorName = contributorName,
                ContributionAmount = amount,
                ContributionDate = DateTime.Now
            });

            // Notify UI of changes
            OnPropertyChanged(nameof(TotalFunding));
            OnPropertyChanged(nameof(FundingProgress));

            await Application.Current.MainPage.DisplayAlert("Success", $"Thank you for contributing {amount:C}!", "OK");
        }
    }

    // New: Contributor model for funding
    public class Contributor
    {
        public string ContributorName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ContributionDate { get; set; }
    }
}