using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Sosa.Gym.Domain.Entidades.Usuario;
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
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly IMapper _mapper;

        public GetAllUsuariosQuery(
            UserManager<UsuarioEntity> userManager,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task <IPagedList<GetAllUsuariosModel>> Execute(int pageNumber, int pageSize)
        {
            var users = _userManager.Users.AsQueryable();

            var queryDto = users.ProjectTo<GetAllUsuariosModel>(_mapper.ConfigurationProvider);

            var pagedDate = await queryDto.ToPagedListAsync(pageNumber, pageSize);

            return pagedDate;
           
        }


    }
}
