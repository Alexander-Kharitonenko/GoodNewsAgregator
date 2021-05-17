using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Interfaces_Servicse;
using Serilog;
using System.Xml;
using System.ServiceModel.Syndication;
using GoodNewsGenerator_Implementation_Services;
using GoodNewsGenerator.Models.ViewModel.News;
using GoodNewsGenerator.Services.Paginator;

namespace GoodNewsGenerator.Controllers
{
    public class NewsController : Controller
    {
        public readonly INewsService DbNewsContext;
        public readonly ISourceService DbSourceContext;
        private readonly TutByParser _TutByParser;
        private readonly OnlinerParser _OnlinerParser;
        private readonly belta _BeltaParser;

        public NewsController(INewsService dbNewsContext, ISourceService dbSourceContext, TutByParser TutByParser, OnlinerParser onlinerParser, belta BeltaParser)
        {
            DbNewsContext = dbNewsContext;
            DbSourceContext = dbSourceContext;
            _TutByParser = TutByParser;
            _OnlinerParser = onlinerParser;
            _BeltaParser = BeltaParser;
        }



        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult News(int page = 1)
        {
            int pageSize = 5;
            IEnumerable<NewsModelDTO> News = DbNewsContext.GetAllNews();
            int count = News.Count();
            List<NewsModelDTO> Itemc = News.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageInfo pageInfo = new PageInfo(count, page, pageSize);

            ViewModelForNews newsView = new ViewModelForNews()
            {
                news = Itemc,
                pageInfo = pageInfo
            };

            if (newsView.news != null)
            {
                return View(newsView);
            }
            else
            {
                return Redirect(nameof(GetNewsFromRss));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetNewsFromRss()
        {

            List<SourceModelDTO> RssSource = DbSourceContext.GetAllSource().ToList();
            List<NewsModelDTO> News = new List<NewsModelDTO>();

            Guid TuTby = RssSource.FirstOrDefault(x => x.SourseURL.Contains("tut.by")).Id;
            Guid Onliner = RssSource.FirstOrDefault(x => x.SourseURL.Contains("onliner.by")).Id;
            Guid Belta = RssSource.FirstOrDefault(x => x.SourseURL.Contains("belta.by")).Id;

            foreach (SourceModelDTO source in RssSource)
            {
                IEnumerable<NewsModelDTO> news = await DbNewsContext.GetNewsFromRssSourwace(source);

                try
                {

                    if (news.All(el => el.SourcesId.Equals(TuTby)))
                    {

                        Parallel.ForEach(news, async (newsDto) =>
                        {
                            (string content, string Img) newsBody = await _TutByParser.Parse(newsDto.NewsURL);
                            if (newsBody.content != null)
                            {
                                Log.Warning($"Новость из ТУТ.бай не распарсилась - {newsDto.NewsURL}");
                            }
                            newsDto.Content = newsBody.content;
                            newsDto.Img = newsBody.Img;
                        });

                        //foreach (var newsDto in news)
                        //{
                        //    var newsBody = await _TutByParser.Parse(newsDto.NewsURL);
                        //    newsDto.Content = newsBody.content;
                        //    newsDto.Img = newsBody.Img;
                        //}
                    }
                    else if (news.All(el => el.SourcesId.Equals(Onliner)))
                    {
                        Parallel.ForEach(news, async (newsDto) =>
                        {
                            (string content, string Img) newsBody = await _OnlinerParser.Parse(newsDto.NewsURL);

                            if (newsBody.content != null)
                            {
                                Log.Warning($"Новость из Онлайнера не распарсилась - {newsDto.NewsURL}");
                            }
                            newsDto.Content = newsBody.content;
                            newsDto.Img = newsBody.Img;



                        });
                        //foreach (var newsDto in news)
                        //{
                        //    var newsBody = await _OnlinerParser.Parse(newsDto.NewsURL);
                        //    newsDto.Content = newsBody.content;
                        //    newsDto.Img = newsBody.Img;
                        //}
                    }
                    else if (news.All(el => el.SourcesId.Equals(Belta)))
                    {
                        Parallel.ForEach(news, async (newsDto) =>
                        {
                            (string content, string Img) newsBody = await _BeltaParser.Parse(newsDto.NewsURL);

                            if (newsBody.content != null)
                            {
                                Log.Warning($"Новость из Белта не распарсилась - {newsDto.NewsURL}");
                            }
                            newsDto.Content = newsBody.content;
                            newsDto.Img = newsBody.Img;
                        });
                        //foreach (var newsDto in news)
                        //{
                        //    var newsBody = await _TjournalParser.Parse(newsDto.NewsURL);
                        //    newsDto.Content = newsBody.content;
                        //    newsDto.Img = newsBody.Img;
                        //}
                    }
                    News.AddRange(news);
                }
                catch (Exception e)
                {
                    Log.Error(e, $"{ e.Message}");
                }

            }

            await DbNewsContext.CreateNewsRangeAsync(News);
            return Redirect(nameof(News));
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            IEnumerable<NewsModelDTO> news = DbNewsContext.GetNewsById(id);
            NewsModelDTO NewsDetail = news.FirstOrDefault();
            if (NewsDetail == null)
            {
                return NotFound();
            }
            else 
            {
                ViewModelForDetailsNews detailsNews = new ViewModelForDetailsNews()
                {
                   Id = NewsDetail.Id,
                   SourcesId = NewsDetail.SourcesId,
                   NewsURL = NewsDetail.NewsURL,
                   CoefficientPositive = NewsDetail.CoefficientPositive,
                   DateTime = NewsDetail.DateTime,
                   Heading = NewsDetail.Heading,
                   Img = NewsDetail.Img,
                   Content = NewsDetail.Content,
                   

                };
                return View(detailsNews);
            }
        }

    }
}
