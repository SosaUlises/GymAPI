using FluentValidation;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using Sosa.Gym.Application.DataBase.Rutina.CreateRutina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Rutina
{
    public class CreateRutinaValidator : AbstractValidator<CreateRutinaModel>
    {
        public CreateRutinaValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Descripcion).NotNull().NotEmpty();
            RuleFor(x => x.FechaCreacion).NotNull();
            RuleFor(x => x.ClienteId).NotNull();
        }
    }
}
