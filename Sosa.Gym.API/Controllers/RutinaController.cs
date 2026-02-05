using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.AsignarRutina.Commands.AsignarRutina;
using Sosa.Gym.Application.DataBase.AsignarRutina.Commands.DesasignarRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.CreateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.DeleteRutina;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetAsignacionesAdminByRutinaId;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaAdmin;
using Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaDetalleAdmin;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("api/v1/rutinas")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class RutinasController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetRutinasAdminQuery query,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await query.Execute(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{rutinaId:int}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int rutinaId,
            [FromServices] IGetRutinaAdminDetalleQuery query)
        {
            if (rutinaId <= 0)
                return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));

            var result = await query.Execute(rutinaId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRutinaModel model,
            [FromServices] ICreateRutinaCommand command,
            [FromServices] IValidator<CreateRutinaModel> validator)
        {
            var validation = await validator.ValidateAsync(model);
            if (!validation.IsValid)
                return BadRequest(ResponseApiService.Response(400, validation.Errors));

            var result = await command.Execute(model);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("{rutinaId:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int rutinaId,
            [FromBody] UpdateRutinaModel model,
            [FromServices] IUpdateRutinaCommand command,
            [FromServices] IValidator<UpdateRutinaModel> validator)
        {
            if (rutinaId <= 0)
                return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));

            var validation = await validator.ValidateAsync(model);
            if (!validation.IsValid)
                return BadRequest(ResponseApiService.Response(400, validation.Errors));

            var result = await command.Execute(rutinaId, model);
            return StatusCode(result.StatusCode, result);
        }


        [HttpDelete("{rutinaId:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int rutinaId,
            [FromServices] IDeleteRutinaCommand command)
        {
            if (rutinaId <= 0)
                return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));

            var result = await command.Execute(rutinaId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("{rutinaId:int}/asignaciones/{clienteId:int}")]
        public async Task<IActionResult> Asignar(
            [FromRoute] int rutinaId,
            [FromRoute] int clienteId,
            [FromServices] IAsignarRutinaCommand command)
        {
            if (rutinaId <= 0) return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));
            if (clienteId <= 0) return BadRequest(ResponseApiService.Response(400, "ClienteId inválido"));

            var result = await command.Execute(rutinaId, clienteId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpDelete("{rutinaId:int}/asignaciones/{clienteId:int}")]
        public async Task<IActionResult> Desasignar(
            [FromRoute] int rutinaId,
            [FromRoute] int clienteId,
            [FromServices] IDesasignarRutinaCommand command)
        {
            if (rutinaId <= 0) return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));
            if (clienteId <= 0) return BadRequest(ResponseApiService.Response(400, "ClienteId inválido"));

            var result = await command.Execute(rutinaId, clienteId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{rutinaId:int}/asignaciones")]
        public async Task<IActionResult> GetAsignacionesDeRutina(
            [FromRoute] int rutinaId,
            [FromServices] IGetAsignacionesAdminByRutinaIdQuery query)
        {
            if (rutinaId <= 0)
                return BadRequest(ResponseApiService.Response(400, "RutinaId inválido"));

            var result = await query.Execute(rutinaId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
