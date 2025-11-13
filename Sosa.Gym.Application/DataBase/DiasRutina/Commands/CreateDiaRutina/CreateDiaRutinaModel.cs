using Sosa.Gym.Domain.Entidades.Rutina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.DiasRutina.Commands.CreateDiaRutina
{
    public class CreateDiaRutinaModel
    {
        public int RutinaId { get; set; }
        public string NombreDia { get; set; }
    }
}
