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
    public class AddRefreshTokenCommandHendler : IRequestHandler<AddRefreshTokenCommand, int>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;

        public AddRefreshTokenCommandHendler(DbContextNewsGenerator dbContext, IMapper mapper) 
        {
            Mapper = mapper;
            DbContext = dbContext;
        }

        public async Task<int> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            RefreshToken token = DbContext.RefreshTokens.FirstOrDefault(el => el.UserId == request.UserId);
            if (token == null)
            {
                RefreshToken RToken = Mapper.Map<RefreshToken>(request.RefreshToken);
                
                DbContext.RefreshTokens.Add(RToken);
                return await DbContext.SaveChangesAsync();
            }
            else 
            {
                token.CreateTimeToken = request.RefreshToken.CreateTimeToken;
                token.ExpiresToken = request.RefreshToken.ExpiresToken;
                token.Key = request.RefreshToken.Key;
                return await DbContext.SaveChangesAsync();
            }
            
            
        }
    }
}
