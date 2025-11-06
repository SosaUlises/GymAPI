using Sosa.Gym.Domain.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Progreso
{
    public class ProgresoEntity
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal PesoActual { get; set; }
        public decimal Pecho { get; set; }
        public decimal Brazos { get; set; }
        public decimal Cintura { get; set; }
        public decimal Piernas { get; set; }
        public string Observaciones { get; set; }
        public UsuarioEntity Usuario { get; set; }
    }
}
