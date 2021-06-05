using AutoMapper;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace GoodNewsGenerator_Implementation_Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;
        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _Mapper = mapper;
        }

        public async Task CreateNews(NewsModelDTO news) // Добавляеем новости в БД по одной
        {

            News news1 = _Mapper.Map<News>(news);
            await _unitOfWork.News.Add(news1);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task CreateNewsRangeAsync(IEnumerable<NewsModelDTO> news) // Добавляеем множество новости в БД разом 
        {

            List<News> BoxNews = news.Select(el => _Mapper.Map<News>(el)).ToList();
            await _unitOfWork.News.AddRange(BoxNews);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteNews(NewsModelDTO entity) // удоляем новость по 1
        {
            News news1 = _Mapper.Map<News>(entity);
            await _unitOfWork.News.Remove(news1.Id);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task DeleteNewsRangeAsync(IEnumerable<NewsModelDTO> news)// удоляем  список новостей
        {
            List<News> BoxNews = news.Select(el => _Mapper.Map<News>(el)).ToList();
            await _unitOfWork.News.RemoveRange(BoxNews);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<NewsModelDTO> GetAllNews() // получить все новости из бд
        {
            IQueryable<News> news = _unitOfWork.News.GetAllEntity(x => x.Content != null);
            List<NewsModelDTO> news1 = news.Select(el => _Mapper.Map<NewsModelDTO>(el)).ToList();
            return news1;
        }

        public IEnumerable<NewsModelDTO> GetNewsById(Guid id) // получить новость по id
        {
            IQueryable<News> BoxNews = _unitOfWork.News.GetById(id);
            IEnumerable<NewsModelDTO> NewsBuId = BoxNews.Select(el => _Mapper.Map<NewsModelDTO>(el));
            return NewsBuId;
        }

        public void UpdateNews(NewsModelDTO entity) //изменить новость 
        {
            News news = _Mapper.Map<News>(entity);
            _unitOfWork.News.Update(news);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<NewsModelDTO>> GetNewsFromRssSourwace(SourceModelDTO source)
        {

            List<NewsModelDTO> News = new List<NewsModelDTO>();

            using (XmlReader reader = XmlReader.Create(source.SourseURL)) // получаем  XML документ из Source
            {
                SyndicationFeed feed = SyndicationFeed.Load(reader); // загружаем весь контент из Source0 в переменную feed(Автоматически расспарсит весь rss)
                reader.Close();//Dispose

                var currentNewsUrls = await _unitOfWork.News
                  .Get()//rssSourseId must be not nullable
                  .Select(n => n.NewsURL)
                  .ToListAsync();

                const string SputnikUrl = "sputnik.by/economy/";
                const string OnlinerUrl = "people.onliner";
                const string beltaUrl = "belta.by";

                foreach (SyndicationItem SyndicationItems in feed.Items) //Items - количество новостей в feed
                {

                    if (!currentNewsUrls.Any(url => url.Equals(SyndicationItems.Id)))
                    {
                        if (SyndicationItems.Id.Contains(beltaUrl) || SyndicationItems.Id.Contains(OnlinerUrl) || SyndicationItems.Id.Contains(SputnikUrl))
                        {
                            NewsModelDTO news = new NewsModelDTO()
                            {
                                Id = Guid.NewGuid(),
                                DateTime = DateTime.Now,
                                SourcesId = source.Id,
                                NewsURL = SyndicationItems.Id, // юрл
                                Heading = SyndicationItems.Title.Text, // заголовок 
                            };

                            News.Add(news);
                        }
                    }

                }
            }
            return News;
        }
    }
}
