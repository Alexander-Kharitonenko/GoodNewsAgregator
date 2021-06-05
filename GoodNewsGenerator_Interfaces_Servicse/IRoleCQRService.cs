using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface IRoleCQRService
    {
        Task Create(string NameRole);
        Task<IEnumerable<UserModelDTO>> GetAllUserWithRole();
        Task<RoleModelDTO> GetRoleById(Guid id);
        Task RemoveRole(Guid id);

    }
}
