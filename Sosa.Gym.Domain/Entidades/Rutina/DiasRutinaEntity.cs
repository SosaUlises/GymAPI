using Sosa.Gym.Domain.Entidades.Ejercicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Rutina
{
    public class DiasRutinaEntity
    {
        public int Id { get; set; }
        public int RutinaId { get; set; }
        public string NombreDia {  get; set; }
        public RutinaEntity Rutina { get; set; }
        public ICollection<EjercicioEntity> Ejercicios { get; set; } = new List<EjercicioEntity>();
    }
}
