using AutoMapper;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator.Models.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.QueryHandlers
{
    public class GetAllUserWithRoleHendler : IRequestHandler<GetAllUserWithRole, IEnumerable<UserModelDTO>>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public GetAllUserWithRoleHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            DbContext = dbContext;
            Mapper = mapper;
        }
        public async Task<IEnumerable<UserModelDTO>> Handle(GetAllUserWithRole request, CancellationToken cancellationToken)
        {
           return await DbContext.Users.Where(el => el.Id != null).Select(el => Mapper.Map<UserModelDTO>(el)).ToListAsync(cancellationToken);
        }
    }
}
