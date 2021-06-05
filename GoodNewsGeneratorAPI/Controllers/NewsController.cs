using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsCQRService NewsCQRService;
        
        public NewsController(INewsCQRService newsCQRService)
        {
            NewsCQRService = newsCQRService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
           NewsModelDTO news = await NewsCQRService.GetNewsById(id);
            return Ok(news);
        }

        [HttpGet]
        [Route("allnews")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<NewsModelDTO> news = await NewsCQRService.GetAllNews();
            return Ok(news);
        }

        [HttpPost]
        [Route("Сreate")]
        public async Task<IActionResult> Post()
        {
            IEnumerable<NewsModelDTO> x = await NewsCQRService.GetNewsFromRssSource();
            
            return Ok(x);
        }
    }
}
