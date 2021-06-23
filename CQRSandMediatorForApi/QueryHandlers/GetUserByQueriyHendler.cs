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
    public class GetUserByQueriyHendler : IRequestHandler<GetUserByQueriy, UserModelDTO>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public GetUserByQueriyHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            DbContext = dbContext;
            Mapper = mapper;
        }
        public async Task<UserModelDTO> Handle(GetUserByQueriy request, CancellationToken cancellationToken)
        {
            return Mapper.Map<UserModelDTO>(DbContext.Users.FirstOrDefault(el => el.Email == request.Email));
        }
    }
}
