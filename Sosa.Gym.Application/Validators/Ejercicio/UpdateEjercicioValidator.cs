using FluentValidation;
using Sosa.Gym.Application.DataBase.Ejercicio.Commands.UpdateEjercicio;

namespace Sosa.Gym.Application.Validators.Ejercicio
{
    public class UpdateEjercicioValidator : AbstractValidator<UpdateEjercicioModel>
    {
        public UpdateEjercicioValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty();
            RuleFor(x => x.Series).NotNull().NotEmpty();
            RuleFor(x => x.Repeticiones).NotNull().NotEmpty();
            RuleFor(x => x.PesoUtilizado).NotNull().NotEmpty();
        }
    }
}
