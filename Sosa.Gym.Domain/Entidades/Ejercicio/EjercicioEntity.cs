using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Ejercicio
{
    public class EjercicioEntity
    {
        public int Id { get; set; }
        public int DiaRutinaId { get; set; }
        public string Nombre { get; set; }
        public int Series {  get; set; }
        public int Repeticiones { get; set; }
        public decimal PesoUtilizado { get; set; }
    }
}
