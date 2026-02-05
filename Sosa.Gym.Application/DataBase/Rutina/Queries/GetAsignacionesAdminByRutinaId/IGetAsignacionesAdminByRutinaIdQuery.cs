using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetAsignacionesAdminByRutinaId
{
    public interface IGetAsignacionesAdminByRutinaIdQuery
    {
        Task<BaseResponseModel> Execute(int rutinaId);
    }
}
