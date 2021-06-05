 using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.ViewModel.Account;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Controllers
{
   
    public class AccountController : Controller
    {
        private readonly IUserService DbContextUser;
        private readonly IRoleService DbContetRole;  

        public AccountController(IUserService dbContextUser, IRoleService dbContetRole)
        {
            DbContextUser = dbContextUser;
            DbContetRole = dbContetRole;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminAccount() 
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(UserModelDTO user)
        {
            if (ModelState.IsValid)
            {

                List<UserModelDTO> AllUser = DbContextUser.GetAllUser().ToList();

                IEnumerable<RoleModelDTO> UserRole = DbContetRole.GetAllUserWithRole().Result;
                RoleModelDTO User = UserRole.FirstOrDefault();
                RoleModelDTO Admin = UserRole.Last();

                if (AllUser != null && AllUser.All(el => el.Login != user.Login && el.Email != user.Email))
                {
                    await DbContextUser.СreateUser(new UserModelDTO()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Login = user.Login,
                        Password = DbContextUser.EncryptionPassword(user.Password),
                        RoleId = User.Id
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction(nameof(Login), "Account");
                }
            }

            return View(user);

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  
                    UserModelDTO user = DbContextUser.GetUserBy(login.Email);
                    if (user != null)
                    {
                        await DbContextUser.AdminInitialization();
                        await Authenticate(user);
                      

                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"{ e.Message} - {e.StackTrace}");
            }
            return View(login);
        }

        private async Task Authenticate(UserModelDTO user)
        {
            try
            {
                if (user != null)
                {

                    List<Claim> claims = new List<Claim> // создаём лист клаймов для хранения данных пользоватея
                    {
                       new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email), // сохраняем емеил пользователя (будет считаться именем пользователя)
                       new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Roles.NameRole) // сохраняем роль пользователя (будет считаться ролью пользоватедя)
                    };

                    //создаём объект в котором хранится список всех данных пользователя , настраиваем что бы они сохранялись в куки , и коворим что в нем есть имя и роль
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)); // передаём в куки настройку которая говорит что сохраняем в куки и создаё объект авторизации клаймс принцепал который хранит в себе свойство User типа клаймс едентити которому мы присваеваем эдентити с нашими клаймс
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message} - {e.StackTrace}");
            }
        }
    }
}
