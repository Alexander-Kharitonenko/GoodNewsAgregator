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
    public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQueriy, IEnumerable<NewsModelDTO>>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Maper;

        public GetAllNewsQueryHandler(DbContextNewsGenerator dbContext, IMapper maper)
        {
            DbContext = dbContext;
            Maper = maper;
        }

        public async Task<IEnumerable<NewsModelDTO>> Handle(GetAllNewsQueriy request, CancellationToken cancellationToken)
        {
            return await DbContext.News.Where(el => el.Id != null).Select(el => Maper.Map<NewsModelDTO>(el)).ToListAsync(cancellationToken);
        }
    }
}
