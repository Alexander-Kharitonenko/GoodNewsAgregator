using AutoMapper;
using CQRSandMediatorForApi.Command;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.CommandHandlers
{
    public class DeleteRoleCommandHendler : IRequestHandler<DeleteRoleCommand , int>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        
        public DeleteRoleCommandHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            DbContext = dbContext;
            Mapper = mapper;
            
        }

        public async Task<int> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
           
           DbContext.Roles.Remove(await DbContext.Roles.FirstOrDefaultAsync(el => el.Id == request.id));
           return await DbContext.SaveChangesAsync();
        }
    }
}
