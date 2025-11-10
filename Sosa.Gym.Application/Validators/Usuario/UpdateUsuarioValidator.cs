using FluentValidation;
using Sosa.Gym.Application.DataBase.Usuario.Commands.UpdateUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Usuario
{
    public class UpdateUsuarioValidator : AbstractValidator<UpdateUsuarioModel>
    {
        public UpdateUsuarioValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Apellido).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(30);
        }
    }
}
