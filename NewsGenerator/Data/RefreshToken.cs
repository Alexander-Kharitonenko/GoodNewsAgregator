using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGeneratorNews.Data
{
    public class RefreshToken : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateTimeToken { get; set; }
        public DateTime ExpiresToken { get; set; }

        [Required]
        public string Key { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }


    }
}
