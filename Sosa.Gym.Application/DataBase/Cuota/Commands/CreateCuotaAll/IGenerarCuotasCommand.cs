using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuotaAll
{
    public interface IGenerarCuotasCommand
    {
        Task<BaseResponseModel> Execute(GenerarCuotasModel model);
    }
}
