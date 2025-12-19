using FluentValidation;
using Sosa.Gym.Application.DataBase.Cuota.Commands.PagarCuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Cuota
{
    public class PagarCuotaValidator : AbstractValidator<PagarCuotaModel>
    {
        public PagarCuotaValidator()
        {
            RuleFor(x => x.MetodoPago).NotNull().NotEmpty();
        }
    }
}
