using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.AsignarRutina.Commands.AsignarRutina
{
    public interface IAsignarRutinaCommand
    {
        Task<BaseResponseModel> Execute(int rutinaId, int clienteId);
    }
}
