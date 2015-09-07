using System;
using System.Net;
using System.Threading;
using NLog.Targets.Lumberjack;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Logstash;

namespace NLog.Targets.Lumberjack.TestConsole
{
    class Program
    {
        private static readonly NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();



        static Random rnd = new Random();

        private static void Main(string[] args)
        {
            //sending metric
            var message = new LogstashMetricMessage("yourid", "backend", "vp", "auth", UnixTimeNow(), new Random().Next(50, 100))
            {
                MachineName = Environment.MachineName
            };
            nlog.Measure(message);


            // sending log
            var log = new LogstashMessage("yourid", "backend", "vp", LogLevel.Info, "My info message")
            {
                Tags = new HashSet<string> { "tag01", "tag02", "tag03" },
                Fields = new Dictionary<string, object> {
                        { "mem", "256"},
                        { "load", 0.3},
                    },
                MachineName = Environment.MachineName
            };
            nlog.Log(log);

            //sending alert
            var alert = new LogstashAlertMessage("yourid", "backend", "vp", "myrule", "Event raised!")
            {
                MachineName = Environment.MachineName
            };
            nlog.Alert(alert);

            Thread.Sleep(TimeSpan.FromSeconds(2000));
        }

        private static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        private static void Test01()
        {
            IList<LogLevel> logs =
                Enumerable.Range(0, 1000).Select(i => LogLevel.Trace)
                    .Concat(Enumerable.Range(0, 500).Select(i => LogLevel.Debug))
                    .Concat(Enumerable.Range(0, 300).Select(i => LogLevel.Info))
                    .Concat(Enumerable.Range(0, 100).Select(i => LogLevel.Warn))
                    .Concat(Enumerable.Range(0, 50).Select(i => LogLevel.Error))
                    .Concat(Enumerable.Range(0, 30).Select(i => LogLevel.Error))
                    .ToList();

            var apps = new[] { "vp", "db", "ca", "sms", "email" };
            var modules = new[] { "auth", "parse", "fetch", "load", "upload", "ocr" };

            var data = messages.Concat(text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(i => i.Trim()).ToArray();

            for (;;)
            {
                for (int i = 0; i < modules.Length; i++)
                {
                    for (int j = 0; j < apps.Length; j++)
                    {
                        for (int k = 0; k < logs.Count; k++)
                        {
                            var module = modules[rnd.Next(0, modules.Length)];
                            var app = apps[rnd.Next(0, apps.Length)];
                            var lvl = logs[rnd.Next(0, logs.Count)];

                            nlog.Setup("yourid", app, module)
                                .Log(lvl, data[rnd.Next(0, data.Length)])
                                .WithTags(Guid.NewGuid())
                                .WithTags(app + module, Guid.NewGuid().ToString("N"))
                                .WithField("timeout", 0.3)
                                .WithFields(new Dictionary<string, object>
                                {
                                    {"cpu", rnd.NextDouble()},
                                    {"mem", rnd.Next(256, 1025)},
                                    {"text", data[rnd.Next(0, data.Length)]},
                                    {"pc", Environment.MachineName},
                                    {"ams", "NLog.LumberjackTarget"},
                                    {"tmp", Path.GetTempPath()},
                                    {"os", Environment.OSVersion.ToString()},
                                    {"cpucnt", Environment.ProcessorCount},
                                    {"domain", Environment.UserDomainName},
                                    {"version", Environment.Version},
                                    {"stacktrace", Environment.StackTrace},
                                })
                                //.Alert("myrule", "event raised!")
                                //.Measure("auth", 100)
                                .Commit();
                        }
                        Thread.Sleep(50000);
                    }
                    Thread.Sleep(10000);
                }
            }
        }

        #region sentences
        static string[] messages = new[] {
            @"Historically, the world of data and the world of objects " ,
        @"have not been well integrated. Programmers work in C# or Visual Basic " ,
        @"and also in SQL or XQuery. On the one side are concepts such as classes, " ,
        @"objects, fields,<br/> inheritance, and .NET Framework APIs. On the other side " ,
        @"are tables, columns, rows, nodes, and separate languages for dealing with " ,
        @"them. Data types often require translation between the two worlds; there are " ,
        @"different standard functions. Because the object world has no notion of query, a " ,
        @"query can only be represented as a string without compile-time type checking or " ,
        @"IntelliSense support in the IDE. Transferring data from SQL tables or XML trees to " ,
        @"objects in memory is often tedious and error-prone.",
        @"Exception in lumberjack input thread",
           @"Can we go to the park.",
        @"Where is the orange cat? Said the big black dog.",
        @"We can make the bird fly away if we jump on something.",
        @"We can go <small>down to the store with the dog. It is not too far</small> away.",
        @"My big yellow cat ate the little black bird.",
        @"I like to read my book at school.",
        @"We are going to swim at the park.",
        @"A string method, Split() separates at string and character delimiters. Even if we want just one part from a string, Split is useful. It returns a string array.",
        @"We call Split on a string instance. This program splits on a single character. The array returned has four elements.",
        @"The foreach-loop loops over this array and displays each word. The string array can be used as any other.",
        @"The input string, which contains four words, is split on spaces. The result value from Split is a string array",
        @"Next we use Regex.Split to separate based on multiple characters",
        @"There is an overloaded method if you need StringSplitOptions",
        @"Regex methods are used to effectively Split strings. But string Split is often faster. This example specifies an array as the first argument to Split",
        @"Good, comprehensive sets of these are a bit of a slog to find online, so I cobbled together some from posts on the Internets, GitHub Gists and some tweaks of my own to make them work",
    };

        static string text = @"There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.
Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.
But I must explain to you how all this mistaken idea of denouncing of a pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure.
Logstash, part of the ELK-Stack, is a tool to collect log files from various sources, parse them into a JSON format and put them into one or more databases, index engines and so forth - often elasticsearch. In the simplest case you can slurp log files from the filesystem, parse them using grok - a collection of named regular expressions - and put them into the integrated elastic search engine with a simple web frontend to search them. In my experience the hardest part is to get the regular expressions for parsing the log files right. The Grok debugger can help you test your regular expressions and provides Grok Discovery that sometimes can suggest regular expressions. This site, GrokConstructor, goes beyond that by providing an incremental construction process that helps you to construct a regular expression that matches all of a set of given log lines, and provides you a matcher where you can simultaneously try out your regular expression on several log lines. You can find the source on GitHub. If you are not comfortable with running this on a public platform you can also run it locally / deploy it as a WAR somewhere.
This is done just for fun and you don't pay me for this, thus you get absolutely and utterly and completely no warranties of any kind to the extend permitted by law. Even if the program jumps out of your screen and chews at your leg.
All services are session-less. Thus, you can open arbitrarily many windows simultaneously without conflicts.
While logstash ships with many patterns, you eventually will need to write a custom pattern for your application’s logs.  The general strategy is to start slowly, working your way from the left of the input string, parsing one field at a time.
Vardenafil's indications and contra-indications are the same as with other PDE5 inhibitors; it is closely related in function to sildenafil citrate (Viagra) and tadalafil (Cialis). The difference between the vardenafil molecule and sildenafil citrate is a nitrogen atom's position and the change of sildenafil's piperazine ring methyl group to an ethyl group. Tadalafil is structurally different from both sildenafil and vardenafil. Vardenafil's relatively short effective time is comparable to but somewhat longer than sildenafil's.
In the course of migrating thousands of texts from Etext to VIRGO, we determined that certain resources were not eligible for inclusion, most often due to copyright issues.  Many of the texts that were not migrated can be found among other university online text collections, Google Books, HathiTrust and Project Gutenberg.  We regret any inconvenience this may cause you and we wish you the best with your research.  Some pages from the Etext center have been preserved at the Internet Archive.
Sadler, a senior at California State University in Sacramento, was on his first trip to Europe when terror struck.
 In their instruments of accession, such organizations shall declare the extent of their competence with respect to the matters governed by the Convention. These organizations shall also inform the Depositary Government of any substantial modification in the extent of their competence. Notifications by regional economic integration organizations concerning their competence with respect to matters governed by this Convention and modifications thereto shall be distributed to the Parties by the Depositary Government.
When his friends jumped the gunman and took him down, he and another passenger helped restrain him and ensure he stayed down.
For each State which ratifies, accepts or approves the present Convention or accedes thereto after the deposit of the tenth instrument of ratification, acceptance, approval or accession, the present Convention shall enter into force 90 days after the deposit by such State of its instrument of ratification, acceptance, approval or accession.
";

        #endregion
    }
}
