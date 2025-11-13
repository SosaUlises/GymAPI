using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.DiasRutina.Queries.GetDiasRutinaByRutinaId
{
    public interface IGetDiasRutinaByRutinaIdQuery
    {
        Task<BaseRespondeModel> Execute(int rutinaId);
    }
}
