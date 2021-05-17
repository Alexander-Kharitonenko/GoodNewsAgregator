using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsGenerator_Interfaces_Servicse
{
    public interface IRoleService
    {
        Task Create(string NameRole);
        Task<IEnumerable<RoleModelDTO>> GetAllUserWithRole();
        RoleModelDTO GetRoleById(Guid id);
        Task RemoveRole(Guid id);

    }
}
