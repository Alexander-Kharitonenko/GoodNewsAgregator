using AutoMapper;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork DbContext;
        private readonly IMapper Mapper;
        public RoleService(IUnitOfWork dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        public async Task Create(string NameRole)
        {
            if (NameRole != null)
            {
                RoleModelDTO role = new RoleModelDTO()
                {
                    Id = Guid.NewGuid(),
                    NameRole = NameRole
                };
                var newsRole = Mapper.Map<Role>(role);

                await DbContext.Role.Add(newsRole);
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RoleModelDTO>> GetAllUserWithRole()
        {
            IQueryable<Role> UserWithRole = DbContext.Role.GetAllEntity(el => el.NameRole != null);
            IEnumerable<RoleModelDTO> userWithRole = UserWithRole.Select(el => Mapper.Map<RoleModelDTO>(el));
            return userWithRole;
        }

        public RoleModelDTO GetRoleById(Guid id)
        {
            IQueryable<EntityGeneratorNews.Data.Role> Role = DbContext.Role.GetById(id);
            RoleModelDTO role = Role.Select(el => Mapper.Map<RoleModelDTO>(el)).FirstOrDefault();
            return role;
        }

        public async Task RemoveRole(Guid id)
        {
            await DbContext.Role.Remove(id);
            await DbContext.SaveChangesAsync();
        }
    }
}
