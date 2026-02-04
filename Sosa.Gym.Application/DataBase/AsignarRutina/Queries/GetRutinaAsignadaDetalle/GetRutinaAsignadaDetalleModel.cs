using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.AsignarRutina.Queries.GetRutinaAsignadaDetalle
{
    public class RutinaAsignadaDetalleModel
    {
        public int RutinaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public List<DiaRutinaDetalleModel> Dias { get; set; } = new();
    }

    public class DiaRutinaDetalleModel
    {
        public int DiaRutinaId { get; set; }
        public string NombreDia { get; set; } = null!;
        public List<EjercicioDetalleModel> Ejercicios { get; set; } = new();
    }

    public class EjercicioDetalleModel
    {
        public int EjercicioId { get; set; }
        public string Nombre { get; set; } = null!;
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public decimal PesoUtilizado { get; set; }
    }
}
