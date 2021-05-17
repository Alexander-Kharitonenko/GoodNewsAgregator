using EntityGeneratorNews.Data;
using GoodNewsGenerator_Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Repositories
{
   public interface IUnitOfWork : IDisposable
    {
        IRepository<News> News {get;}
        IRepository<Comment> Comment { get; }
        IUserRepository User { get; }
        IRepository<Role> Role { get; }
        IRepository<Source> Source { get; }


        Task<int> SaveChangesAsync(); 
    }
}
