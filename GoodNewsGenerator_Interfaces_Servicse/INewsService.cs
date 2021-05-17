using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface INewsService
    {
        IEnumerable<NewsModelDTO> GetNewsById(Guid id);
        IEnumerable<NewsModelDTO> GetAllNews();
        Task CreateNews(NewsModelDTO news);
        Task CreateNewsRangeAsync(IEnumerable<NewsModelDTO> news);

        Task<IEnumerable<NewsModelDTO>> GetNewsFromRssSourwace(SourceModelDTO source);
        Task DeleteNewsRangeAsync(IEnumerable<NewsModelDTO> news);
        Task DeleteNews(NewsModelDTO entity);
        void UpdateNews(NewsModelDTO entity);
    }
}
