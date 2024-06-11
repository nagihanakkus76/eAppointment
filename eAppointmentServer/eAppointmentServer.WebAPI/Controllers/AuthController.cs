using eAppointmentServer.Application.Features.Auth.Login;
using eAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eAppointmentServer.WebAPI.Controllers
{
    [AllowAnonymous]//Authorize atribute ezip çalışmasını engeller
    public sealed class AuthController : ApiController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
