using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries
{
    public interface IGetRutinaByClienteIdQuery
    {
        Task<BaseRespondeModel> Execute(int clienteId);
    }
}
