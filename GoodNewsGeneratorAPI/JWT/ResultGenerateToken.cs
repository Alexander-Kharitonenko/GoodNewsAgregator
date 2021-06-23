using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.JWT
{
    public class ResultGenerateToken
    {
        public string Toket { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
