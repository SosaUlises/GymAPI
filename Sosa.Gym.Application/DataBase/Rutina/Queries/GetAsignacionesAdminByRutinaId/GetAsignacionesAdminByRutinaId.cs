using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetAsignacionesAdminByRutinaId
{
    public class GetAsignacionAdminItemModel
    {
        public int RutinaAsignadaId { get; set; }
        public int ClienteId { get; set; }
        public long Dni { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public DateTime FechaAsignacion { get; set; }
        public bool Activa { get; set; }
    }
}
