using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityGeneratorNews.Data;
using GoodNewsGenerator.Models.Data;
using GoodNewsGenerator_Interfaces_Repositories;
using Microsoft.EntityFrameworkCore;


namespace GoodNewsGenerator_Implementation_Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {

        protected readonly DbContextNewsGenerator DbContext;
        protected readonly DbSet<T> Table;

        protected Repository(DbContextNewsGenerator dbContext)
        {
            DbContext = dbContext;
            Table = DbContext.Set<T>(); // возврадаем из контекста таблицу типа T и присваеваем её переменной Table

        }

        public async Task Add(T entity)
        {
      
                await Table.AddAsync(entity);
                
       
        }

        public async Task AddRange(IEnumerable<T> entity)
        {
            await Table.AddRangeAsync(entity);
        }

        public IQueryable<T> GetAllEntity(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] include)
        {
            
            IQueryable<T> result = Table.Where(predicate); // predicate это лямда условия которое передаёётся методу Where на основе условия метод найдёт T объекты и если они совпалает с условием вернёт переменнуб типа bool (мы получаем все объекты в переменнну result согласно условию predicate)
            if (include.Any()) // если передан хоть один include
            result = include.Aggregate(result, (current, include) => current.Include(include)); // мы совераем над элеметом include совершаем операцию которая подключит все объекты include в объекте result после этого result будет хранить все объекты include всебе вместе со своими данными

            return result; 
        }
    
        public IQueryable<T> GetById(Guid id)
        {
            return GetAllEntity(entity => entity.Id.Equals(id));
        }

        public IQueryable<T> Get()
        {
            return Table;
        }

        public async Task Remove(Guid id)
        {
             T result = await Table.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
             Table.Remove(result);
          

        }

        public async Task RemoveRange(IEnumerable<T> entity)
        {
           Table.RemoveRange(entity);   
        }

        public async Task Update(T entity)
        {
            T Entitu = GetById(entity.Id).FirstOrDefault();
            Entitu = entity;
            Table.Update(Entitu);
            

        }
    }
}
