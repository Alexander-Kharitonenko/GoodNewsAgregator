using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator.Models.ViewModel.Rss;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RssSourcesController : Controller
    {

        private readonly ISourceService DbContext;

        public RssSourcesController(ISourceService dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult RssSource() // получение всех не повторяющихся источников
        {

            ModelForViewRssSources rssSources = new ModelForViewRssSources()
            {
                sources = DbContext.GetAllSource().ToDictionary(el => el.SourseURL, elm => elm.Id).Distinct()

            };

            if (rssSources.sources != null)
            {
                return View(rssSources);
            }
            else
            {
                return Redirect(nameof(ErroNullSourse));
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ViewModelForCreateRss collection)
        {
            try
            {
                if (collection.RssName != null)
                {
                    SourceModelDTO source = new SourceModelDTO()
                    {
                        SourseURL = collection.RssName,
                        Id = Guid.NewGuid()
                    };

                    DbContext.Add(source);

                    return Redirect(nameof(RssSource));
                }
            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message}");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ErroNullSourse()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(ViewModelForDelete Rss)
        {
            SourceModelDTO source = DbContext.GetSourceById(Rss.id);
            if (source != null)
            {
                await DbContext.DeletSourceById(source.Id);
               return Redirect(nameof(RssSource));
            }

           return View();
            
           
        }

    }
}
