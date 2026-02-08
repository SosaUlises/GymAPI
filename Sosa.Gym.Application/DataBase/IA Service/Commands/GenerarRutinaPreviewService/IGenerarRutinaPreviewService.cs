using Sosa.Gym.Application.DataBase.IA_Service.GenerarRutinaPreviewService;
using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.IA_Service.Commands.GenerarRutinaPreviewService
{
    public interface IGenerarRutinaPreviewService
    {
        Task<BaseResponseModel> Execute(GenerarRutinaPreviewRequest request);
    }
}
