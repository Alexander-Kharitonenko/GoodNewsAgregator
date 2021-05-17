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
    public class TutByParser : IWebPageParser
    {
        public async Task<(string content, string Img)> Parse(string url)
        {

            (string conTent, string IMg) result = (null, null);

            try
            {
                string Img;

                Regex regex = new Regex(@"<[^>]*>");

                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(url);
                HtmlNode Content = htmlDoc.DocumentNode.SelectSingleNode("//body//div[@id='article_body']//p//strong");

                if (Content == null)
                {
                    Content = htmlDoc.DocumentNode.SelectSingleNode("//body//div[@id='article_body']//p");
                }

                Img = htmlDoc.DocumentNode.SelectSingleNode("//body//div[@id='article_body']//figure//img")?.GetAttributeValue("src", @"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRTrIUAHP1tTymrh_-Jn6ivvaKWkSLI8T57ulghIN86FWs58z7MmjCuXDlTIXZoUbZnprA&usqp=CAU");

                if (Img == null)
                {
                    Img = @"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRTrIUAHP1tTymrh_-Jn6ivvaKWkSLI8T57ulghIN86FWs58z7MmjCuXDlTIXZoUbZnprA&usqp=CAU";
                }
                string content = regex.Replace(Content.InnerHtml, String.Empty).Replace(@"&nbsp;", " ").Replace("&mdash;", " ").Replace("&laquo;", " ").Replace("&raquo;", " ").Replace("&hellip;", " ").Replace("&thinsp;", "");
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
