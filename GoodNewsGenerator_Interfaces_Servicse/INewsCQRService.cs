using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface INewsCQRService
    {
        Task<NewsModelDTO> GetNewsById(Guid id);
        Task<IEnumerable<NewsModelDTO>> GetAllNews();

        Task<IEnumerable<NewsModelDTO>> GetNewsFromRssSource();

        Task CoefficientPositivity();


    }
}
