using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaAdmin
{
    public interface IGetRutinasAdminQuery
    {
        Task<BaseResponseModel> Execute(int pageNumber = 1, int pageSize = 20);
    }
}
