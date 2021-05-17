using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Interfaces_Repositories;


namespace GoodNewsGenerator_Implementation_Repositories
{
    public class UserRepository : Repository<User> , IUserRepository
    {
        public UserRepository(DbContextNewsGenerator dbContext) : base(dbContext)
        {

        }
        public void UpdateUser(User user)
        {
            var User = Table.FirstOrDefault(el => el.Id == user.Id);
            User.RoleId = user.RoleId;
        }
    }
}
