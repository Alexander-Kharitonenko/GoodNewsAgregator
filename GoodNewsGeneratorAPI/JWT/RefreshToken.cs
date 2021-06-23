using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.JWT
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime LlfeTime { get; set; }
    }
}
