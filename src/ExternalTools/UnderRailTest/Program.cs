using System;
using System.Diagnostics;
using System.IO;
using UnderRailLib;
using UnderRailLib.AssemblyResolver;

namespace UnderRailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Initialize();
            Binder.SetAssemblyResolver(assemblyResolver);

            var dialogManager = new DialogManager();

            var files = Directory.EnumerateFiles(@"H:\Games\Underrail\data\dialogs", "*.udlg",
                SearchOption.AllDirectories);

            foreach (var file in files)
            {
                //Debug.WriteLine(file);
                var model = dialogManager.LoadModel(file);

                if (model != null)
                {
                    var output = file.Replace("dialogs", "dialogs2");
                    dialogManager.SaveModel(model, output);
                }
            }

            /*var file = @"H:\Games\Underrail\data\dialogs\characters\abram.udlg";
            var model = dialogManager.LoadModel(file);
            if (model != null)
            {
                var file2 = file.Replace("dialogs", "dialogs2");
                dialogManager.SaveModel(model, file2);
                var model2 = dialogManager.LoadModel(file2);
            }*/
        }
    }
}
