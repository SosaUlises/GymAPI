using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioById
{
    public class GetUsuarioByIdQuery : IGetUsuarioByIdQuery
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;
        public GetUsuarioByIdQuery(
           UserManager<UsuarioEntity> userManager,
            IMapper mapper
            )
        {
            _userManager = userManager ;
            _mapper = mapper;   
        }

        public async Task<BaseResponseModel> Execute(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.Id == userId);

            if (user == null)
            {
                return ResponseApiService.Response(StatusCodes.Status404NotFound,
                    "Usuario no encontrado");
            }

            return ResponseApiService.Response(StatusCodes.Status200OK,
                _mapper.Map<GetUsuarioByIdModel>(user));
        }
    }
}
