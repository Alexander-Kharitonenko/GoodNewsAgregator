using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator.Models.ViewModel.Role;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService DbContextRole;
        private readonly IUserService DbContextUser;
        public RoleController(IRoleService dbContextRole, IUserService dbContextUser)
        {
            DbContextUser = dbContextUser;
            DbContextRole = dbContextRole;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AllUsersWithRole()
        {
           IEnumerable<UserModelDTO> userDTO = DbContextUser.GetAllUser();
            if (userDTO != null)
            {

                RoleViewModel Role = new RoleViewModel()
                {
                    user = userDTO   
                };

                return View(Role);
            }
            else 
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            var Role = DbContextRole.GetAllUserWithRole();
            AllRoleViewModel allRole = new AllRoleViewModel()
            {
                AllRole = Role.Result.Distinct().ToList(),
                Role = new RoleModelDTO()
            };
            return View(allRole);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult AddRole(string Role)
        {
            RoleModelDTO NewsRole = new RoleModelDTO()
            {
                Id = Guid.NewGuid(),
                NameRole = Role

            };
            if(NewsRole != null)
            DbContextRole.Create(NewsRole.NameRole);
            return RedirectToAction(nameof(AddRole));
        }
    }
}
