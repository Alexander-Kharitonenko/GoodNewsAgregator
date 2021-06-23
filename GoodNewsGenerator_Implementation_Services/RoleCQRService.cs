using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Services
{
    public class RoleCQRService : IRoleCQRService
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;
        public RoleCQRService(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        public async Task Create(string NameRole)
        {
           await Mediator.Send(new AddRoleCommand() { NameRole = NameRole });

        }

        public async Task<IEnumerable<UserModelDTO>> GetAllUserWithRole()
        {
            GetAllUserWithRole request = new GetAllUserWithRole();
            IEnumerable<UserModelDTO> allUser =  await Mediator.Send(request);
            return allUser;

        }

        public async Task<RoleModelDTO> GetRoleById(Guid id)
        {
            GetRoleByIdQueriy requesr = new GetRoleByIdQueriy() { id = id };
            RoleModelDTO role = await Mediator.Send(requesr);
            return role;
        }

        public async Task RemoveRole(Guid id)
        {
           await Mediator.Send(new DeleteRoleCommand() { id = id });

        }
    }
}
