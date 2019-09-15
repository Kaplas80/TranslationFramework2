using System;
using System.Diagnostics;
using System.IO;
using UnderRailLib;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;

namespace UnderRailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Initialize();
            Binder.SetAssemblyResolver(assemblyResolver);

            var fileManager = new FileManager();

            var files = Directory.EnumerateFiles(@"I:\Games\UnderRail\data\maps\locale\static", "*.uz", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Debug.WriteLine(file);

                Process<Zone>(file, fileManager);
            }

            files = Directory.EnumerateFiles(@"I:\Games\UnderRail\data\maps\locale\static", "*.uzl", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Debug.WriteLine(file);

                Process<ZoneLayer>(file, fileManager);
            }
            //var file = @"I:\Games\UnderRail\data\maps\locale\static\stationalpha-level1.uz";
            //Process(file, fileManager);
            //file = @"I:\Games\UnderRail\data\maps\locale\static\stationalpha-level1_l1.uzl";
            //Process(file, fileManager);
        }

        private static void Process<T>(string file, FileManager fileManager)
        {
            //var output_dump = file.Replace("static", "static_dump");
            //fileManager.Dump(file, output_dump);
                
            var model = fileManager.Load<T>(file, true);

            if (model != null)
            {
                var output = file.Replace("static", "static2");
                fileManager.Save(model, output, false);
            }
        }
    }
}
