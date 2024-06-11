using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.DeleteDoctor
{
    internal sealed class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, Result<string>>
    {
        private readonly IDoctorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDoctorCommandHandler(IDoctorRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor? doctor = await _repository.GetByExpressionAsync(p => p.ID == request.id, cancellationToken);
            if (doctor is null) 
            {
                return Result<string>.Failure("Doctor not found");
            }

            _repository.Delete(doctor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return "Doctor delete is successful";
        }
    }
}
