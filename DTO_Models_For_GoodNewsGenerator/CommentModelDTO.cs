using EntityGeneratorNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Models_For_GoodNewsGenerator
{
    public class CommentModelDTO
    {
        public Guid Id { get; set; }
        public string TextComment { get; set; }
        public DateTime DateTime { get; set; }


        public Guid UserId { get; set; }
        public virtual UserModelDTO Users { get; set; }

        public Guid NewsId { get; set; }
        public virtual NewsModelDTO Newss { get; set; }
    }
}
