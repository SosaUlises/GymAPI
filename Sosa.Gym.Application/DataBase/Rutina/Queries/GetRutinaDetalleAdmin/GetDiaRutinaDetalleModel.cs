using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaDetalleAdmin
{
    public class GetDiaRutinaDetalleModel
    {
        public int Id { get; set; }
        public string NombreDia { get; set; } = null!;
        public List<GetEjercicioDetalleModel> Ejercicios { get; set; } = new();
    }
}
