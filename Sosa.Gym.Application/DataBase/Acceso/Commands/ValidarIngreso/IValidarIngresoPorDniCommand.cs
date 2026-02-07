using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.Acceso.Commands.ValidarIngreso
{
    public interface IValidarIngresoPorDniCommand
    {
        Task<BaseResponseModel> Execute(long dni);
    }
}
