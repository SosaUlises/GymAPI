using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById
{
    public interface IGetUsuarioByIdQuery
    {
        Task<BaseResponseModel> Execute(int userId);
    }
}
