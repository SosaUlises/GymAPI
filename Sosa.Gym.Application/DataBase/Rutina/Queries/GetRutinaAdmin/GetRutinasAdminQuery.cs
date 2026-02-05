using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaAdmin
{
    public class GetRutinasAdminQuery : IGetRutinasAdminQuery
    {
        private readonly IDataBaseService _db;

        public GetRutinasAdminQuery(IDataBaseService db)
        {
            _db = db;
        }

        public async Task<BaseResponseModel> Execute(int pageNumber = 1, int pageSize = 20)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            // Proyección directa + conteo de asignaciones activas
            var query = _db.Rutinas
                .AsNoTracking()
                .OrderByDescending(r => r.FechaCreacion)
                .Select(r => new GetRutinaAdminListItemModel
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    FechaCreacion = r.FechaCreacion,
                    TotalAsignacionesActivas = _db.RutinasAsignadas.Count(ra => ra.RutinaId == r.Id)
                });

            var total = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return ResponseApiService.Response(StatusCodes.Status200OK, new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = total,
                Items = items
            });
        }
    }
}
