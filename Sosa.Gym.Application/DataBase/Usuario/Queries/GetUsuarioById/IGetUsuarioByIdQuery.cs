using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById
{
    public interface IGetUsuarioByIdQuery
    {
        Task<GetUsuarioByIdModel> Execute(int userId);
    }
}
