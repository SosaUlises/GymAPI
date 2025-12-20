using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Rutina.Commands.UpdateRutina
{
    public interface IUpdateRutinaCommand
    {
        Task<BaseResponseModel> Execute(int rutinaId, UpdateRutinaModel model, int userId);
    }
}
