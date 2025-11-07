using Sosa.Gym.Domain.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario
{
    public interface ICreateUsuarioCommand
    {
        Task<UsuarioEntity> Execute(CreateUsuarioModel model);
    }
}
