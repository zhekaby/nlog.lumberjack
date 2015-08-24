using System;

namespace NLog.Targets.Lumberjack.Settings
{
    public class ComponentSettings : IComponentSettings
    {
        public string ComponentName { get { return "vp"; } }
        public string ApplicationId  { get { return "backend"; } }
        public string Source { get { return "yourid"; } }
        public string MachineName { get { return Environment.MachineName; } }
    }
}