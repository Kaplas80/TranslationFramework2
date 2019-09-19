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

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private void OnExecute()
        {
            ConfigureNLog();

            var sw = new Stopwatch();

            var isFile = File.Exists(Input);
            if (isFile)
            {
                _logger.Info(Recursive ? "MODE: EXTRACT (Recursive)" : "MODE: EXTRACT");
                _logger.Info("INPUT: {0}", Input);

                sw.Start();
                var parFile = ParFile.ReadPar(Input);
                parFile.Extract($"{Input}.unpack", Recursive);
                sw.Stop();

                _logger.Info("Time elapsed: {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            var isDirectory = Directory.Exists(Input);

            if (isDirectory)
            {
                _logger.Info(Recursive ? "MODE: REPACK (Recursive)" : "MODE: REPACK");
                _logger.Info("INPUT: {0}", Input);

                var compression = CompressionType.Default;
                
                if (ForceCompression.hasValue)
                {
                    compression = (CompressionType) (ForceCompression.value + 1);
                }

                _logger.Info("USE COMPRESSION: {0}", compression);
                _logger.Info("LOW MEMORY USAGE: {0}", LowMemory);

                var filename = Input.Replace(".unpack", string.Empty);

                sw.Start();
                var parFile = ParFile.ReadFolder(Input);
                parFile.Save(Input, $"{filename}2", compression, Recursive, LowMemory);
                
                sw.Stop();

                _logger.Info("Time elapsed: {0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            _logger.Error("{0} not found", Input);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void ConfigureNLog()
        {
            var nLogConfig = new NLog.Config.LoggingConfiguration();

            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            logconsole.Layout = Layout.FromString("${message}");
            nLogConfig.AddRule(Verbose ? NLog.LogLevel.Debug : NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);

            NLog.LogManager.Configuration = nLogConfig;
        }
    }
}
