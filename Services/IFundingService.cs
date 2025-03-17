using Osprey3.Models;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public interface IFundingService
    {
        Task<decimal> GetTotalFundingAsync();
        Task AddContributionAsync(Funding contribution);
    }
}