using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.DeleteCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/cliente")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class ClienteController : Controller
    {
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create(
               [FromBody] CreateClienteModel model,
               [FromServices] ICreateClienteCommand createClienteCommand,
               [FromServices] IValidator<CreateClienteModel> validator
               )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var cliente = await createClienteCommand.Execute(model);

            return StatusCode(cliente.StatusCode, cliente);

        }

        [AllowAnonymous]
        [HttpPut("update")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateClienteModel model,
            [FromServices] IUpdateClienteCommand updateClienteCommand,
            [FromServices] IValidator<UpdateClienteModel> validator
            )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(
                    ResponseApiService.Response(StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var clienteUpdate = await updateClienteCommand.Execute(model);

            return StatusCode(clienteUpdate.StatusCode, clienteUpdate);
        }



        [AllowAnonymous]
        [HttpDelete("delete/{clienteId}")]
        public async Task<IActionResult> Delete(
           int clienteId,
           [FromServices] IDeleteClienteCommand deleteClienteCommand
           )
        {
            if (clienteId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var data = await deleteClienteCommand.Execute(clienteId);

            return StatusCode(data.StatusCode, data);
        }
    }
}
