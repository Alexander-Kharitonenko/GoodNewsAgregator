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
   public class GetRssSourceByIdQueriyHendler : IRequestHandler<GetRssSourceByIdQueriy, SourceModelDTO>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Maper;

        public GetRssSourceByIdQueriyHendler(DbContextNewsGenerator dbContext, IMapper maper)
        {
            DbContext = dbContext;
            Maper = maper;
        }

        public async Task<SourceModelDTO> Handle(GetRssSourceByIdQueriy request, CancellationToken cancellationToken)
        {
            return Maper.Map<SourceModelDTO>(await DbContext.Sources.FirstOrDefaultAsync(el => el.Id == request.id));
        }
    }
}
