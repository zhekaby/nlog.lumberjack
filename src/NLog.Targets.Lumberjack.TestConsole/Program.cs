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
                //Log.Metric()
                //   .Name("auth")
                //   .Value(new Random().Next(50, 100))
                //   .Commit();

                Log.Message()
                   .Tags("tag01", "tag02", "tag03")
                   .Level(LogLevel.Info)
                   .Field("mem", "256").Field("load", 0.3)
                   .Message("My info message " + Guid.NewGuid())
                   .Commit();

                //Log.Alert()
                //.Rule("myrule")
                //.Text("Event raised!")
                //.Commit();

                Thread.Sleep(10);
            }

            // sending alert
            //var alert = new LumberjackAlertMessage("yourid", "backend", "vp", "myrule", "Event raised!");
            //nlog.Alert(alert);
            Thread.Sleep(TimeSpan.FromSeconds(2000));
        }
    }
}
