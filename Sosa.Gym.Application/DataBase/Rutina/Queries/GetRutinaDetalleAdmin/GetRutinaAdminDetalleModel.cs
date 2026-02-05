using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaDetalleAdmin
{
    public class GetRutinaAdminDetalleModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public List<GetDiaRutinaDetalleModel> Dias { get; set; } = new();
    }
}
