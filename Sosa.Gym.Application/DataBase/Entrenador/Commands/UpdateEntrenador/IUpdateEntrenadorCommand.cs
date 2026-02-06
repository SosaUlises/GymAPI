using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Entrenador.Commands.UpdateEntrenador
{
    public interface IUpdateEntrenadorCommand
    {
        Task<BaseResponseModel> Execute(int userId, UpdateEntrenadorModel model);
    }
}
