using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.EF;

namespace Sosa.Gym.Application.DataBase.Usuario.Queries.GetAllUsuarios
{
    public class GetAllUsuariosQuery : IGetAllUsuariosQuery
    {
        private readonly IDataBaseService _dataBaseService;
        private readonly IMapper _mapper;

        public GetAllUsuariosQuery(
            IDataBaseService dataBaseService,
            IMapper mapper
            )
        {
            _dataBaseService = dataBaseService;
            _mapper = mapper;
        }

        public async Task <IPagedList<GetAllUsuariosModel>> Execute(int pageNumber, int pageSize)
        {
            var users = _dataBaseService.Usuarios.AsQueryable();

            var queryDto = users.ProjectTo<GetAllUsuariosModel>(_mapper.ConfigurationProvider);

            var pagedDate = await queryDto.ToPagedListAsync(pageNumber, pageSize);

            return pagedDate;
           
        }


    }
}
