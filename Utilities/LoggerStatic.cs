using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Utilities
{
    public static class LoggerStatic
    {
        public static Logger Logger = LogManager.GetLogger("AllLog");

        static LoggerStatic()
        {
            Logger.SetProperty("machineName", Environment.MachineName);
        }

    }
}
