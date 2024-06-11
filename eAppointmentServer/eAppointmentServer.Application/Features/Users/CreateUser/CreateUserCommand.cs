using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.CreateUser
{
    public sealed record CreateUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string UserName,
        string Password,
        List<Guid> RoleIds
        ): IRequest<Result<string>>;


    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(UserManager<AppUser> userManager, IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.Users.AnyAsync(p => p.Email == request.Email))
            {
                return Result<string>.Failure("Email already exists");
            }

            if (await _userManager.Users.AnyAsync(p => p.UserName == request.UserName))
            {
                return Result<string>.Failure("User name already exists");
            }

            AppUser user = _mapper.Map<AppUser>(request);
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Result<string>.Failure(result.Errors.Select(s => s.Description).ToList());
            }


            if (request.RoleIds.Any())
            {
                List<AppUserRole> userRoles = new();
                foreach (var roleId in request.RoleIds)
                {
                    AppUserRole userRole = new()
                    {
                        RoleId = roleId,
                        UserId = user.Id
                    };
                    userRoles.Add(userRole);
                }

                await _userRoleRepository.AddRangeAsync(userRoles, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return "User create is successful";

        }
    }
}
