using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils.Models.BO
{
    /// <summary>
    /// 登录账户信息
    /// </summary>
    public class LoginUserBO
    {
        public object UserID { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string OrgnizationId { get; set; }
        public string RoleId { get; set; }
        public bool SupperAdmin { get; set; } = false;
        public bool AdminOrg { get; set; } = false;
    }
}
