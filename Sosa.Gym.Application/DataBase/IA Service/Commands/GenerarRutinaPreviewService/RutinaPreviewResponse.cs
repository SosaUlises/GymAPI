using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.IA_Service.Commands.GenerarRutinaPreviewService
{
    public class RutinaPreviewResponse
    {
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public List<DiaRutinaPreviewResponse> Dias { get; set; } = new();
    }
}
