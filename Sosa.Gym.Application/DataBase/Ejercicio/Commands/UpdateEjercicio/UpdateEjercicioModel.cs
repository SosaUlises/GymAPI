using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio
{
    public class UpdateEjercicioModel
    {
        public string Nombre { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public decimal PesoUtilizado { get; set; }
    }
}
