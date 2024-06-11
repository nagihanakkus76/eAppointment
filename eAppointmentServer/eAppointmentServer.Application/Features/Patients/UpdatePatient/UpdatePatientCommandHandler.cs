using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.UpdatePatient
{
    internal sealed class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<string>>
    {
        private readonly IPatientRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePatientCommandHandler(IPatientRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            Patient? patient = await _repository.GetByExpressionWithTrackingAsync(p => p.ID == request.Id, cancellationToken);
            if (patient is null) { return Result<string>.Failure("Patient not found"); }
            if (patient.IdentityNumber != request.IdentityNumber)
            {
                if (_repository.Any(p=>p.IdentityNumber == request.IdentityNumber))
                {
                    return Result<string>.Failure("This identity number already use");
                }
            }
            _mapper.Map(request, patient);
            _repository.Update(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return "Patient update is successful";
        }
    }
}
