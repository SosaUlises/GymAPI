using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.DeleteEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio;
using Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Application.Features;

namespace Sosa.Gym.API.Controllers
{
    [Route("/api/v1/ejercicio")]
    [ApiController]
    [Authorize(Roles = "Cliente")]
    [TypeFilter(typeof(ExceptionManager))]
    public class EjercicioController : Controller
    {

        [HttpPost("create")]
        public async Task<IActionResult> Create(
                 [FromBody] CreateEjercicioModel model,
                 [FromServices] ICreateEjercicioCommand createEjercicioCommand,
                 [FromServices] IValidator<CreateEjercicioModel> validator
                 )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var ejercicio = await createEjercicioCommand.Execute(model);

            return StatusCode(ejercicio.StatusCode, ejercicio);

        }


        [HttpPut("update")]
        public async Task<IActionResult> Update(
                [FromBody] UpdateEjercicioModel model,
                [FromServices] IUpdateEjercicioCommand updateEjercicioCommand,
                [FromServices] IValidator<UpdateEjercicioModel> validator
                )
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                return BadRequest(ResponseApiService.Response(
                    StatusCodes.Status400BadRequest,
                    validationResult.Errors));
            }

            var ejercicio = await updateEjercicioCommand.Execute(model);

            return StatusCode(ejercicio.StatusCode, ejercicio);

        }


        [HttpDelete("delete/{ejercicioId}")]
        public async Task<IActionResult> Delete(
            int ejercicioId,
            [FromServices] IDeleteEjercicioCommand deleteEjercicioCommand
            )
        {
            if(ejercicioId == 0)
            return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest));

            var ejercicio = await deleteEjercicioCommand.Execute(ejercicioId);

            return StatusCode(ejercicio.StatusCode, ejercicio);
            
        }



        [HttpGet("getByDiaRutina/{diaRutinaId}")]
        public async Task<IActionResult> GetByDiaRutina(
            int diaRutinaId,
            [FromServices] IGetEjerciciosByDiaRutinaQuery getEjerciciosByDiaRutinaQuery
            )
        {
            if (diaRutinaId == 0)
                return BadRequest(ResponseApiService.Response(StatusCodes.Status400BadRequest));

            var ejercicios = await getEjerciciosByDiaRutinaQuery.Execute(diaRutinaId);

            return StatusCode(ejercicios.StatusCode, ejercicios);

        }
    }
}
