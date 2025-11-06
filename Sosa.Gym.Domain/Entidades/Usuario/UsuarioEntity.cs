using Microsoft.AspNetCore.Identity;
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
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int Edad {  get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public string? Objetivo { get; set; }
        public DateTime FechaRegistro { get; set; }

        public ICollection<ProgresoEntity> Progresos { get; set; } = new List<ProgresoEntity>();
        public ICollection<RutinaEntity> Rutinas { get; set; } = new List<RutinaEntity>();
    }
}
