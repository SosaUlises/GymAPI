using Sosa.Gym.Domain.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.External
{
    public interface IGetTokenJWTService
    {
        string Execute(string userId, string role, UsuarioEntity usuario);
    }
}
