using DTO_Models_For_GoodNewsGenerator;
using EntityGeneratorNews.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public  interface IUserCQRService
    {


        Task<UserModelDTO> GetUserBy(string Email);
        Task<string> GetUserEmailByTokenKey(string Key);

        Task СreateUser(UserModelDTO user);

        Task AdminInitialization();

        string EncryptionPassword(string password);



    }
}
