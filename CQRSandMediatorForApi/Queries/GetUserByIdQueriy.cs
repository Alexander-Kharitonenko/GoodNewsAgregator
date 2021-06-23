using DTO_Models_For_GoodNewsGenerator;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Queries
{
    public class GetUserByIdQueriy : IRequest<UserModelDTO>
    {
        public string Id { get; set; }
    }
}
