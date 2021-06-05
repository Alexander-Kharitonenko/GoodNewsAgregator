using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_Models_For_GoodNewsGenerator;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface ISourceService
    {
        public void Add(SourceModelDTO entity);
        public void AddRange(IEnumerable<SourceModelDTO> entity);

        SourceModelDTO GetSourceById(Guid id);

        IEnumerable<SourceModelDTO> GetAllSource();

       Task DeletSourceById(Guid id);

    }
}
