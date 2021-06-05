using DTO_Models_For_GoodNewsGenerator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Command
{
    public class AddNewsCommand : IRequest<int>
    {
        public IEnumerable<NewsModelDTO> AllNews { get; set; }
    }
}
