using Osprey3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Osprey3.Services
{
    public interface IEventService
    {
        Task<List<Event>> GetEventsAsync();
        Task<Event> GetEventAsync(int id);
        Task AddEventAsync(Event eventItem);
        Task UpdateEventAsync(Event eventItem);
        Task DeleteEventAsync(int id);
    }
}