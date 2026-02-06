using FluentValidation;
using Sosa.Gym.Application.DataBase.Entrenador.Commands.CreateEntrenador;

namespace Sosa.Gym.Application.Validators.Entrenador
{
    public class CreateEntrenadorValidator : AbstractValidator<CreateEntrenadorModel>
    {
        public CreateEntrenadorValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Nombre).NotEmpty();
            RuleFor(x => x.Apellido).NotEmpty();
            RuleFor(x => x.Dni).GreaterThan(0);
        }
    }
}
