using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cliente.Commands.UpdateCliente
{
    public class UpdateClienteModel
    {
        // User Identity
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public long Dni { get; set; }
        public string? Rol { get; set; } // solo admin

        // Cliente
        public int Edad { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public string? Objetivo { get; set; }
    }
}
