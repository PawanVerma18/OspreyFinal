using Osprey3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetPromotionsAsync();
        Task<Promotion> GetPromotionAsync(int id);
        Task AddPromotionAsync(Promotion promotion);
        Task UpdatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(int id);
    }
}