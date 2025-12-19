using FluentValidation;
using Sosa.Gym.Application.DataBase.Cuota.Commands.CreateCuotaAll;

namespace Sosa.Gym.Application.Validators.Cuota
{
    public class GenerarCuotaValidator : AbstractValidator<GenerarCuotasModel>
    {
        public GenerarCuotaValidator()
        {
            RuleFor(x => x.Mes).NotNull().NotEmpty();
            RuleFor(x => x.Anio).NotNull().NotEmpty();
            RuleFor(x => x.Monto).NotNull().NotEmpty();
        }

    }
}
