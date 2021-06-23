using EntityGeneratorNews.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Models.Data
{
    public class DbContextNewsGenerator : DbContext
    {
        public DbContextNewsGenerator(DbContextOptions<DbContextNewsGenerator> options) : base(options)
        {
            
         
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
        

    }
}
