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

            var files = Directory.EnumerateFiles(@"I:\Games\Underrail\data\rules\items", "*.item",
                SearchOption.AllDirectories);
            foreach (var file in files)
            {
                //Debug.WriteLine(file);

                var output_dump = file.Replace("items", "items_dump");
                fileManager.Dump(file, output_dump, true);
                
                var model = fileManager.Load(file, true);

                if (model != null)
                {
                    var output = file.Replace("items", "items2");
                    fileManager.Save(model, output, false);
                }
            }

            //var file = @"I:\Games\Underrail\data\rules\items\weapons\xpbl\reddragon.item";
            //var model = fileManager.Load(file, true);

            //if (model != null)
            //{
            //    var output = file.Replace("items", "items2");
            //    fileManager.Save(model, output, true);
            //}
        }
    }
}
