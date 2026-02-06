using FluentValidation;
using Sosa.Gym.Application.DataBase.Entrenador.Commands.UpdateEntrenador;

namespace Sosa.Gym.Application.Validators.Entrenador
{
    public class UpdateEntrenadorValidator : AbstractValidator<UpdateEntrenadorModel>
    {
        public UpdateEntrenadorValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Nombre).NotEmpty();
            RuleFor(x => x.Apellido).NotEmpty();
            RuleFor(x => x.Dni).GreaterThan(0);
            When(x => !string.IsNullOrWhiteSpace(x.Password),
                () => RuleFor(x => x.Password!).MinimumLength(6));
        }
    }
}
