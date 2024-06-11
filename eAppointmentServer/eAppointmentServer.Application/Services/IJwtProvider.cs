using eAppointmentServer.Domain.Entities;

namespace eAppointmentServer.Application.Services
{
    public interface IJwtProvider
    {
        Task<string> CreatedTokenAsync(AppUser user);
    }
}
