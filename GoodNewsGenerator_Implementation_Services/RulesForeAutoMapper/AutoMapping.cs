using AutoMapper;
using DTO_Models;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Services.RulesForeAutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            CreateMap<NewsModelDTO, News>();
            CreateMap<News, NewsModelDTO>();
          
            CreateMap<UserModelDTO, User>();
            CreateMap<User, UserModelDTO>();
            CreateMap<User, UserModelDTO>()
           .ForMember<RoleModelDTO>(opt => opt.Roles, opt => opt.MapFrom<Role>(c => c.Roles));

            CreateMap<SourceModelDTO, Source>();
            CreateMap<Source, SourceModelDTO>();

            CreateMap<Role, RoleModelDTO>();
            CreateMap<RoleModelDTO, Role>();

            CreateMap<RefreshTokenModelDTO, RefreshToken>();
            CreateMap<RefreshToken, RefreshTokenModelDTO>();



        }
    }
}
