using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetUsuarioByNombreAndApellido
{
    public interface IGetUsuarioByNombreAndApellidoQuery
    {
        Task<GetUsuarioByNombreAndApellidoModel> Execute(string nombre, string apellido);
    }
}
