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
    public class AddNewsCommandHendler : IRequestHandler<AddNewsCommand, int>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public AddNewsCommandHendler(DbContextNewsGenerator dbContext, IMapper mapprer) 
        {
            DbContext = dbContext;
            Mapper = mapprer;
        }

        public async Task<int> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<News> allNews = request.AllNews.Select(el => Mapper.Map<News>(el));
            await DbContext.News.AddRangeAsync(allNews);
            return await DbContext.SaveChangesAsync();
        }
    }
}
