using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios
{
    public interface IGetAllUsuariosQuery
    {
        Task<IPagedList<GetAllUsuariosModel>> Execute(int pageNumber, int pageSize);
    }
}
