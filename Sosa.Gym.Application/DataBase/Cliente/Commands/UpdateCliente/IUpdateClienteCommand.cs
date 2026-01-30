using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente
{
    public interface IUpdateClienteCommand
    {
        Task<BaseResponseModel> Execute(int clienteId, UpdateClienteModel model, int userIdLogueado, bool esAdmin);
        Task<BaseResponseModel> ExecuteMe(UpdateClienteModel model, int userIdLogueado);
    }
}
