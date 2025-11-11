using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.DeleteCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni;
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

        [AllowAnonymous]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllClientesQuery getAllClientesQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var data = await getAllClientesQuery.Execute(pageNumber, pageSize);

            if (!data.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    ResponseApiService.Response(StatusCodes.Status404NotFound));
            }

            return StatusCode(StatusCodes.Status200OK,
                ResponseApiService.Response(StatusCodes.Status200OK, data));

        }

        [AllowAnonymous]
        [HttpGet("getById/{clienteId}")]
        public async Task<IActionResult> GetById(
            int clienteId,
          [FromServices] IGetClienteByIdQuery getClienteByIdQuery)
        {

            if (clienteId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    ResponseApiService.Response(StatusCodes.Status400BadRequest));
            }

            var data = await getClienteByIdQuery.Execute(clienteId);

            return StatusCode(data.StatusCode, data);

        }
    }
}
