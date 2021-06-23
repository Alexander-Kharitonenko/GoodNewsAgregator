using DTO_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.JWT
{
    public interface IJwtAuthManager
    {
        public Task<ResultGenerateToken> GetTokens(string email, IEnumerable<Claim> claims);
        Task<bool> ValidationRefreshToken(string key);
        Task<RefreshTokenModelDTO> GetRefreshToken(Guid userId);
    }
}
