using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
using DTO_Models;
using GoodNewsGeneratorAPI.Models.JwtModel;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.JWT
{
    public class GenerateTokens : IJwtAuthManager
    {
        private readonly IOptions<JwtOptions> Options;
        private readonly IMediator Mediator;
       
        public GenerateTokens(IOptions<JwtOptions> options, IConfiguration startapConfiguration, IMediator mediator) 
        {
            Options = options;
            Mediator = mediator;


        }

        public async Task<ResultGenerateToken> GetTokens(string email, IEnumerable<Claim> claims)
        {
            JwtSecurityToken createToken = new JwtSecurityToken(
                Options.Value.Issuer,//Указываем издателя токена
                Options.Value.Audience,// указываем получатея токена
                claims,// передаём клеймсы в токен
                expires: DateTime.Now.AddMinutes(Options.Value.TokenLifeTime),// устанавливаем время жизни токенна 
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Value.Key)), SecurityAlgorithms.HmacSha256Signature));// настраиваем тип шифрованя и тип кодировки , передаём ключь для шифрования
                                                                                                                                                                         // создаём новый объект токена передавая в него все необходимые параметры

            string token = new JwtSecurityTokenHandler().WriteToken(createToken);// создвем токен из объекта токена
            Claim ClaimUserId = claims.FirstOrDefault(el => el.Type == "UserId");
            Guid UserId = new Guid($"{ClaimUserId.Value}");
            RefreshTokenModelDTO refreshToken = await GetRefreshToken(UserId);
            return new ResultGenerateToken()
            {
                Toket = token,
                RefreshToken = new RefreshToken()
                {
                    Token = refreshToken.Key,
                    LlfeTime = refreshToken.ExpiresToken
                }
            };
        }

        public async Task<RefreshTokenModelDTO> GetRefreshToken(Guid userId)
        {
            RefreshTokenModelDTO refresh = new RefreshTokenModelDTO()
            {
                Id = Guid.NewGuid(),
                CreateTimeToken = DateTime.Now,
                ExpiresToken = DateTime.Now.AddHours(1),
                Key = Guid.NewGuid().ToString("D"),
                UserId = userId

            };
           await Mediator.Send(new AddRefreshTokenCommand() { RefreshToken = refresh , UserId = userId });

           RefreshTokenModelDTO refreshTokenWithDb = await Mediator.Send(new GetRefreshTokenByKeyQueriy() { Key = refresh.Key});

          
            return refresh;
        }


        public async Task<bool> ValidationRefreshToken(string key)
        {
            RefreshTokenModelDTO refreshToken =  await Mediator.Send(new GetRefreshTokenByKeyQueriy() { Key = key });
            if (refreshToken != null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        
    }
}
