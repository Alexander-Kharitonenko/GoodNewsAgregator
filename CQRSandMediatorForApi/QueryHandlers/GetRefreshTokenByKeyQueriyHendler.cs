using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
using DTO_Models;
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
    public class GetRefreshTokenByKeyQueriyHendler : IRequestHandler<GetRefreshTokenByKeyQueriy, RefreshTokenModelDTO>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        private readonly IMediator Mediator;
        public GetRefreshTokenByKeyQueriyHendler(DbContextNewsGenerator dbContext, IMapper mapper, IMediator mediator) 
        {
            DbContext = dbContext;
            Mapper = mapper;
            Mediator = mediator; 
    }

        public async Task<RefreshTokenModelDTO> Handle(GetRefreshTokenByKeyQueriy request, CancellationToken cancellationToken)
        {
            RefreshTokenModelDTO refreshToken = Mapper.Map<RefreshTokenModelDTO>(await DbContext.RefreshTokens.FirstOrDefaultAsync(el => el.Key.Equals(request.Key)));
            if (refreshToken != null)
            {
                return refreshToken;
            }

            else 
            {
                return null;
            }
         

            
        }
    }
}
