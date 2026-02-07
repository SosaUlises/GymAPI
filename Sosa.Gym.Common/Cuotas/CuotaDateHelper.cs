using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Common.Cuotas
{
    public static class CuotaDateHelper
    {
        public static DateTime CalcularVencimientoUtc(
            int anio,
            int mes,
            int diaVencimiento)
        {
            var ultimoDia = DateTime.DaysInMonth(anio, mes);
            var dia = Math.Min(diaVencimiento, ultimoDia);

            return new DateTime(
                anio,
                mes,
                dia,
                23, 59, 59,
                DateTimeKind.Utc
            );
        }
    }
}
