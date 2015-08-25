using NLog.Targets.Lumberjack.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Targets.Lumberjack.TestConsole
{
    public class ComponentSettings : IComponentSettings
    {
        public string ComponentName { get { return "vp"; } }
        public string ApplicationId { get { return "backend"; } }
        public string Source { get { return "yourid"; } }
        public string MachineName { get { return Environment.MachineName; } }
    }
}
