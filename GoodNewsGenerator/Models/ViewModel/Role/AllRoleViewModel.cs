using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Models.ViewModel.Role
{
    public class AllRoleViewModel
    {
        public RoleModelDTO Role { get; set; }
        public IEnumerable<RoleModelDTO> AllRole { get; set; }
    }
}
