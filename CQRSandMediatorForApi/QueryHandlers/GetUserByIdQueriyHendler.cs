using AutoMapper;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
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
    public class GetUserByIdQueriyHendler : IRequestHandler<GetUserByIdQueriy, UserModelDTO>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public GetUserByIdQueriyHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            DbContext = dbContext;
            Mapper = mapper;
        }
        public async Task<UserModelDTO> Handle(GetUserByIdQueriy request, CancellationToken cancellationToken)
        {
            Guid id = new Guid($"{request.Id}");
            User user =  DbContext.Users.FirstOrDefault(el => el.Id == id);
            return Mapper.Map<UserModelDTO>(user);
        }
    }
}
