using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Rutina
{
    public class DiasRutina
    {
        public int Id { get; set; }
        public int RutinaId { get; set; }
        public string NombreDia {  get; set; }
    }
}
