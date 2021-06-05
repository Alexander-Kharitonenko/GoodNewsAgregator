using AutoMapper;
using CQRSandMediatorForApi.Command;
using CQRSandMediatorForApi.Queries;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Services
{
    public class SourceSQRService : ISourceCQRService
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;

        public SourceSQRService(IMediator mediator, IMapper mapper ) 
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        public async Task Add(string url)
        {
            AddRssSourceCommand postRequest = new AddRssSourceCommand() { SourseURL = url };
            await Mediator.Send(postRequest);
            
        }

        public async Task Delete(Guid id)
        {
            DeleteRssSourceByIdCommand deleteRss = new DeleteRssSourceByIdCommand() { id = id };
            await Mediator.Send(deleteRss);
        }

        public async Task<IEnumerable<SourceModelDTO>> GetAllSource()
        {
            GetAllRssSourceQueriy requestAllRssSource = new GetAllRssSourceQueriy();
            IEnumerable<SourceModelDTO> allSource = await Mediator.Send(requestAllRssSource);
            return allSource;
        }

        public async Task<SourceModelDTO>GetSourceById(Guid id)
        {
            GetRssSourceByIdQueriy requestSource = new GetRssSourceByIdQueriy() { id = id };
            SourceModelDTO source = await Mediator.Send(requestSource);
            return source;
        }
    }
}
