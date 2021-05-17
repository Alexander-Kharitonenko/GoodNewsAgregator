using DTO_Models_For_GoodNewsGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsGenerator.Models.ViewModel.Role
{
    public class RoleViewModel
    {
        
        public IEnumerable<UserModelDTO> user { get; set; }
       
    }
}
