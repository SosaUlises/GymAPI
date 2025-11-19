using Sosa.Gym.Domain.Entidades.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Cuota
{
    public class CuotaEntity
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public ClienteEntity Cliente { get; set; }

        public decimal Monto { get; set; }

        public int Anio { get; set; }
        public int Mes { get; set; }

        public string Estado { get; set; } = "Pendiente"; 

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaPago { get; set; }

        public string MetodoPago { get; set; } 
    }
}
