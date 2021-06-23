using AutoMapper;
using CQRSandMediatorForApi.Command;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using MediatR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.CommandHandlers
{
    public class UpdateNewsCommandHendler : IRequestHandler<UpdateNewsCommand, int>
    {
        private readonly DbContextNewsGenerator Dbcontext;
        private readonly IMapper Mapper;
        public UpdateNewsCommandHendler(DbContextNewsGenerator dbcontext, IMapper mapper) 
        {
            Dbcontext = dbcontext;
            Mapper = mapper;
        }

        public async Task<int> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<News> News = request.updateNews.Select(el => Mapper.Map<News>(el));
            List<News> UpdateNews = new List<News>();

            foreach (News newsUpd in News)
            {

                News DBnews = Dbcontext.News.FirstOrDefault(el => el.Id == newsUpd.Id);
                DBnews.CoefficientPositive = newsUpd.CoefficientPositive;
                await Dbcontext.SaveChangesAsync();
            }

            return await Dbcontext.SaveChangesAsync();

        }                          
    }
}
