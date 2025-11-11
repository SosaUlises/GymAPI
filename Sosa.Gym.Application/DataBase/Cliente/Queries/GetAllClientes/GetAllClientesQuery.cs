using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace Sosa.Gym.Application.DataBase.Cliente.Queries.GetAllClientes
{
    public class GetAllClientesQuery : IGetAllClientesQuery
    {

        private readonly IMapper _mapper;
        private readonly IDataBaseService _dataBaseService;
        public GetAllClientesQuery(
            IDataBaseService dataBaseService,
            IMapper mapper)
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task<IPagedList<GetAllClientesModel>> Execute(int pageNumber, int pageSize)
        {
            var users = _dataBaseService.Clientes.Include(c => c.Usuario).AsQueryable();

            var queryDto = users.ProjectTo<GetAllClientesModel>(_mapper.ConfigurationProvider);

            var pagedDate = await queryDto.ToPagedListAsync(pageNumber, pageSize);

            return pagedDate;

        }
    }
}
