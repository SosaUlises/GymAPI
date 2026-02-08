using Sosa.Gym.Domain.Models;

namespace Sosa.Gym.Application.DataBase.IA_Service.Commands.GenerarRutinaPreviewService
{
    public interface IGenerarRutinaPreviewService
    {
        Task<BaseResponseModel> Execute(GenerarRutinaPreviewRequest request);
    }
}
