using DTO_Models_For_GoodNewsGenerator;
using GoodNewsGenerator.Models.ViewModel.Rss;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Controllers
{
    public class RssSources : Controller
    {

        private readonly ISourceService DbContext;

        public RssSources(ISourceService dbContext) 
        {
            DbContext = dbContext;
        }

        public ActionResult RssSource() // получение всех не повторяющихся источников
        {
            ModelForViewRssSources rssSources = new ModelForViewRssSources()
            {
                sources = DbContext.GetAllSource().Select(el => el.SourseURL).Distinct().ToList()
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

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModelForCreateRss collection)
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

                    return RedirectToAction(nameof(RssSource));
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

        // GET: RssSources/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RssSources/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

     

        // GET: RssSources/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RssSources/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RssSources/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RssSources/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
