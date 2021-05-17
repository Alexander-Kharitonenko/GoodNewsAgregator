using GoodNewsGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace EntityGeneratorNews.Data
{
    public class Comment : IBaseEntity
    {
        public Guid Id { get; set; }
        public string TextComment { get; set; }
        public DateTime DateTime { get; set; }


        public Guid UserId { get; set; }
        public virtual User Users { get; set; }
        
    }
}
