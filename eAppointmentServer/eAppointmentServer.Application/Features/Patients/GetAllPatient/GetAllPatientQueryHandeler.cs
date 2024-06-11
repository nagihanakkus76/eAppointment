using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.GetAllPatient
{
    internal sealed class GetAllPatientQueryHandeler : IRequestHandler<GetAllPatientQuery, Result<List<Patient>>>
    {
        private readonly IPatientRepository _repository;

        public GetAllPatientQueryHandeler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Patient>>> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
        {
            List<Patient> patients = await _repository.GetAll().OrderBy(p=>p.FirstName).ToListAsync(cancellationToken);
            return patients;
        }
    }
}
