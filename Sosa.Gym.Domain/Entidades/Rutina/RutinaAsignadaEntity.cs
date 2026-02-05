using Sosa.Gym.Domain.Entidades.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Rutina
{
    public class RutinaAsignadaEntity
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public ClienteEntity Cliente { get; set; } = null!;

        public int RutinaId { get; set; }
        public RutinaEntity Rutina { get; set; } = null!;

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

    }
}
