using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Services.Paginator
{
    public class PageInfo
    {
        public int NumberPage { get; set; } // текущий номер страницы
        public int TotalNumberPage { get; set; } // общее количество страниц

        public PageInfo(int AmountElementsint, int numberPage, int sizePage) // устаналиваем общее количество элементов которое у нас есть,какую страницу мы хотим видеть , сколько элементов отобразиться на 1 странице
        {
            NumberPage = numberPage; //устанавливаем номер страницы который нам отобразит
            TotalNumberPage = (int)Math.Ceiling((double)AmountElementsint / sizePage); // расчитываем общее число страниц 
        }

        public bool CanGetPreviousPage { get { return NumberPage > 1;} } // проверка дошли ли до начала нумерации страниц
        public bool CanGetNextPage { get { return NumberPage < TotalNumberPage; } } // проверка дошли ли до конца нумерации страниц


    }
}
