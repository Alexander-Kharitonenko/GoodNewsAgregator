using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GoodNewsGenerator_Implementation_Services
{
    public class NewsCQRService : INewsCQRService
    {
        private readonly IMediator Mediator;
        private readonly IMapper _Mapper;

        public NewsCQRService(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            _Mapper = mapper;
        }



        public async Task<NewsModelDTO> GetNewsById(Guid id) // получить новость по id
        {
            GetNewsByIdQueriy request = new GetNewsByIdQueriy() { Id = id };
            NewsModelDTO news = await Mediator.Send(request);
            return news;
        }



        public async Task<IEnumerable<NewsModelDTO>> GetAllNews()
        {
            GetAllNewsQueriy request = new GetAllNewsQueriy();
            IEnumerable<NewsModelDTO> allnews = await Mediator.Send(request);
            return allnews;
        }

        public async Task<IEnumerable<NewsModelDTO>> GetNewsFromRssSource()
        {
            //1.Получить все источники
            IEnumerable<SourceModelDTO> allRss = await Mediator.Send(new GetAllRssSourceQueriy());

            //2.Получить урлы всех новостей имеющихся в бд
            IEnumerable<string> allUrl = await Mediator.Send(new GetUrlFromNewsQueriy());

            //3.Создать Колеекцию которая позволит работать с потоками
            ConcurrentBag<NewsModelDTO> news = new ConcurrentBag<NewsModelDTO>();

            //4.Извлекать новости из рсс
            Parallel.ForEach(allRss, (Rss) =>
            {
                //5.Прочитать и распарсить rss источник
                using (XmlReader reader = XmlReader.Create(Rss.SourseURL))
                {

                    //5.1 считываем rss
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                   reader.Close();

                   if (feed.Items.Any())
                   {
                       foreach (SyndicationItem items in feed.Items.Where(el => !allUrl.Any(Url => Url.Equals(el.Id))))
                       {
                            if (items.Summary.Text.Contains("https://"))
                            {
                                string imgUrl = Regex.Replace(items.Summary.Text.ToString(), @"[а-яёА-ЯЁ]", string.Empty).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2].Replace("src=", "").Replace("\"", "");
                                string content = Regex.Replace(items.Summary.Text.ToString(), @"<[^>]*(>|$)", string.Empty);

                                NewsModelDTO News = new NewsModelDTO()
                                {
                                    Id = Guid.NewGuid(),
                                    SourcesId = Rss.Id,
                                    NewsURL = items.Id,
                                    DateTime = DateTime.Now,
                                    Content = content,
                                    Heading = items.Title.Text,
                                    Img = imgUrl,
                                };
                                news.Add(News);
                            }
                            else 
                            {
                                string content = Regex.Replace(items.Summary.Text.ToString(), @"<[^>]*(>|$)", string.Empty);
                                NewsModelDTO News = new NewsModelDTO()
                                {
                                    Id = Guid.NewGuid(),
                                    SourcesId = Rss.Id,
                                    NewsURL = items.Id,
                                    DateTime = DateTime.Now,
                                    Content = content,
                                    Heading = items.Title.Text,
                                    Img = @"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRTrIUAHP1tTymrh_-Jn6ivvaKWkSLI8T57ulghIN86FWs58z7MmjCuXDlTIXZoUbZnprA&usqp=CAU",
                                };
                                news.Add(News);
                            }


                       }
                   }
                }
            });

            await Mediator.Send( new AddNewsCommand() { AllNews = news} );
            
            return news;
        }
    }
}
