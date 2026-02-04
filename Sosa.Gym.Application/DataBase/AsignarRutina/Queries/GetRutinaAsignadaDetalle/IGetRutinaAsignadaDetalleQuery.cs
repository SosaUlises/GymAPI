using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignadaDetalle
{
    public interface IGetRutinaAsignadaDetalleQuery
    {
        Task<BaseResponseModel> Execute(int rutinaId, int userId);
    }
}
