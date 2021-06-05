using DTO_Models_For_GoodNewsGenerator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Queries
{
    public  class GetRssSourceByIdQueriy : IRequest<SourceModelDTO>
    {
        public Guid id { get; set; }
    }
}
