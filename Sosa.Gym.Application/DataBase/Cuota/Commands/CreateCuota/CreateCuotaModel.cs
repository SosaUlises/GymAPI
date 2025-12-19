using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota
{
    public class CreateCuotaModel
    {
        public decimal Monto { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
    }
}
