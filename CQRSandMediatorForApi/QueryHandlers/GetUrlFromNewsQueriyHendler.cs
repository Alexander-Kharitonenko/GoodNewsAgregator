using AutoMapper;
using CQRSandMediatorForApi.Queries;
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
    public class GetUrlFromNewsQueriyHendler : IRequestHandler<GetUrlFromNewsQueriy, IEnumerable<string>>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        public GetUrlFromNewsQueriyHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            DbContext = dbContext;
            Mapper = mapper;
        }
        public async Task<IEnumerable<string>> Handle(GetUrlFromNewsQueriy request, CancellationToken cancellationToken)
        {
            var allUrlNews =  await DbContext.News.Where(el => el.NewsURL != null).Select(el => el.NewsURL).ToListAsync();
            return allUrlNews;
        }
    }
}
