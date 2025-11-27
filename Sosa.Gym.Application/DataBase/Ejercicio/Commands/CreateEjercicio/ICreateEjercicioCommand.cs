using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio
{
    public interface ICreateEjercicioCommand
    {
        Task<BaseRespondeModel> Execute(CreateEjercicioModel model, int userId);
    }
}
