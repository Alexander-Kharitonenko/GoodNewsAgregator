using AutoMapper;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Services
{
    public class CommentsService : ICommentService
    {
        private readonly IUnitOfWork DbContext;
        private IMapper Mapper;

        public CommentsService(IUnitOfWork dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        public async Task<IEnumerable<CommentModelDTO>> GetAllComents(Guid id) 
        {
            List<Comment> CommentList = await DbContext.Comment.GetAllEntity(el=>el.NewsId == id).Include(el => el.Users).ToListAsync();
            var CommetnsWithUser = CommentList.Select(el => Mapper.Map<CommentModelDTO>(el));
            return CommetnsWithUser;
        }

        public async Task AddComents(CommentModelDTO comment)
        {
            if (comment != null)
            {
               await DbContext.Comment.Add(Mapper.Map<Comment>(comment));
               await DbContext.SaveChangesAsync();
            }
           
        }
    }
}
