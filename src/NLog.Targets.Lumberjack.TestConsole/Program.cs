using System;
using System.Net;
using System.Threading;

namespace NLog.Targets.Lumberjack.TestConsole
{
    class Program
    {
        //private static readonly NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            for (;;)
            {
                //sending metric
                //var message = new LumberjackMetricMessage("yourid", "backend", "vp", "auth", UnixTimeNow(), new Random().Next(50, 100))
                //{
                //    MachineName = Environment.MachineName
                //};
                //nlog.Measure(message);

                Log.Metric()
                   .Name("auth")
                   .Value(new Random().Next(50, 100))
                   .Commit();

                // sending log
<<<<<<< HEAD
                //var log = new LumberjackLogMessage("yourid", "backend", "vp", LogLevel.Info, "My info message")
                //{
                //    Tags = new HashSet<string> { "tag01", "tag02", "tag03" },
                //    Fields = new Dictionary<string, object> {
                //        { "mem", "256"},
                //        { "load", 0.3},
                //    }
                //};
                //nlog.Log(log);

                Log.Message()
                   .Tags("tag01", "tag02", "tag03")
                   .Level(LogLevel.Info)
                   .Field("mem", "256").Field("load", 0.3)
                   .Message("My info message")
                   .Commit();

                Log.Alert()
                .Rule("myrule")
                .Text("Event raised!")
                .Commit();

=======
                var log = new LumberjackLogMessage("yourid", "backend", "vp", LogLevel.Info, "My info message")
                {
                    Tags = new HashSet<string> { "tag01", "tag02", "tag03" },
                    Fields = new Dictionary<string, object> {
                        { "mem", "256"},
                        { "load", 0.3},
                    }
                };
                nlog.Log(log);
>>>>>>> ca4965f2fa6792b18c3e51094b5fa50ad9ace14e
                Thread.Sleep(10);
            }

            // sending alert
            //var alert = new LumberjackAlertMessage("yourid", "backend", "vp", "myrule", "Event raised!");
            //nlog.Alert(alert);
<<<<<<< HEAD
            Thread.Sleep(TimeSpan.FromSeconds(2000));
=======

            //Thread.Sleep(TimeSpan.FromSeconds(2000));
>>>>>>> ca4965f2fa6792b18c3e51094b5fa50ad9ace14e
        }
    }
}
