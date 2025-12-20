using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.DeleteCliente;
using Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteAdmin;
using Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;
using System.Security.Claims;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/cliente")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManager))]
    public class ClienteController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateClienteModel model,
            [FromServices] ICreateClienteCommand createClienteCommand,
            [FromServices] IValidator<CreateClienteModel> validator)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var result = await createClienteCommand.Execute(model);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Cliente")]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe(
                [FromBody] UpdateClienteModel model,
                [FromServices] IUpdateClienteCommand updateClienteCommand,
                [FromServices] IValidator<UpdateClienteModel> validator,
                [FromServices] IDataBaseService dataBaseService
)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(
                    ResponseApiService.Response(StatusCodes.Status400BadRequest, validationResult.Errors));
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(ResponseApiService.Response(StatusCodes.Status401Unauthorized, "Token inválido"));
            }

            // Buscar el cliente del usuario logueado
            var clienteId = await dataBaseService.Clientes
                .Where(c => c.UsuarioId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (clienteId == 0)
            {
                return NotFound(ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado"));
            }


            var esAdmin = User.IsInRole("Administrador");
            var result = await updateClienteCommand.Execute(clienteId, model, userId, esAdmin);


            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPut("{clienteId:int}")]
        public async Task<IActionResult> UpdateByAdmin(
                [FromRoute] int clienteId,
                [FromBody] UpdateClienteModel model,
                [FromServices] IUpdateClienteCommand updateClienteCommand,
                [FromServices] IValidator<UpdateClienteModel> validator
)
        {

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(
                    ResponseApiService.Response(
                        StatusCodes.Status400BadRequest,
                        validationResult.Errors));
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(
                    ResponseApiService.Response(
                        StatusCodes.Status401Unauthorized,
                        "Token inválido"));
            }

            var esAdmin = User.IsInRole("Administrador"); 
            var result = await updateClienteCommand.Execute(clienteId, model, userId, esAdmin);


            return StatusCode(result.StatusCode, result);
        }




        [Authorize(Roles = "Administrador")]
        [HttpDelete("{clienteId:int}")]
        public async Task<IActionResult> Delete(
             [FromRoute] int clienteId,
             [FromServices] IDeleteClienteCommand deleteClienteCommand)
        {
            if (clienteId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "Id inválido"));
            }

            var result = await deleteClienteCommand.Execute(clienteId);
            return StatusCode(result.StatusCode, result);
        }



        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GetAll(
             [FromServices] IGetAllClientesQuery getAllClientesQuery,
             [FromQuery] int pageNumber = 1,
             [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var data = await getAllClientesQuery.Execute(pageNumber, pageSize);

            return Ok(ResponseApiService.Response(StatusCodes.Status200OK, data));
        }



        [Authorize(Roles = "Cliente")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe(
          [FromServices] IGetClienteQuery query)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(ResponseApiService.Response(
                    StatusCodes.Status401Unauthorized,
                    "Token inválido"));
            }

            var result = await query.Execute(userId);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{clienteId:int}")]
        public async Task<IActionResult> GetById(
        [FromRoute] int clienteId,
        [FromServices] IGetClienteByIdAdminQuery query)
        {
            if (clienteId <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "Id inválido"));
            }

            var result = await query.Execute(clienteId);
            return StatusCode(result.StatusCode, result);
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet("dni/{dni:long}")]
        public async Task<IActionResult> GetByDni(
              [FromRoute] long dni,
              [FromServices] IGetClienteByDniQuery getClienteByDniQuery)
        {
            if (dni <= 0)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    "DNI inválido"));
            }

            var result = await getClienteByDniQuery.Execute(dni);
            return StatusCode(result.StatusCode, result);
        }
    }
}
