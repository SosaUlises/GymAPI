using FluentValidation;
using Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina;

namespace Sosa.Gym.Application.Validators.DiaRutina
{
    public class CreateDiaRutinaValidator : AbstractValidator<CreateDiaRutinaModel>
    {
        public CreateDiaRutinaValidator()
        {
            RuleFor(x => x.NombreDia).NotNull().NotEmpty().MaximumLength(30);
        }
    }
}
