using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGeneratorAPI.Models.JwtModel
{
    public class JwtOptions
    {
        public string Issuer { get; set; } //издатель
        public string Audience { get; set; }//слушатель
        public string Key { get; set; } // ключь шефрования
        public int TokenLifeTime { get; set; } // время жизни токена в секундах

        public SymmetricSecurityKey GetSymmetricSecurityKey() // метод семетричного ифрования который будет шифовать наш секретный ключь для токена
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
