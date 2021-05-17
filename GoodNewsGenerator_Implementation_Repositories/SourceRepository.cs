using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Repositories
{
    public class SourceRepository : Repository<Source>
    {
        public SourceRepository(DbContextNewsGenerator dbContext) : base(dbContext)
        {

        }
    }
}
