using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bddify.Configuration
{
    public class ConfigurationFactory
    {
        public static IEnumerable<T> GetConfigurations<T>() where T : IConfiguration
        {
            var configufationTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(T).IsAssignableFrom(t)) // the configs in the test assembly
                .Union(typeof(ConfigurationFactory).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(T).IsAssignableFrom(t))); // plus the default configs from the bddify assembly

            return configufationTypes.Select(Activator.CreateInstance).Cast<T>().OrderBy(c => c.Priority);
        }
    }
}