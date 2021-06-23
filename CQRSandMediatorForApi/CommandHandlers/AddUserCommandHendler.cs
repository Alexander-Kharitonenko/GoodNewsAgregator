using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
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

namespace CQRSandMediatorForApi.CommandHandlers
{
    public class AddUserCommandHendler : IRequestHandler<AddUserCommand, int>
    {
        private readonly DbContextNewsGenerator DbContext;
        private readonly IMapper Mapper;
        private readonly IMediator Mediator;
        private readonly IUserCQRService UserCQRService;
        public AddUserCommandHendler(DbContextNewsGenerator dbcontext, IMapper mapper, IMediator mediator, IUserCQRService userCQRService) 
        {
            Mediator = mediator;
            DbContext = dbcontext;
            Mapper = mapper;
            UserCQRService = userCQRService;
        }

        public async Task<int> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {

            if (request.User != null && DbContext.Users.All(el => el.Email != request.User.Email && el.Login != request.User.Login)) 
            {
                Role Role = Mapper.Map<Role>(await Mediator.Send(new GetRoleByIdQueriy() { id = new Guid("787d4edc-3590-4355-9da6-39d0198b63d1") }));

                User user = new User()
                {
                    Id = Guid.NewGuid(),
                    RoleId = Role.Id,
                    Email = request.User.Email,
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Login = request.User.Login,
                    Password = UserCQRService.EncryptionPassword(request.User.Password),
                    
                };
                DbContext.Users.Add(user);
            }
            return await DbContext.SaveChangesAsync();
        }
    }
}
