using AutoMapper;
using CQRSandMediatorForApi.Command;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.CommandHandlers
{
    public class AddRoleCommandHendler : IRequestHandler<AddRoleCommand, int>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public AddRoleCommandHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            DbContext = dbContext;
            Mapper = mapper;
        }
        public async Task<int> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            Role role = new Role()
            {
                Id = Guid.NewGuid(),
                NameRole = request.NameRole
            };
            DbContext.Roles.Add(role);
            return await DbContext.SaveChangesAsync();
        }
    }
}
