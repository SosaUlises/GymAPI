using FluentValidation;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuota;
using Sosa.Gym.Domain.Entidades.Cuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Cuota
{
    public class CreateCuotaValidator : AbstractValidator<CreateCuotaModel>
    {
        public CreateCuotaValidator()
        {

            RuleFor(x => x.Mes).NotNull().NotEmpty();
            RuleFor(x => x.Anio).NotNull().NotEmpty();
            RuleFor(x => x.Monto).NotNull().NotEmpty();
        }
    }
}
