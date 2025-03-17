using Osprey3.Views;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Osprey3.ViewModel
{
    public class MainScreenViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        // Commands for buttons
        public ICommand TalentApplyCommand { get; }
        public ICommand ExecutiveApplyCommand { get; }
        public ICommand CollegeWaitlistCommand { get; }
        public ICommand HelpCommand { get; }

        public MainScreenViewModel()
        {
            // Initialize commands
            TalentApplyCommand = new Command(OnTalentApplyClicked);
          

            
        }

        private async void OnTalentApplyClicked()
        {
            // Navigate to Talent Apply Page
            await Application.Current.MainPage.Navigation.PushAsync(new ApplyForTalentPage());
        }

      
      

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}