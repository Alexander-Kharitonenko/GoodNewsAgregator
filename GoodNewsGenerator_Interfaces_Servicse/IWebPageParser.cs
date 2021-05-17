using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface IWebPageParser
    {
        
            Task<(string content, string Img)> Parse(string url);
        
    }
}
