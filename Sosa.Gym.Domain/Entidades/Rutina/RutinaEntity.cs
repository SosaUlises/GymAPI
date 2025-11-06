using Sosa.Gym.Domain.Entidades.Cliente;
using Sosa.Gym.Domain.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Entidades.Rutina
{
    public class RutinaEntity
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }
        public DateTime FechaCreacion { get; set; }
        public ClienteEntity Cliente { get; set; }
        public ICollection<DiasRutinaEntity> DiasRutina { get; set; } = new List<DiasRutinaEntity>();
    }
}
