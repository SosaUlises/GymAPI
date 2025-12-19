using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sosa.Gym.Application.External;
using Sosa.Gym.Application.Features;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Login
{
    public class LoginCommand : ILoginCommand
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly SignInManager<UsuarioEntity> _signInManager;
        private readonly IGetTokenJWTService _jwtService;

        public LoginCommand(
            UserManager<UsuarioEntity> userManager,
            SignInManager<UsuarioEntity> signInManager,
            IGetTokenJWTService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<BaseResponseModel> Execute(LoginModel model)
        {
            var usuario = await _userManager.FindByEmailAsync(model.Email);

            if (usuario == null)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Usuario o contraseña incorrectos");

            var result = await _signInManager.CheckPasswordSignInAsync(usuario, model.Password, false);

            if (!result.Succeeded)
                return ResponseApiService.Response(StatusCodes.Status400BadRequest, "Usuario o contraseña incorrectos");

            var roles = await _userManager.GetRolesAsync(usuario);
            var rol = roles.FirstOrDefault();

            var token = _jwtService.Execute(usuario.Id.ToString(), rol, usuario);

            return ResponseApiService.Response(StatusCodes.Status200OK, new
            {
                Token = token,
                Usuario = new
                {
                    usuario.Id,
                    usuario.Email,
                    usuario.Nombre,
                    usuario.Apellido,
                    Rol = rol
                }
            }, "Login exitoso");
        }
    }

    }
