using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Cuota.Commands.PagarCuota
{
    public class PagarCuotaModel
    {
        public int CuotaId { get; set; }
        public string MetodoPago { get; set; }
    }
}
