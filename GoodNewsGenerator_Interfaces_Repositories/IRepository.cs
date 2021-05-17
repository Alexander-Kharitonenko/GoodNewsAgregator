using EntityGeneratorNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Repositories
{
    public interface IRepository<T> where T : class, IBaseEntity // создаём жденерик интерфейс репозитория куда в качестве Т будут передоваться сущности с к которым будет обращаться репозиторий 
    {

        //базовая логика работы репозитория с бд

        Task Add(T entity); // добавляем элемент в таблицу 
        Task AddRange(IEnumerable<T> entity); // добавляем множество элементов в таблицу

        IQueryable<T> Get();



        IQueryable<T> GetById(Guid id); //получение объекта из таблицы T по его Id
        IQueryable<T> GetAllEntity(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] include); // получение всех объектов из таблицы T

        Task Update(T entity);//обновление модели

        Task Remove(Guid id);//удаление одной записи
        Task RemoveRange(IEnumerable<T> entity);//удаление множества записей  

    }
}
