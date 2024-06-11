using eAppointmentServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace eAppointmentServer.Application
{
    public static class Constants
    {
        public static List<AppRole> GetRoles()
        {
            List<string> roles = new()
            {
                "Admin",
                "Doctor",
                "Personel",
                "Patient"
            };
        
            return roles.Select(x => new AppRole() { Name = x}).ToList();
          
        }
    }
}
