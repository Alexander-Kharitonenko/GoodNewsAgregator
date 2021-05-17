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
    public class NewsRepository : Repository<News>
    {
        public NewsRepository(DbContextNewsGenerator dbContext) : base(dbContext)
        {

        }
    }
}
