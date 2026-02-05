using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaDetalleAdmin
{
    public class GetEjercicioDetalleModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public decimal PesoUtilizado { get; set; }
    }
}
