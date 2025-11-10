using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Domain.Entidades.Cliente;
using Sosa.Gym.Domain.Entidades.Progreso;
using Sosa.Gym.Domain.Entidades.Rutina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Usuario
{
    public class UsuarioEntity : IdentityUser<int>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public long Dni { get; set; }

        public ClienteEntity? Cliente { get; set; }

    }
}
