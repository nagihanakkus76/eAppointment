﻿using eAppointmentServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Roles.RoleSync
{
    internal sealed class RoleSyncCommandHandler : IRequestHandler<RoleSyncCommand, Result<string>>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleSyncCommandHandler(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<string>> Handle(RoleSyncCommand request, CancellationToken cancellationToken)
        {
            List<AppRole> currentRoles =await _roleManager.Roles.ToListAsync(cancellationToken);
            List<AppRole> staticRoles = Constants.GetRoles();
            foreach (var role in currentRoles) 
            {
                if (!staticRoles.Any(p=>p.Name == role.Name))
                {
                    await _roleManager.DeleteAsync(role); 
                }
            }
            foreach (var role in staticRoles)
            {
                if (!currentRoles.Any(p=>p.Name == role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            return " Sync is successful";

        }
    }
}
