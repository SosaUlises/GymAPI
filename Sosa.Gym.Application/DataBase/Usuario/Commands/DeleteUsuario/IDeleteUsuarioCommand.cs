using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Commands.DeleteUsuario
{
    public interface IDeleteUsuarioCommand
    {
        Task<bool> Execute(int userId);
    }
}
