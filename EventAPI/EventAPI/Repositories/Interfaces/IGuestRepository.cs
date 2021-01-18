using EventAPI.DomainModels;
using EventAPI.Entities;
using System.Threading.Tasks;

namespace EventAPI.Repositories.Interfaces
{
    public interface IGuestRepository : IBaseRepository<Guest, GuestModel>
    {
        Task<GuestModel> GetGuestAsync(GuestModel model);
        Task<GuestModel> GetGuestByEmailAsync(string email);
    }
}
