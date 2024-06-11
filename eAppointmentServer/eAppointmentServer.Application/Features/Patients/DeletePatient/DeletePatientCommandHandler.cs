using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.DeletePatient
{
    internal sealed class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Result<string>>
    {
        private readonly IPatientRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePatientCommandHandler(IPatientRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
          
            Patient patient = await _repository.GetByExpressionAsync(p=>p.ID==request.Id,cancellationToken);

            if (patient is null) { return Result<string>.Failure("Patient not found"); }

            _repository.Delete(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return "Patient delete is successful";
       
        }
    }
}
