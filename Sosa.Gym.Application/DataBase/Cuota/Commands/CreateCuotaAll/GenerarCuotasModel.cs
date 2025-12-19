using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuotaAll
{
    public class GenerarCuotasModel
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Monto { get; set; }
    }
}
