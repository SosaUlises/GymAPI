using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinasAsignadasAdminByCliente
{
    public class GetRutinasAsignadasAdminByClienteIdQuery : IGetRutinasAsignadasAdminByClienteIdQuery
    {
        private readonly IDataBaseService _db;

        public GetRutinasAsignadasAdminByClienteIdQuery(IDataBaseService db)
        {
            _db = db;
        }

        public async Task<BaseResponseModel> Execute(int clienteId)
        {
            if (clienteId <= 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "ClienteId inválido");

            var clienteExiste = await _db.Clientes.AsNoTracking().AnyAsync(c => c.Id == clienteId);
            if (!clienteExiste)
                return ResponseApiService.Response(StatusCodes.Status404NotFound, "Cliente no encontrado");

            var items = await _db.RutinasAsignadas
                .AsNoTracking()
                .Where(x => x.ClienteId == clienteId)
                .OrderByDescending(x => x.FechaAsignacion)
                .Select(x => new GetRutinaAsignadaAdminItemModel
                {
                    RutinaAsignadaId = x.Id,
                    RutinaId = x.RutinaId,
                    Nombre = x.Rutina.Nombre,
                    Descripcion = x.Rutina.Descripcion,
                    FechaAsignacion = x.FechaAsignacion
                })
                .ToListAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, items);
        }
    }
}
