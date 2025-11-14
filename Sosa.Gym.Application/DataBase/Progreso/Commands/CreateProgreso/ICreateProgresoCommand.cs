using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Progreso.Commands.CreateProgreso
{
    public interface ICreateProgresoCommand
    {
        Task<BaseRespondeModel> Execute(CreateProgresoModel model);
    }
}
