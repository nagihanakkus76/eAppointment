using AutoMapper;
using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.UpdateDoctor
{
    internal sealed class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Result<string>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDoctorCommandHandler(IDoctorRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor? doctor =  await _repository.GetByExpressionWithTrackingAsync(p => p.ID == request.id, cancellationToken);
            if (doctor is null) 
            {
                return Result<string>.Failure("Doctor not found");
            }

            _mapper.Map(request, doctor);
            _repository.Update(doctor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return "Doctor update is successful";
        }
    }
}
