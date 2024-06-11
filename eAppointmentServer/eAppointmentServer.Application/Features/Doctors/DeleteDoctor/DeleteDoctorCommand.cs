using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Doctors.DeleteDoctor
{
    public sealed record DeleteDoctorCommand(Guid id) : IRequest<Result<string>>;
}
