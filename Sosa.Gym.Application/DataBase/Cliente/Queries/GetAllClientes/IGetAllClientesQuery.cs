using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes
{
    public interface IGetAllClientesQuery
    {
        Task<IPagedList<GetAllClientesModel>> Execute(int pageNumber, int pageSize);
    }
}
