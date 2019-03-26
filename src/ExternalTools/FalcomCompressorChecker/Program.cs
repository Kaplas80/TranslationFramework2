using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using TFGame.TrailsSky;

namespace FalcomCompressorChecker
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, "input", "Fichero a comprimir")]
        [Required]
        public string Input { get; }

        private void OnExecute()
        {
            if (string.IsNullOrEmpty(Input) || !System.IO.File.Exists(Input))
            {
                Console.WriteLine($"ERROR: No existe el fichero \"{Input}\"");
                Exit();
            }

            var uncompressedData = File.ReadAllBytes(Input);
            var data = FalcomCompressor.Compress(uncompressedData);
            File.WriteAllBytes(@"C:\Users\Kaplas\Desktop\kk\export\test.bin", data);
        }

        private static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Pulse una tecla para salir...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
