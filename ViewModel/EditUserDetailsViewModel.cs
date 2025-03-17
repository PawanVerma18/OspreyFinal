using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Osprey3.Models;
using Osprey3.Services;
using System.Diagnostics;

namespace Osprey3.ViewModels
{
    public class EditUserDetailsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly INavigation _navigation;
        private User _user;

        public EditUserDetailsViewModel(IUserService userService, INavigation navigation, User user)
        {
            _userService = userService;
            _navigation = navigation;
            _user = user;

            SaveChangesCommand = new Command(async () => await OnSaveChangesAsync());
        }

        // Properties for binding to the UI
        public string Name
        {
            get => _user?.Name;
            set
            {
                if (_user != null)
                {
                    _user.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Email
        {
            get => _user?.Email;
            set
            {
                if (_user != null)
                {
                    _user.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string PhoneNumber
        {
            get => _user?.PhoneNumber;
            set
            {
                if (_user != null)
                {
                    _user.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public DateTime DateOfBirth
        {
            get => _user?.DateOfBirth ?? DateTime.Now;
            set
            {
                if (_user != null)
                {
                    _user.DateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                }
            }
        }

        public string Gender
        {
            get => _user?.Gender;
            set
            {
                if (_user != null)
                {
                    _user.Gender = value;
                    OnPropertyChanged(nameof(Gender));
                }
            }
        }

        public string Address
        {
            get => _user?.Address;
            set
            {
                if (_user != null)
                {
                    _user.Address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public List<string> Genders => new List<string> { "Male", "Female", "Other" };

        // Command for saving changes
        public ICommand SaveChangesCommand { get; }

        // Save changes logic (without validation)
        private async Task OnSaveChangesAsync()
        {
            try
            {
                // Update the user details
                bool isUpdated = await _userService.UpdateUserAsync(_user);

                if (isUpdated)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "User details updated successfully.", "OK");
                    await _navigation.PopAsync(); // Navigate back
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update user details.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating user details: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while updating user details.", "OK");
            }
        }
    }
}