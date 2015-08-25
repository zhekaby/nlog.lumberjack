using Autofac;
using NLog.Targets.Lumberjack.Builders;
using NLog.Targets.Lumberjack.Loggers;
using NLog.Targets.Lumberjack.Settings;

namespace NLog.Targets.Lumberjack.TestConsole
{
    public static class Log
    {
        private static readonly IContainer container;
        static Log()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(x => LogManager.GetCurrentClassLogger()).As<Logger>();
            containerBuilder.RegisterType<Logger>().As<ILogger>();
            containerBuilder.RegisterType<ConsoleLogger>().As<IMessageLogger>();
            containerBuilder.RegisterType<ComponentSettings>().As<IComponentSettings>();
            containerBuilder.RegisterType<LumberjackMetricMessageBuilder>();
            containerBuilder.RegisterType<LumberjackAlertMessageBuilder>();
            containerBuilder.RegisterType<LumberjackLogMessageBuilder>();
            container = containerBuilder.Build();
        }

        public static LumberjackLogMessageBuilder Message()
        {
            return container.Resolve<LumberjackLogMessageBuilder>();
        }

        public static LumberjackMetricMessageBuilder Metric()
        {
            return container.Resolve<LumberjackMetricMessageBuilder>();
        }

        public static LumberjackAlertMessageBuilder Alert()
        {
            return container.Resolve<LumberjackAlertMessageBuilder>();
        }
    }
}