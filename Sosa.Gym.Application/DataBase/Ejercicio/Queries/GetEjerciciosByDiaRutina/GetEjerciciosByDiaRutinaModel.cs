using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Ejercicio.Queries.GetEjerciciosByDiaRutina
{
    public class GetEjerciciosByDiaRutinaModel
    {
        public int DiaRutinaId { get; set; }
        public string Nombre { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public decimal PesoUtilizado { get; set; }
    }
}
