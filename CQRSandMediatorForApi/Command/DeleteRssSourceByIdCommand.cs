using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Command
{
    public class DeleteRssSourceByIdCommand : IRequest<int>
    {
        public Guid id { get; set; }
    }
}
