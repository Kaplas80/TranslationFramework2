using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using NLog.Layouts;
using ParLib;

namespace ParTool
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "input")]
        [Required]
        public string Input { get; }

        [Option("-fc|--force-compression")]
        public (bool hasValue, int value) ForceCompression { get; set; }

        [Option("-r|--recursive")]
        public bool Recursive { get; }

        [Option("-lm|--low-memory")]
        public bool LowMemory { get; }

        [Option("-v|--verbose")]
        public bool Verbose { get; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private void OnExecute()
        {
            ConfigureNLog();

            var sw = new Stopwatch();

            var isFile = File.Exists(Input);
            if (isFile)
            {
                Logger.Info(Recursive ? "MODE: EXTRACT (Recursive)" : "MODE: EXTRACT");
                Logger.Info("INPUT: {0}", Input);

                sw.Start();
                var parFile = ParFile.ReadPar(Input);
                parFile.Extract($"{Input}.unpack", Recursive);
                sw.Stop();

                Logger.Info("Time elapsed: {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            var isDirectory = Directory.Exists(Input);

            if (isDirectory)
            {
                Logger.Info(Recursive ? "MODE: REPACK (Recursive)" : "MODE: REPACK");
                Logger.Info("INPUT: {0}", Input);

                var compression = CompressionType.Default;
                
                if (ForceCompression.hasValue)
                {
                    compression = (CompressionType) (ForceCompression.value + 1);
                }

                Logger.Info("USE COMPRESSION: {0}", compression);
                Logger.Info("LOW MEMORY USAGE: {0}", LowMemory);

                var filename = Input.EndsWith(".unpack")
                    ? Input.Substring(0, Input.Length - 7)
                    : string.Concat(Input, ".par");

                sw.Start();
                var parFile = ParFile.ReadFolder(Input);
                parFile.Save(Input, $"{filename}2", compression, Recursive, LowMemory);
                
                sw.Stop();

                Logger.Info("Time elapsed: {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Logger.Error("{0} not found", Input);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void ConfigureNLog()
        {
            var nLogConfig = new NLog.Config.LoggingConfiguration();

            var logConsole = new NLog.Targets.ConsoleTarget("logconsole") {Layout = Layout.FromString("${message}")};
            nLogConfig.AddRule(Verbose ? NLog.LogLevel.Debug : NLog.LogLevel.Info, NLog.LogLevel.Fatal, logConsole);

            NLog.LogManager.Configuration = nLogConfig;
        }
    }
}
