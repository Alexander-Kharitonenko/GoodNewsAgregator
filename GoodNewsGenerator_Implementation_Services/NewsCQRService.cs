using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
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

        public async Task<IEnumerable<NewsModelDTO>> GetNewsFromRssSource()//
        {
            //1.Получить все источники
            IEnumerable<SourceModelDTO> allRss = await Mediator.Send(new GetAllRssSourceQueriy());

            //2.Получить урлы всех новостей имеющихся в бд
            IEnumerable<string> allUrl = await Mediator.Send(new GetUrlFromNewsQueriy());

            //3.Создать Колеекцию которая позволит работать с потоками
            ConcurrentBag<NewsModelDTO> news = new ConcurrentBag<NewsModelDTO>();

            //4.Извлекать новости из рсс
            Parallel.ForEach(allRss, async (Rss) =>
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

            await Mediator.Send(new AddNewsCommand() { AllNews = news });

            return news;
        }

        public async Task CoefficientPositivity()
        {

            IEnumerable<NewsModelDTO> allNews = await Mediator.Send(new GetAllNewsQueriy());

            string data;
            int Coefficient = 0;
            int y;
            string path = @"C:\Users\Александр\Desktop\С#\Проект35 - Генератор хороших новостей\GoodNewsGenerator\GoodNewsGeneratorAPI\AFINN-ru.json";

            List<NewsModelDTO> News = allNews.Where(el => el.CoefficientPositive == null).Take(30).ToList();

            foreach (NewsModelDTO news in News)
            {
                using (HttpClient request = new HttpClient())
                {
                    request.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); // говорим что формат заголовка по умолчанию json
               
                    HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=a3b5afea2ad9a4c948f4457db9342383415605dd")
                    {
                        Content = new StringContent("[{\"text\":\"" + news.Content + "\"}]", Encoding.UTF8, "application/json")
                    }; //создаём запрос который мы зарание настроили как HttpMethod.Post по адресу http://api.ispras.ru и формат контента этого запроса json

                    HttpResponseMessage respons = await request.SendAsync(postRequest); // SendAsync отправляет наш запрос и возвращает ответ
                    data = await respons.Content.ReadAsStringAsync();

                    await using (FileStream sr = File.OpenRead($"{path}"))
                    {
                        byte[] array = new byte[sr.Length]; // создаём буфир записи а который будут записан считанный текст в виде массива байт

                        sr.Read(array, 0, array.Length); // считываем данные и записываем в буфер

                        string textFromFile = System.Text.Encoding.UTF8.GetString(array);// декодируем байты в строку 

                        Dictionary<string, string> Dictionari = JsonConvert.DeserializeObject<Dictionary<string, string>>(textFromFile);

                        if (data != null)
                        {
                            List<string> contentData = Regex.Replace(Regex.Replace(Regex.Replace(data, @"[a-zA-Z0-9]", ""), @"[\""-.?!)}\]\[{(,]", ""), @"(:)\1+", ":").Split(':').ToList<string>();
                            contentData.RemoveAt(0);
                            foreach (KeyValuePair<string, string> items in Dictionari)
                            {
                                if (contentData.Any(el => el.Contains(items.Key)))
                                {

                                    if (int.TryParse(items.Value, out y) && y != 0)
                                    {
                                        Coefficient += y;
                                    }
                                }
                            }
                        }



                        news.CoefficientPositive = Coefficient;
                        Coefficient = 0;
                    }
                }
            }
            await Mediator.Send(new UpdateNewsCommand() { updateNews = News });
        }
    }
}