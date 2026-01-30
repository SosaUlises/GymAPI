using Sosa.Gym.Domain.Entidades.Cuota;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Queries.GetCuotasPendientes
{
    public interface IGetCuotasByEstadoQuery
    {
       Task<BaseResponseModel> Execute(EstadoCuota estado,int userId,bool esAdmin);
    }
}
