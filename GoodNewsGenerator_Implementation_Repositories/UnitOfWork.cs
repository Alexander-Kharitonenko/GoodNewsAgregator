using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Repositories
{
    public class UnitOfWork :IDisposable, IUnitOfWork
    {

        private readonly DbContextNewsGenerator DbContext;

        private readonly IRepository<News> _News;

        private readonly IRepository<Comment> _Comment;

        private readonly IUserRepository _User;

        private readonly IRepository<Role> _Role;

        private readonly IRepository<Source> _Source;


        public UnitOfWork(DbContextNewsGenerator dbContext, IRepository<News> news, IRepository<Comment> comment, IUserRepository user, IRepository<Role> role, IRepository<Source> source)
        {
            DbContext = dbContext;
            _News = news;
            _User = user;
            _Role = role;
            _Comment = comment;
            _Source = source; 

        }

        public IRepository<News> News { get { return _News; } }

        public IRepository<Comment> Comment { get { return _Comment; } }

        public IUserRepository User { get { return _User; } }

        public IRepository<Role> Role { get { return _Role; } }

        public IRepository<Source> Source { get { return _Source; } }

        

        public void Dispose()
        {
            DbContext?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
