using FluentValidation;
using Sosa.Gym.Application.DataBase.Progreso.Commands.UpdateProgreso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Progreso
{
    public class UpdateProgresoValidator : AbstractValidator<UpdateProgresoModel>
    {
        public UpdateProgresoValidator()
        {
            RuleFor(x => x.PesoActual).NotNull().NotEmpty();
            RuleFor(x => x.Cintura).NotNull().NotEmpty();
            RuleFor(x => x.Brazos).NotNull().NotEmpty();
            RuleFor(x => x.Pecho).NotNull().NotEmpty();
            RuleFor(x => x.Piernas).NotNull().NotEmpty();
        }
    }
}
