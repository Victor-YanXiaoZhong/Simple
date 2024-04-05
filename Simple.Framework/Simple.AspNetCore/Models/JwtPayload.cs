using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.AspNetCore.Models
{
    public class JwtPayload
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string OrgnizationId { get; set; }
        public string RoleId { get; set; }
        public bool SupperAdmin { get; set; } = false;
        public bool AdminOrg { get; set; } = false;
        public DateTime? ExpTime { get; set; }
    }
}
