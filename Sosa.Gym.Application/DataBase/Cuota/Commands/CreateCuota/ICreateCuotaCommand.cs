using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota
{
    public interface ICreateCuotaCommand
    {
        Task<BaseResponseModel> Execute(int clienteId, CreateCuotaModel model);
    }
}
