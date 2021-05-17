using System;
using System.Collections.Generic;

namespace DTO_Models_For_GoodNewsGenerator
{
    public class NewsModelDTO
    {
        public Guid Id { get; set; }

        public Guid SourcesId { get; set; }

        public string NewsURL { get; set; }

        public string Img { get; set; }

        public virtual IEnumerable<CommentModelDTO> Comments { get; set; }

        public float CoefficientPositive { get; set; }

        public DateTime DateTime { get; set; }

        public string Heading { get; set; }

        public string Content { get; set; }

     
    }
}
