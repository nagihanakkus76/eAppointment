using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.GetAllDoctorByDepartment
{
    internal sealed class GetAllDoctorByDepartmentQueryHandler : IRequestHandler<GetAllDoctorByDepartmentQuery, Result<List<Doctor>>>
    {
        private readonly IDoctorRepository _repository;

        public GetAllDoctorByDepartmentQueryHandler(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Doctor>>> Handle(GetAllDoctorByDepartmentQuery request, CancellationToken cancellationToken)
        {
            List<Doctor> doctors =  await _repository
                .Where(p=>p.Department == request.DepartmentValue)
                .OrderBy(p=>p.FirstName)
                .ToListAsync(cancellationToken);
            return doctors;
        }
    }
}
