using FluentValidation;
using Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Rutina
{
    public class UpdateRutinaValidator : AbstractValidator<UpdateRutinaModel>
    {
        public UpdateRutinaValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.Descripcion).NotNull().NotEmpty();
        }
    }
}
