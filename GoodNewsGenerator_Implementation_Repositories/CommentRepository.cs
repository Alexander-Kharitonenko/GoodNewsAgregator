using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using System;

namespace GoodNewsGenerator_Implementation_Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(DbContextNewsGenerator dbContext) : base(dbContext) 
        {
            
        }
    }
}
