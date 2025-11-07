using Sosa.Gym.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.Features
{
    public class ResponseApiService
    {
        public static BaseRespondeModel Response(int statusCode, object Data = null, string message = null)
        {
            bool success = false;

            if (statusCode >= 200 && statusCode < 300)
            {
                success = true;
            }

            var result = new BaseRespondeModel()
            {
                Success = success,
                Data = Data,
                Message = message,
                StatusCode = statusCode
            };

            return result;
        }
    }
}
