using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.IA_Service.Commands.GenerarRutinaPreviewService
{
    public class DiaRutinaPreviewResponse
    {
        public string NombreDia { get; set; } = null!;
        public List<EjercicioPreviewResponse> Ejercicios { get; set; } = new();
    }
}
