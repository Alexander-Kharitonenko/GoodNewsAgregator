using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Models.ViewModel.News
{
    public class ViewModelForComments
    {
        public Guid UserId { get; set; }
        public Guid NewsId { get; set; }
        public string TextComment { get; set; }
       
    }
}
