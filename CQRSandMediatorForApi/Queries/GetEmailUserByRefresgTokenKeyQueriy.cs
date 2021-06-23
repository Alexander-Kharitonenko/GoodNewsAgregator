using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Queries
{
    public class GetEmailUserByRefresgTokenKeyQueriy : IRequest<string>
    {
        public string Key { get;set; }
    }
}
