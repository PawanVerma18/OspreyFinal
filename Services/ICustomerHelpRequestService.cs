using Osprey3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public interface ICustomerHelpRequestService
    {
        Task<List<CustomerHelpRequest>> GetCustomerHelpRequestsAsync();
        Task AddCustomerHelpRequestAsync(CustomerHelpRequest request);
    }
}