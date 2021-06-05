using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleCQRService RoleCQRService;
        public RoleController(IRoleCQRService roleCQRService)
        {
            RoleCQRService = roleCQRService;
        }

        [HttpGet]
        [Route("AllUserWithRole")]
        public async Task<IActionResult> Get() 
        {
           await RoleCQRService.GetAllUserWithRole();
            return Ok(await RoleCQRService.GetAllUserWithRole());
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> Post(string nameRole)
        {
            await RoleCQRService.Create(nameRole);
            return Ok();
        }

        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> Post(Guid id)
        {
            await RoleCQRService.RemoveRole(id);
            return Ok();
        }
    }
}
