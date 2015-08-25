using System;
using System.Net;
using System.Threading;
using NLog.Targets.Lumberjack;
using System.Collections.Generic;

namespace NLog.Targets.Lumberjack.TestConsole
{
    class Program
    {
        private static readonly NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            for (;;)
            {
                nlog.Setup("yourid", "vp", "auth")

                    .Log(LogLevel.Debug, "Debug message")
                    .WithTags("tag1")
                    .WithTags("tag2", "tag3", "tag10")
                    .WithField("timeout", 0.3)
                    .WithFields(new Dictionary<string, object> {
                        { "cpu", 0.5},
                        { "mem", 512}
                    })
                    .Alert("myrule", "event raised!")
                    .Measure("auth", 100)
                    .Commit();

                Thread.Sleep(1000);
            }
        }
    }
}
