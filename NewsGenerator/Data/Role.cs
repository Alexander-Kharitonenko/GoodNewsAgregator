using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityGeneratorNews.Data
{
    public class Role : IBaseEntity
    {
        public Guid Id { get; set; }
        public string NameRole { get; set; }
    }
}
