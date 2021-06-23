using DTO_Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Command
{
    public class AddRefreshTokenCommand : IRequest<int>
    {
        public Guid UserId { get; set; }
        public RefreshTokenModelDTO RefreshToken { get; set; }
    }
}
