using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Progreso.Queries.GetProgresoByCliente
{
    public class GetProgresoModel
    {
        public int Id { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal PesoActual { get; set; }
        public decimal Pecho { get; set; }
        public decimal Brazos { get; set; }
        public decimal Cintura { get; set; }
        public decimal Piernas { get; set; }
        public string? Observaciones { get; set; }
    }

}
