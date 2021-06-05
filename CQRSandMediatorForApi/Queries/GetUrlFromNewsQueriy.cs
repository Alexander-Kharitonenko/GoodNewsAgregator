using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Queries 
{
    public class GetUrlFromNewsQueriy : IRequest<IEnumerable<string>>
    {
    }
}
