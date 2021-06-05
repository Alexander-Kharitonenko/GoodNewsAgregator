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
    public class GetAllRssSourceQueriyHendler : IRequestHandler<GetAllRssSourceQueriy, IEnumerable<SourceModelDTO>>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Maper;

        public GetAllRssSourceQueriyHendler(DbContextNewsGenerator dbContext, IMapper maper)
        {
            DbContext = dbContext;
            Maper = maper;
        }
        public async Task<IEnumerable<SourceModelDTO>> Handle(GetAllRssSourceQueriy request, CancellationToken cancellationToken)
        {
            return  await DbContext.Sources.Where(el => el.SourseURL != null).Select(el => Maper.Map<SourceModelDTO>(el)).ToListAsync(cancellationToken);
        }
    } 
}
