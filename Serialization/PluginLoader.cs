using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization
{
    public class PluginLoader
    {
        private string _pluginsDirectory;

        public PluginLoader(string pluginsDirectory)
        {
            _pluginsDirectory = pluginsDirectory;
        }

        public List<ITraceResultSerializer> LoadPlugins()
        {
            var plugins = new List<ITraceResultSerializer>();
            var pluginFiles = Directory.GetFiles(_pluginsDirectory, "*.dll");

            foreach (var pluginFile in pluginFiles)
            {
                var assembly = Assembly.LoadFrom(pluginFile);
                var pluginTypes = assembly.GetTypes().Where(t => typeof(ITraceResultSerializer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in pluginTypes)
                {
                    var plugin = (ITraceResultSerializer)Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }
            }
            return plugins;
        }
    }

}