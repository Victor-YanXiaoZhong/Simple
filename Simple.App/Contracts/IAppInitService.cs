using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Utils.Models;

namespace Simple.Contracts
{
    public interface IAppInitService
    {
        ApiResult SysDbInit();
    }
}
