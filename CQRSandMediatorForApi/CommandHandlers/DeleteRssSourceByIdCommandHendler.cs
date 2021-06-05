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
    public class DeleteRssSourceByIdCommandHendler : IRequestHandler<DeleteRssSourceByIdCommand, int>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public DeleteRssSourceByIdCommandHendler(DbContextNewsGenerator dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }
        public async Task<int> Handle(DeleteRssSourceByIdCommand request, CancellationToken cancellationToken)
        {
            DbContext.Sources.Remove(Mapper.Map<Source>(DbContext.Sources.FirstOrDefault(el => el.Id == request.id)));
            return await DbContext.SaveChangesAsync();
        }
    }
}
