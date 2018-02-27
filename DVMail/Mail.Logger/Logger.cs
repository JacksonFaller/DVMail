using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Mail.Logger
{
    public class Logger
    {
        public static NLog.Logger Instance = LogManager.GetCurrentClassLogger();
    }
}
