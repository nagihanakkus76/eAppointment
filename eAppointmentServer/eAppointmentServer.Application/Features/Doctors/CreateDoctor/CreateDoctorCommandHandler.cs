using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.CreateDoctor
{
    internal sealed class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Result<string>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateDoctorCommandHandler(IDoctorRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor doctor = _mapper.Map<Doctor>(request);

           await _repository.AddAsync(doctor, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return "Doctor create is successful";
        }
    }
}
