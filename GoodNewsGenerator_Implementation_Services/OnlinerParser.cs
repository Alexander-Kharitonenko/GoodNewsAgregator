using GoodNewsGenerator_Interfaces_Servicse;
using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Implementation_Services
{
    public class OnlinerParser : IWebPageParser
    {
        public async Task<(string content, string Img)> Parse(string url)
        {
            (string conTent, string IMg) result = (null, null);

            try
            {
                string Img;
                string content;

                Regex regex = new Regex(@"<[^>]*>");

                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(url);
                HtmlNode Content = htmlDoc.DocumentNode.SelectSingleNode("//body//div[@class='news-text']//p");

                if (Content == null)
                {
                    Content = htmlDoc.DocumentNode.SelectSingleNode("//body//div[@class='news-grid__part news-grid__part_1']//div[@class='news-text']//p");
                }
                content = regex.Replace(Content.InnerHtml, String.Empty).Replace(@"&nbsp;", " ").Replace("&mdash;", " ").Replace("&laquo;", " ").Replace("&raquo;", " ").Replace("&hellip;", " ").Replace("&thinsp;", "");

                string css = htmlDoc.DocumentNode.SelectSingleNode("//body//div[@class='news-header__image']").GetAttributeValue("style", null);

                if (css == null)
                {
                    Img = @"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRTrIUAHP1tTymrh_-Jn6ivvaKWkSLI8T57ulghIN86FWs58z7MmjCuXDlTIXZoUbZnprA&usqp=CAU";
                }

                Img = $@"{css.Replace("background-image: url('", "").Replace("');", "")}";
                result = (content, Img);
            }
            catch (Exception e) 
            {
                Log.Warning(e, $"Не удалось распарсить страницу{url} - {e.Message} - {e.StackTrace}");
            }
            return result;
        }
    }
}
