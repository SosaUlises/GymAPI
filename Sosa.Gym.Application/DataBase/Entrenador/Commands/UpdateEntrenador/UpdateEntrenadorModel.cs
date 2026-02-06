using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Entrenador.Commands.UpdateEntrenador
{
    public class UpdateEntrenadorModel
    {
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public long Dni { get; set; }
    }
}
