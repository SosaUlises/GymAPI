using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Progreso.Commands.UpdateProgreso
{
    public class UpdateProgresoModel
    {
        public decimal PesoActual { get; set; }
        public decimal Pecho { get; set; }
        public decimal Brazos { get; set; }
        public decimal Cintura { get; set; }
        public decimal Piernas { get; set; }
        public string? Observaciones { get; set; }
    }
}
