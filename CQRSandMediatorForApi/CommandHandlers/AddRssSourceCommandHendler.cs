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
    public class AddRssSourceCommandHendler : IRequestHandler<AddRssSourceCommand, int>
    {
        private readonly DbContextNewsGenerator DbContext;
        
        public AddRssSourceCommandHendler(DbContextNewsGenerator dbContext) 
        {
            DbContext = dbContext;
        }
        public async Task<int> Handle(AddRssSourceCommand request, CancellationToken cancellationToken)
        {
            Source source = new Source()
            {
                Id = Guid.NewGuid(),
                SourseURL = request.SourseURL
            };

            DbContext.Sources.Add(source);
            return await DbContext.SaveChangesAsync();

        }
    }
}
