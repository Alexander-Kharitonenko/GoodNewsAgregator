using DTO_Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Queries
{
    public class GetRefreshTokenByKeyQueriy : IRequest<RefreshTokenModelDTO>
    {
        public string Key { get; set; }
    }
}
