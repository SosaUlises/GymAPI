using FluentValidation;
using Sosa.Gym.Application.DataBase.Usuario.Commands.CreateUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Usuario
{
    public class CreateUsuarioValidator : AbstractValidator<CreateUsuarioModel>
    {
        public CreateUsuarioValidator() 
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Apellido).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(30);
        }
    }
}
