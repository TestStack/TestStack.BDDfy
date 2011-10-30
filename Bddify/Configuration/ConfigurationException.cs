using System;

namespace Bddify.Configuration
{
    public class ConfigurationException : ApplicationException
    {
        public ConfigurationException(string message) : base(message)
        {
        }
    }
}