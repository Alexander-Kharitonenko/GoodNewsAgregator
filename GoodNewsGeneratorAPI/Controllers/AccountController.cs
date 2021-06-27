using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator.Models.ViewModel.Account;
using GoodNewsGenerator_Interfaces_Servicse;
using GoodNewsGeneratorAPI.JWT;
using GoodNewsGeneratorAPI.Models.JwtModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IUserCQRService UserCQRService;
        private readonly IRoleCQRService RoleCQRService;
        private readonly IOptions<JwtOptions> JwtOptions;
        private readonly IJwtAuthManager JwtManager;
        public AccountController(IUserCQRService userCQRService, IOptions<JwtOptions> jwtOptions, IRoleCQRService roleCQRService, IJwtAuthManager jwtManager)
        {
            UserCQRService = userCQRService;
            RoleCQRService = roleCQRService;
            JwtOptions = jwtOptions;
            JwtManager = jwtManager;
        }

        [HttpPost]
        [Route("Authorization")]
        public async Task<IActionResult> Post(UserModelDTO user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModelDTO User = await UserCQRService.GetUserBy(user.Email);
                    if (User == null)
                    {
                        user.Roles = await RoleCQRService.GetRoleById(new Guid("787d4edc-3590-4355-9da6-39d0198b63d1"));
                        await UserCQRService.СreateUser(user);
                        UserModelDTO UserWthDb = await UserCQRService.GetUserBy(user.Email);
                        ClaimsIdentity identity = GetIdentity(UserWthDb);
                        ResultGenerateToken Token = await JwtManager.GetTokens(UserWthDb.Email, identity.Claims);
                        return Ok(Token);
                    }

                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message} - не удалось зарегистрировать пользователя");

            }
            return BadRequest();

        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Post(LoginViewModel login)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    UserModelDTO user = await UserCQRService.GetUserBy(login.Email);
                    user.Roles = await RoleCQRService.GetRoleById(new Guid("787d4edc-3590-4355-9da6-39d0198b63d1"));
                    if (User != null)
                    {
                        await JwtManager.GetRefreshToken(user.Id);
                        ClaimsIdentity identity = GetIdentity(user);
                        ResultGenerateToken Jwtresult = await JwtManager.GetTokens(login.Email, identity.Claims);
                        return Ok(Jwtresult);
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message} - не удалось авторизовать пользователя");

            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Post(string Key)
        {
            try
            {
                if (!await JwtManager.ValidationRefreshToken(Key))
                {
                    return BadRequest();
                }
                string Email = await UserCQRService.GetUserEmailByTokenKey(Key);

                if (!string.IsNullOrEmpty(Email))
                {
                    UserModelDTO user = await UserCQRService.GetUserBy(Email);
                    user.Roles = await RoleCQRService.GetRoleById(new Guid("787d4edc-3590-4355-9da6-39d0198b63d1"));
                    ClaimsIdentity identity = GetIdentity(user);
                    ResultGenerateToken Jwtresult = await JwtManager.GetTokens(Email, identity.Claims);
                    return Ok(Jwtresult);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message} - не удалось авторизовать пользователя");

            }
            return BadRequest();
        }

        private ClaimsIdentity GetIdentity(UserModelDTO user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", user.Id.ToString("D")),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Roles.NameRole)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
