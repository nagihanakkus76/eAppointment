using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.GetPatientByIdentityNumber
{
    internal sealed class GetPatientByIdentityNumberQueryHandler : IRequestHandler<GetPatientByIdentityNumberQuery, Result<Patient>>
    {
        private readonly IPatientRepository _repository;

        public GetPatientByIdentityNumberQueryHandler(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Patient>> Handle(GetPatientByIdentityNumberQuery request, CancellationToken cancellationToken)
        {
           Patient? patient = await _repository.GetByExpressionAsync(p=>p.IdentityNumber == request.IdentityNumber,cancellationToken);
            return patient;
        }
    }
}
