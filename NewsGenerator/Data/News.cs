using GoodNewsGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityGeneratorNews.Data
{
    public class News : IBaseEntity
    {
        public Guid Id { get; set; }
        public string NewsURL { get; set; }
        public string Img { get; set; }
        public float CoefficientPositive { get; set; }
        public DateTime DateTime { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }


        public Guid SourcesId { get; set; }
        public virtual Source Sources { get; set; }


        public Guid CommentsId { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
