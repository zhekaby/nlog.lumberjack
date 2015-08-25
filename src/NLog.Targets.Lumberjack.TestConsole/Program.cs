using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NLog.Targets.Lumberjack.TestConsole
{
    class Program
    {
        private static readonly NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            for (;;)
            {
                //sending metric
                var message = new LumberjackMetricMessage("yourid", "backend", "vp", "auth", UnixTimeNow(), new Random().Next(50, 100))
                {
                    MachineName = Environment.MachineName
                };
                nlog.Measure(message);


                // sending log
                var log = new LumberjackLogMessage("yourid", "backend", "vp", LogLevel.Info, "My info message")
                {
                    Tags = new HashSet<string> { "tag01", "tag02", "tag03" },
                    Fields = new Dictionary<string, object> {
                        { "mem", "256"},
                        { "load", 0.3},
                    }
                };
                nlog.Log(log);
                Thread.Sleep(10);
            }
            // sending alert
            //var alert = new LumberjackAlertMessage("yourid", "backend", "vp", "myrule", "Event raised!");
            //nlog.Alert(alert);

            //Thread.Sleep(TimeSpan.FromSeconds(2000));
        }

        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }
    }
}
