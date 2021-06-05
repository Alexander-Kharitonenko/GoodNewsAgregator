using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_Models_For_GoodNewsGenerator;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface ISourceCQRService
    {
  

        Task<SourceModelDTO> GetSourceById(Guid id);

        Task<IEnumerable<SourceModelDTO>> GetAllSource();

        Task Add(string url);

        Task Delete(Guid id);


    }
}
