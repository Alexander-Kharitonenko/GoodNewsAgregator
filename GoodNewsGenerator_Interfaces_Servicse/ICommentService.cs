using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentModelDTO>> GetAllComents(Guid id);
        Task AddComents(CommentModelDTO comment);
    }
}
