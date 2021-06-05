using AutoMapper;
using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using GoodNewsGenerator_Implementation_Repositories;
using GoodNewsGenerator_Interfaces_Servicse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Services
{
    public  class SourceService : ISourceService
    {
        public IUnitOfWork _unitOfWork;
        private IMapper Mapper;

        public SourceService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            Mapper = mapper;
        }

        public void Add(SourceModelDTO entity)
        {
            Source source = Mapper.Map<Source>(entity);

            List<SourceModelDTO> sourse = GetAllSource().ToList();

            if (sourse.Any(el => el.SourseURL.Equals(entity.SourseURL)))
            {
                throw new Exception("Такой ресурс уже существует"); 
            }
            else
            {
                _unitOfWork.Source.Add(source);
                _unitOfWork.SaveChangesAsync();
            }
        }

        public void AddRange(IEnumerable<SourceModelDTO> entity)
        {
            List<Source> source = entity.Select(el=> Mapper.Map<Source>(el)).ToList();
            _unitOfWork.Source.AddRange(source);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletSourceById(Guid id)
        {          
            await _unitOfWork.Source.Remove(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<SourceModelDTO> GetAllSource()
        {  
            IQueryable<Source> surce = _unitOfWork.Source.GetAllEntity(el => el.SourseURL != null);
            List<SourceModelDTO> source = surce.Select(el => Mapper.Map<SourceModelDTO>(el)).ToList();
            return source;
        }

        public SourceModelDTO GetSourceById(Guid id)
        {
            List<Source> source = _unitOfWork.Source.GetById(id).ToList();
            SourceModelDTO source1 = Mapper.Map<SourceModelDTO>(source.FirstOrDefault());
            return source1;
        }
    }
}
