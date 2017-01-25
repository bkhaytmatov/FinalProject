using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSD.Services
{
    public abstract class CommandManager
    {
        protected abstract DbCommand GetCommand(string sql);
    }
}
