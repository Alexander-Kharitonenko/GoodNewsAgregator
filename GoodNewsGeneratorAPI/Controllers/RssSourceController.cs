using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssSourceController : ControllerBase
    {

        private readonly ISourceCQRService SourceCQRService;
        public RssSourceController(ISourceCQRService sourceCQRService) 
        {
            SourceCQRService = sourceCQRService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id) 
        {
           SourceModelDTO source = await SourceCQRService.GetSourceById(id);
            return Ok(source);
        }

        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<SourceModelDTO> source = await SourceCQRService.GetAllSource();
            return Ok(source);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Post(string url)
        {
            await SourceCQRService.Add(url);
            return Ok(HttpContext.Response.StatusCode);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Post(Guid id)
        {
            await SourceCQRService.Delete(id);
            return Ok(HttpContext.Response.StatusCode);
        }
    }
}
