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

            var fileManager = new FileManager<Item>();

            //var files = Directory.EnumerateFiles(@"I:\Games\Underrail\data\rules\items", "*.item",
            //    SearchOption.AllDirectories);
            //foreach (var file in files)
            //{
            //    //Debug.WriteLine(file);

            //    Process(file, fileManager);
            //}

            var file = @"I:\Games\UnderRail\data\rules\items\components\weapons\assaultrifleframe1.item";
            Process(file, fileManager);
        }

        private static void Process(string file, FileManager<Item> fileManager)
        {
            var output_dump = file.Replace("items", "items_dump");
            fileManager.Dump(file, output_dump, true);
                
            var model = fileManager.Load(file, true);

            if (model != null)
            {
                var output = file.Replace("items", "items2");
                fileManager.Save(model, output, false);
            }
        }
    }
}
