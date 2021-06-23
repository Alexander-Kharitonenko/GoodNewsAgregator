using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Models.ViewModel.News
{
    public class ViewModelForDetailsNews
    {
        public Guid Id { get; set; }

        public Guid SourcesId { get; set; }

        public string NewsURL { get; set; }

        public string Img { get; set; }

        public virtual IEnumerable<CommentModelDTO> Comments { get; set; }

        public int? CoefficientPositive { get; set; }

        public DateTime DateTime { get; set; }

        public string Heading { get; set; }

        public string Content { get; set; }

    }
}
