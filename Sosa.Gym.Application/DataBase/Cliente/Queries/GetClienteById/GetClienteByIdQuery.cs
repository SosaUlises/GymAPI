using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Cliente.Queries.GetClienteByDni
{
    public class GetClienteByIdQuery : IGetClienteByIdQuery
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;
        private readonly UserManager<UsuarioEntity> _userManager;

        public GetClienteByIdQuery(
            IDataBaseService dataBaseService,
            IMapper mapper,
            UserManager<UsuarioEntity> userManager
            )
        {
            _mapper = mapper;
            _dataBaseService = dataBaseService;
            _userManager = userManager;
        }

        public async Task<BaseRespondeModel> Execute(int clienteId, int userId)
        {
            var cliente = await _dataBaseService.Clientes
                                        .Include(x => x.Usuario)
                                        .FirstOrDefaultAsync(x => x.Id == clienteId);

            if (cliente == null)
            {
                return ResponseApiService.Response(StatusCodes.Status404NotFound,
                    "Cliente no encontrado");
            }


            var usuarioActual = await _dataBaseService.Usuarios
                                        .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuarioActual == null)
            {
                return ResponseApiService.Response(StatusCodes.Status401Unauthorized,
                    "No se pudo identificar al usuario");
            }


            var roles = await _userManager.GetRolesAsync(usuarioActual);



            // Si NO es admin y NO es el dueño de la info, no lo dejamos ver
            if (!roles.Contains("Administrador") && cliente.UsuarioId != userId)
            {
                return ResponseApiService.Response(
                    StatusCodes.Status403Forbidden,
                    "No puedes ver datos de otro cliente"
                );
            }


            return ResponseApiService.Response(StatusCodes.Status200OK,
                   _mapper.Map<GetClienteByIdModel>(cliente));
        }

    }
}
