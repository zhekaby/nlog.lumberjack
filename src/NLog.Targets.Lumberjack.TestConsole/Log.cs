using Autofac;
using NLog.Targets.Lumberjack.Builders;
using NLog.Targets.Lumberjack.Loggers;


namespace NLog.Targets.Lumberjack.TestConsole
{
    public static class Log
    {
        private static readonly IContainer container;
        static Log()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(x => LogManager.GetCurrentClassLogger()).As<ILogger>();
            containerBuilder.RegisterType<NlogLogger>().As<IMessageLogger>();
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