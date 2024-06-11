using eAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.GetAllRolesForUsers
{
    internal sealed class GetAllRolesForUsersQueryHandler : IRequestHandler<GetAllRolesForUsersQuery, Result<List<AppRole>>>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public GetAllRolesForUsersQueryHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<List<AppRole>>> Handle(GetAllRolesForUsersQuery request, CancellationToken cancellationToken)
        {
           List<AppRole> roles = await _roleManager.Roles.OrderBy(p=>p.Name).ToListAsync(cancellationToken);
            return roles;
        }
    }
}
