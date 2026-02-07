using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Cuota;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Acceso.Commands.ValidarIngreso
{
    public class ValidarIngresoPorDniCommand : IValidarIngresoPorDniCommand
    {
        private readonly IDataBaseService _db;
        private readonly UserManager<UsuarioEntity> _userManager;

        public ValidarIngresoPorDniCommand(IDataBaseService db, UserManager<UsuarioEntity> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<BaseResponseModel> Execute(long dni)
        {
            if (dni <= 0)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "DNI inválido");

            // Usuario + cliente
            var usuario = await _userManager.Users
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.Dni == dni);

            if (usuario == null || usuario.Cliente == null)
            {
                return ResponseApiService.Response(StatusCodes.Status404NotFound,
                    new ValidarIngresoResponseModel
                    {
                        AccesoPermitido = false,
                        Mensaje = "Cliente no encontrado"
                    });
            }

            var clienteId = usuario.Cliente.Id;

            // Cuota del mes actual 
            var now = DateTime.UtcNow;
            var anio = now.Year;
            var mes = now.Month;

            var cuota = await _db.Cuotas
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId && c.Anio == anio && c.Mes == mes);

            if (cuota == null)
            {
                return ResponseApiService.Response(StatusCodes.Status200OK,
                    new ValidarIngresoResponseModel
                    {
                        AccesoPermitido = false,
                        Mensaje = "Sin cuota del mes actual",
                        ClienteId = clienteId,
                        UsuarioId = usuario.Id,
                        NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
                        Anio = anio,
                        Mes = mes
                    });
            }

            // Si está pendiente y ya venció -> la marco vencida
            if (cuota.Estado == EstadoCuota.Pendiente &&
                cuota.FechaVencimiento != default &&
                now > cuota.FechaVencimiento)
            {
                cuota.Estado = EstadoCuota.Vencida;
                await _db.SaveAsync();
            }


            if (cuota.Estado == EstadoCuota.Pagada)
            {
                return ResponseApiService.Response(StatusCodes.Status200OK,
                    new ValidarIngresoResponseModel
                    {
                        AccesoPermitido = true,
                        Mensaje = "Acceso OK",
                        ClienteId = clienteId,
                        UsuarioId = usuario.Id,
                        NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
                        Anio = cuota.Anio,
                        Mes = cuota.Mes,
                        Estado = cuota.Estado,
                        FechaVencimientoUtc = cuota.FechaVencimiento,
                        FechaPagoUtc = cuota.FechaPago
                    });
            }

            // Pendiente o Vencida
            var msg = cuota.Estado == EstadoCuota.Vencida
                ? "Cuota vencida. Acceso denegado"
                : "Cuota pendiente. Acceso denegado";

            return ResponseApiService.Response(StatusCodes.Status200OK,
                new ValidarIngresoResponseModel
                {
                    AccesoPermitido = false,
                    Mensaje = msg,
                    ClienteId = clienteId,
                    UsuarioId = usuario.Id,
                    NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
                    Anio = cuota.Anio,
                    Mes = cuota.Mes,
                    Estado = cuota.Estado,
                    FechaVencimientoUtc = cuota.FechaVencimiento,
                    FechaPagoUtc = cuota.FechaPago
                });
        }
    }
}
