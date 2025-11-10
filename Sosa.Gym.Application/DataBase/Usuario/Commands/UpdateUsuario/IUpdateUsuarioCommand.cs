using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario
{
    public interface IUpdateUsuarioCommand
    {
        Task<BaseRespondeModel> Execute(UpdateUsuarioModel model);
    }
}
