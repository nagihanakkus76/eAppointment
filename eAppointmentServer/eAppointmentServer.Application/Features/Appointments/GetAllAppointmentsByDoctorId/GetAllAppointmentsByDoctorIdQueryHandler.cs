using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.GetAllAppointments
{
    internal sealed class GetAllAppointmentsByDoctorIdQueryHandler : IRequestHandler<GetAllAppointmentsByDoctorIdQuery, Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>>
    {
        private readonly IAppointmentRepository _repository;

        public GetAllAppointmentsByDoctorIdQueryHandler(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<GetAllAppointmentsByDoctorIdQueryResponse>>> Handle(GetAllAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            List<Appointment> appointments = await _repository
                .Where(p => p.DoctorID == request.DoctorId)
                .Include(p=>p.Patient)
                .ToListAsync(cancellationToken);

            List<GetAllAppointmentsByDoctorIdQueryResponse> response = appointments.Select(s=> 
            new GetAllAppointmentsByDoctorIdQueryResponse(
                s.ID,
                s.StartDate,
                s.EndDate,
                s.Patient!.FullName,
                s.Patient
                )).ToList();   
            
            return response;
        }
    }
}
