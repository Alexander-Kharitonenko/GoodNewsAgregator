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
    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQueriy, NewsModelDTO>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Maper;

        public GetNewsByIdQueryHandler(DbContextNewsGenerator dbContext, IMapper maper)
        {
            DbContext = dbContext;
            Maper = maper;
        }

        public async Task<NewsModelDTO> Handle(GetNewsByIdQueriy request, CancellationToken cancellationToken)
        {
            NewsModelDTO news = Maper.Map<NewsModelDTO>( await DbContext.News.FirstOrDefaultAsync(el => el.Id.Equals(request.Id), cancellationToken));

            return news;
        }
    }
}
