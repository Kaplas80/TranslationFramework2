using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TF.Core.Entities;

namespace TF.Core
{
    public class PluginManager
    {
        private IList<IGame> _loadedPlugins;

        public void LoadPlugins(string pluginsFolder)
        {
            IList<IGame> plugins = new List<IGame>();

            if (Directory.Exists(pluginsFolder))
            {
                var dllFileNames = Directory.GetFiles(pluginsFolder, "TFGame.*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (var dllFile in dllFileNames)
                {
                    try
                    {
                        var an = AssemblyName.GetAssemblyName(dllFile);
                        var assembly = Assembly.Load(an);
                        assemblies.Add(assembly);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Error Loading Assembly: {dllFile}\n{e.Message}");
                    }
                }

                var pluginType = typeof(IGame);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (var assembly in assemblies)
                {
                    if (assembly == null)
                    {
                        continue;
                    }

                    try
                    {
                        var types = assembly.GetTypes();

                        foreach (var type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }

                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                foreach (var type in pluginTypes)
                {
                    try
                    {
                        var plugin = (IGame)Activator.CreateInstance(type);
                        plugins.Add(plugin);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Error Loading Game Plugin: {type.FullName}\n{e.Message}");
                    }
                }
            }

            _loadedPlugins = plugins;
        }

        public IList<IGame> GetAllGames()
        {
            return _loadedPlugins;
        }

        public IGame GetGame(string gameId)
        {
            return _loadedPlugins.FirstOrDefault(x => x.Id == gameId);
        }
    }
}
