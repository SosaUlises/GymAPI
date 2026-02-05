using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetAsignacionesAdminByRutinaId
{
    public class GetAsignacionesAdminByRutinaIdQuery : IGetAsignacionesAdminByRutinaIdQuery
    {
        private readonly IDataBaseService _db;

        public GetAsignacionesAdminByRutinaIdQuery(IDataBaseService db)
        {
            _db = db;
        }

        public async Task<BaseResponseModel> Execute(int rutinaId)
        {
            if (rutinaId <= 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "RutinaId inválido");

            var rutinaExiste = await _db.Rutinas.AsNoTracking().AnyAsync(r => r.Id == rutinaId);
            if (!rutinaExiste)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Rutina no encontrada");

            var items = await _db.RutinasAsignadas
                .AsNoTracking()
                .Where(x => x.RutinaId == rutinaId)
                .OrderByDescending(x => x.FechaAsignacion)
                .Select(x => new GetAsignacionAdminItemModel
                {
                    RutinaAsignadaId = x.Id,
                    ClienteId = x.ClienteId,
                    Dni = x.Cliente.Usuario.Dni,
                    Nombre = x.Cliente.Usuario.Nombre,
                    Apellido = x.Cliente.Usuario.Apellido,
                    FechaAsignacion = x.FechaAsignacion
                })
                .ToListAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, items);
        }
    }
}
