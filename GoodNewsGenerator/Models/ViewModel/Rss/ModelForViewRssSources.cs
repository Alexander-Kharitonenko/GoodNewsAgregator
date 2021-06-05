using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Models.ViewModel.Rss
{
    public class ModelForViewRssSources
    {
        public IEnumerable<KeyValuePair<string, Guid>> sources;
    }
}
