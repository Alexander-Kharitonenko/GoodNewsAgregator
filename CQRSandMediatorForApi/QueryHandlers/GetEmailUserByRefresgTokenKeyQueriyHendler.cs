using AutoMapper;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.QueryHandlers
{
    public class GetEmailUserByRefresgTokenKeyQueriyHendler : IRequestHandler<GetEmailUserByRefresgTokenKeyQueriy, string>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        private readonly IMediator Mediator;

        public GetEmailUserByRefresgTokenKeyQueriyHendler(DbContextNewsGenerator dbContext, IMapper mapper, IMediator mediator) 
        {
            DbContext = dbContext;
            Mapper = mapper;
            Mediator = mediator;
        }
        public async Task<string> Handle(GetEmailUserByRefresgTokenKeyQueriy request, CancellationToken cancellationToken)
        {
            RefreshToken token = DbContext.RefreshTokens.FirstOrDefault(el => el.Key.Equals(request.Key));

            UserModelDTO User = await Mediator.Send(new GetUserByIdQueriy() { Id = token.UserId.ToString() });
            return User.Email;

        }
    }
}
