using Osprey3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> CheckUserExistsAsync(string email, string password);
        Task<User> GetUserAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> RegisterUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task UpdateUserPasswordAsync(int userId, string newPassword);
        Task DeleteUserAsync(int id);
        Task SaveUserAsync(User user);

        // Password Reset and Verification Methods
        Task<bool> SendVerificationCodeAsync(string email, string code); // New method
        Task<bool> VerifyCodeAsync(string email, string code); // New method
        Task<bool> CheckEmailAsync(string email);

        // Talent Application Methods

        Task<IEnumerable<TalentApplication>> GetTalentApplicationsAsync();
        Task<bool> SubmitTalentApplicationAsync(TalentApplication talentApplication);
        Task<bool> UpdateTalentApplicationAsync(TalentApplication talentApplication);
        Task<bool> DeleteTalentApplicationAsync(int id);

     

        // Customer Help Request Methods
        Task<IEnumerable<CustomerHelpRequest>> GetCustomerHelpRequestsAsync();
        Task<bool> SubmitCustomerHelpRequestAsync(CustomerHelpRequest customerHelpRequest);
        Task<bool> UpdateCustomerHelpRequestAsync(CustomerHelpRequest customerHelpRequest);
        Task<bool> DeleteCustomerHelpRequestAsync(int id);
    }
}