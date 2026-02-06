using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinasAsignadasAdminByCliente
{
    public class GetRutinaAsignadaAdminItemModel
    {
        public int RutinaAsignadaId { get; set; }
        public int RutinaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime FechaAsignacion { get; set; }
    }
}
