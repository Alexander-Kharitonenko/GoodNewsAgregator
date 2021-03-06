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
using System.Security.Claims;

namespace GoodNewsGenerator.Controllers
{
    public class NewsController : Controller
    {
        public readonly INewsService DbNewsContext;
        public readonly ISourceService DbSourceContext;
        private readonly SputnikParser _Sputnik;
        private readonly OnlinerParser _OnlinerParser;
        private readonly belta _BeltaParser;
        private readonly ICommentService CommentService;
        private readonly IUserService UserService;
        public NewsController(INewsService dbNewsContext, ISourceService dbSourceContext, SputnikParser sputnik, OnlinerParser onlinerParser, belta BeltaParser, ICommentService commentService, IUserService userService)
        {
            DbNewsContext = dbNewsContext;
            DbSourceContext = dbSourceContext;
            _Sputnik = sputnik;
            _OnlinerParser = onlinerParser;
            _BeltaParser = BeltaParser;
            CommentService = commentService;
            UserService = userService;
        }



        [HttpGet]
        [Authorize(Roles = "User,Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetNewsFromRss()
        {

            List<SourceModelDTO> RssSource = DbSourceContext.GetAllSource().ToList();
            List<NewsModelDTO> News = new List<NewsModelDTO>();

            Guid Sputnik = RssSource.FirstOrDefault(x => x.SourseURL.Contains("sputnik.by")).Id;
            Guid Onliner = RssSource.FirstOrDefault(x => x.SourseURL.Contains("onliner.by")).Id;
            Guid Belta = RssSource.FirstOrDefault(x => x.SourseURL.Contains("belta.by")).Id;

            foreach (SourceModelDTO source in RssSource)
            {
                IEnumerable<NewsModelDTO> news = await DbNewsContext.GetNewsFromRssSourwace(source);

                try
                {

                    if (news.All(el => el.SourcesId.Equals(Sputnik)))
                    {

                        Parallel.ForEach(news, async (newsDto) =>
                        {
                            (string content, string Img) newsBody = await _Sputnik.Parse(newsDto.NewsURL);
                            if (newsBody.content != null)
                            {
                                Log.Warning($"Новость из Sputnik не распарсилась - {newsDto.NewsURL}");
                            }
                            newsDto.Content = newsBody.content;
                            newsDto.Img = newsBody.Img;
                        });

                        //foreach (var newsDto in news)
                        //{
                        //    var newsBody = await _Sputnik.Parse(newsDto.NewsURL);
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
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Details(Guid id)
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
                    Comments = await CommentService.GetAllComents(NewsDetail.Id),
                    

                };
                return View(detailsNews);
            }
        }

       

        [HttpPost("id")]
        [Authorize(Roles = "User,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(ViewModelForDetailsNews request)
        {
            string EmailUser =  HttpContext.User.Claims.FirstOrDefault(el => el.Type == ClaimsIdentity.DefaultNameClaimType).Value;
           UserModelDTO user =  UserService.GetUserBy(EmailUser);

            CommentModelDTO newsComment = new CommentModelDTO()
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                NewsId = request.NewsId,
                TextComment = request.CommentText,
                UserId = user.Id

            };
            await CommentService.AddComents(newsComment);
            return RedirectToAction(nameof(Details), "News",new { id = request.NewsId });
        }
    }
}
