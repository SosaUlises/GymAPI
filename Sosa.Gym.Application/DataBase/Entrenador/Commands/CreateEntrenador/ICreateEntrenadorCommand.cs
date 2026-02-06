using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Entrenador.Commands.CreateEntrenador
{
    public interface ICreateEntrenadorCommand
    {
        Task<BaseResponseModel> Execute(CreateEntrenadorModel model);
    }
}
