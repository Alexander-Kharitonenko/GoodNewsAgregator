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
    public  interface IUserService
    {
        IEnumerable<UserModelDTO> GetAllUser();

        UserModelDTO GetUserBy(string Email);

        Task СreateUser(UserModelDTO user);

        Task RemoveUser(Guid id);

        Task AdminInitialization();

        string EncryptionPassword(string password);



    }
}
