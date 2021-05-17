using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator.Services;
using GoodNewsGenerator.Services.Paginator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Models.ViewModel.News
{
    public class ViewModelForNews
    {

        public IEnumerable<NewsModelDTO> news;
        public PageInfo pageInfo { get; set; } 
    }
}
