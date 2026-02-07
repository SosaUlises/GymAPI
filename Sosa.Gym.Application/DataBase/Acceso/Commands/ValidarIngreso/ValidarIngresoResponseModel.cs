using Sosa.Gym.Domain.Entidades.Cuota;

namespace Sosa.Gym.Application.DataBase.Acceso.Commands.ValidarIngreso
{
    public class ValidarIngresoResponseModel
    {
        public bool AccesoPermitido { get; set; }
        public string Mensaje { get; set; } = null!;

        public int? ClienteId { get; set; }
        public int? UsuarioId { get; set; }
        public string? NombreCompleto { get; set; }

        public int? Anio { get; set; }
        public int? Mes { get; set; }
        public EstadoCuota? Estado { get; set; }
        public DateTime? FechaVencimientoUtc { get; set; }
        public DateTime? FechaPagoUtc { get; set; }
    }
}
