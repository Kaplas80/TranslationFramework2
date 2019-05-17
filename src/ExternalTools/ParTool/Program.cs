using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using NLog.Layouts;

namespace ParTool
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "input")]
        [Required]
        public string Input { get; }

        [Option("-nc|--no-compression")]
        public bool NotUseCompression { get; }

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private void OnExecute()
        {
            ConfigureNLog();

            var useCompression = !NotUseCompression;

            var isFile = File.Exists(Input);
            if (isFile)
            {
                _logger.Info("MODE: EXTRACT");
                _logger.Info("INPUT: {0}", Input);

                ParFile.Extract(Input, $"{Input}.unpack");

                _logger.Info("FINISHED");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            var isDirectory = Directory.Exists(Input);

            if (isDirectory)
            {
                var extractDataPath = Path.Combine(Input, "Extract_Data.tf");
                if (File.Exists(extractDataPath))
                {
                    _logger.Info("MODE: REPACK");
                    _logger.Info("INPUT: {0}", Input);
                    _logger.Info("USE COMPRESSION: {0}", useCompression);

                    var filename = Input.Replace(".unpack", string.Empty);

                    ParFile.Repack(Input, filename, useCompression);

                    _logger.Info("FINISHED");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    return;
                }

                _logger.Error("{0} not found", extractDataPath);

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
                /*else
                {
                    var parFiles = Directory.GetFiles(Input, "*.par", SearchOption.AllDirectories);

                    foreach (var file in parFiles)
                    {
                        ParFile.Extract(file, $"{file}.unpack");
                    }
                }*/
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
            nLogConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);

            NLog.LogManager.Configuration = nLogConfig;
        }
    }
}
