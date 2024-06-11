using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Patients.CreatePatient
{
    internal sealed class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<string>>
    {
        private readonly IPatientRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreatePatientCommandHandler(IPatientRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            if (_repository.Any(p=>p.IdentityNumber == request.IdentityNumber))
            {
                return Result<string>.Failure("This identity number already use");
            }

            Patient patient = _mapper.Map<Patient>(request);

            await _repository.AddAsync(patient,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return "Patient create is successful";
        }
    }
}
