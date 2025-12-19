using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina
{
    public interface IGetEjerciciosByDiaRutinaQuery
    {
        Task<BaseResponseModel> Execute(int diaRutinaId, int userId);
    }
}
