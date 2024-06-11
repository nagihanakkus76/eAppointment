using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Appointments.DeleteAppointmentById
{
    internal sealed class DeleteAppointmentByIdCommandHandler : IRequestHandler<DeleteAppointmentByIdCommand, Result<string>>
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAppointmentByIdCommandHandler(IAppointmentRepository appointmentsRepository, IUnitOfWork unitOfWork)
        {
            _appointmentsRepository = appointmentsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteAppointmentByIdCommand request, CancellationToken cancellationToken)
        {
            Appointment? appointment = await _appointmentsRepository.GetByExpressionAsync(p=> p.ID == request.Id, cancellationToken);
            if (appointment is null) 
            {
                return Result<string>.Failure("Appointment not found");
            }

            if (appointment.IsCompleted)
            {
                return Result<string>.Failure("You cannot delete a completed appointment");
            }

            _appointmentsRepository.Delete(appointment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return "Appointment delete is successful";

        }
    }
}
