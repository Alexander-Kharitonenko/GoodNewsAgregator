using System;

namespace EntityGeneratorNews.Data
{
    public interface IBaseEntity // интефейс который наследуют все сущности базы данных для приведения 
    {
        Guid Id { get; set; }
    }
}
