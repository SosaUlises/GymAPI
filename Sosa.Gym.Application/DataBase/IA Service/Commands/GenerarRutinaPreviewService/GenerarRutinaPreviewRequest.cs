using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.IA_Service.GenerarRutinaPreviewService
{
    public class GenerarRutinaPreviewRequest
    {
        public string Objetivo { get; set; } = null!;          // ej: "hipertrofia"
        public int DiasPorSemana { get; set; }                 // 3..6
        public string Nivel { get; set; } = null!;             // "principiante|intermedio|avanzado"
        public int DuracionMinutos { get; set; }               // 45..90
        public string? Equipamiento { get; set; }              // "gimnasio completo", "mancuernas", etc.
        public string? Restricciones { get; set; }             // lesiones, movimientos prohibidos
    }
}
