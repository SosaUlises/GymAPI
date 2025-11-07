using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Domain.Models
{
    public class BaseRespondeModel
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public dynamic Data { get; set; }

    }
}
