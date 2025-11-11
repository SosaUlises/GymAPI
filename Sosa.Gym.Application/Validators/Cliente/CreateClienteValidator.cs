using FluentValidation;
using Sosa.Gym.Application.DataBase.Cliente.Commands.CreateCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Validators.Cliente
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteModel>
    {
        public CreateClienteValidator()
        {
            RuleFor(x => x.Nombre).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Apellido).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Dni).NotNull().NotEmpty();
            RuleFor(x => x.Altura).NotNull().NotEmpty();
            RuleFor(x => x.Edad).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Objetivo).NotNull().NotEmpty();

        }
    }
}
