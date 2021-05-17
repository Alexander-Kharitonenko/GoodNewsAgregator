using GoodNewsGenerator.Services.Paginator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsGenerator.Models.ViewModel.News;

namespace GoodNewsGenerator.TegHelper
{
    public class PaginatorTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory { get; set; } //формирует url запросы для отображения в представлении 
        public PageInfo pageInfo { get; set; } // хранит в себе всю информацию о погинации

        public PaginatorTagHelper(IUrlHelperFactory urlHelper)
        {
            urlHelperFactory = urlHelper;
        }
        
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext viewContext { get; set; } // класс отвечающий за работу с контекстом представления 
        

        public string PageAction { get; set; } //будет хранить в себе имя метода контроллера

        public override void Process(TagHelperContext context, TagHelperOutput output) // метод выполнения тег хелпера
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(viewContext); // формируем url в тег хелпере 

            output.TagName = "div"; //<div>

            TagBuilder tag = new TagBuilder("ul");// <lu>

            tag.AddCssClass("pagination"); // <lu class="pagination">

            // формируем три ссылки - на текущую, предыдущую и следующую
            TagBuilder currentItem = CreateTag(pageInfo.NumberPage, urlHelper);

            // создаем ссылку на предыдущую страницу, если она есть
            if (pageInfo.CanGetPreviousPage)
            {
                TagBuilder prevItem = CreateTag(pageInfo.NumberPage - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            tag.InnerHtml.AppendHtml(currentItem);

            // создаем ссылку на следующую страницу, если она есть
            if (pageInfo.CanGetNextPage)
            {
                TagBuilder nextItem = CreateTag(pageInfo.NumberPage + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);

            }
            output.Content.AppendHtml(tag);

            /*
             * <div>
             * <lu class="pagination">
             *
             * </lu>
             * </div>
             */

        }

        TagBuilder CreateTag(int NumberPage, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li"); //<li>
            TagBuilder link = new TagBuilder("a");  //<a>
            if (NumberPage == this.pageInfo.NumberPage) // если переданная страница в метод равна странице pageInfo.NumberPage 
            {
                item.AddCssClass("active"); //то к тегу li примняется css  класс active <li class ="active">
            }
            else
            {
                for(int i = 1; i < pageInfo.TotalNumberPage; i++)
                link.Attributes["href"] = urlHelper.Action(PageAction, new { page = NumberPage}); // иначе формируем в атрибуте <a asp-action="Имя метода" asp-route-page= "NumberPage"></a>
             
            }

            item.AddCssClass("page-item");//добавляем к <li> class="page-item" = <li class="page-item"></li>
            link.AddCssClass("page-link");//добавляем к <a> class="page-link" = <a class="page-link""></a>
            link.InnerHtml.Append(NumberPage.ToString()); //<a>1</a>
            item.InnerHtml.AppendHtml(link);//<a asp-action="Имя метода" asp-route-page="NumberPage"></a> 
            return item;

            /* 
             * <li class="page-item">
             *     <a class="page-link" asp-action="Имя метода" asp-route-page="NumberPage"></a>
             * </li>
             */


        }

    }
}
