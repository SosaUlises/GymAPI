using FluentValidation;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.CreateEjercicio;

namespace Sosa.Gym.Application.Validators.Ejercicio
{
    public class CreateEjercicioValidator : AbstractValidator<CreateEjercicioModel>
    {
        public CreateEjercicioValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().MaximumLength(40);
            RuleFor(x => x.Series).NotNull().NotEmpty();
            RuleFor(x => x.Repeticiones).NotNull().NotEmpty();
            RuleFor(x => x.PesoUtilizado).NotNull().NotEmpty();

        }
    }
}
