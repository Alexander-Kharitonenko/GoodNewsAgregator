using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityGeneratorNews.Data
{
    public class Source : IBaseEntity
    {
        public Guid Id { get; set; }
        public string SourseURL { get; set; }
    }
}
