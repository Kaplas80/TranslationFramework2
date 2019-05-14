using System;
using System.IO;

namespace ParTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var isFile = File.Exists(args[0]);
            if (isFile)
            {
                ParFile.Extract(args[0], $"{args[0]}.unpack");
                return;
            }

            var isDirectory = Directory.Exists(args[0]);

            if (isDirectory)
            {
                if (args[0].EndsWith(".unpack"))
                {
                    var filename = args[0].Replace(".unpack", string.Empty);

                    ParFile.Repack(args[0], filename, true);
                    return;
                }
                else
                {
                    var parFiles = Directory.GetFiles(args[0], "*.par", SearchOption.AllDirectories);

                    foreach (var file in parFiles)
                    {
                        ParFile.Extract(file, $"{file}.unpack");
                    }
                }
            }
        }
    }
}
