using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio
{
    public class CreateEjercicioModel
    {
        public string Nombre { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public decimal PesoUtilizado { get; set; }
    }
}
