using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<GetAllUsersQueryResponse>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly RoleManager<AppRole> _roleManager;
        public GetAllUsersQueryHandler(UserManager<AppUser> userManager, IUserRoleRepository userRoleRepository, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
            _roleManager = roleManager;
        }

        public async Task<Result<List<GetAllUsersQueryResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            List<AppUser> users = await _userManager.Users.OrderBy(p => p.FirstName).ToListAsync(cancellationToken);

            List<GetAllUsersQueryResponse> response =
                users.Select(s => new GetAllUsersQueryResponse()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    FullName = s.FullName,
                    UserName = s.UserName,
                    Email = s.Email
                }).ToList();

            foreach (var item in response)
            {
                List<AppUserRole> userRoles = await _userRoleRepository.Where(p => p.UserId == item.Id).ToListAsync(cancellationToken);

                List<Guid> stringRoles = new();
                List<string?> stringRoleNames = new();

                foreach (var userRole in userRoles)
                {
                    AppRole? role =
                        await _roleManager
                        .Roles
                        .Where(p => p.Id == userRole.RoleId)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (role is not null)
                    {
                        stringRoles.Add(role.Id);
                        stringRoleNames.Add(role.Name);
                    }
                }

                item.RoleIds = stringRoles;
                item.RoleNames = stringRoleNames;
            }

            return response;
        }
    }
}
