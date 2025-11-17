using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Login
{
    public interface ILoginCommand
    {
        Task<BaseRespondeModel> Execute(LoginModel model);
    }
}
