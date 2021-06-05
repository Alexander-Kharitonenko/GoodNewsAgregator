using AutoMapper;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator.Models.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.QueryHandlers
{
    public class GetRoleByIdQueriyHendler : IRequestHandler<GetRoleByIdQueriy, RoleModelDTO>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public GetRoleByIdQueriyHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            DbContext = dbContext;
            Mapper = mapper;
        }
        public async Task<RoleModelDTO> Handle(GetRoleByIdQueriy request, CancellationToken cancellationToken)
        {
          return Mapper.Map<RoleModelDTO>(DbContext.Roles.FirstOrDefault(el => el.Id == request.id));
        }
    }
}
