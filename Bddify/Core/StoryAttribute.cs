using System;

namespace Bddify.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StoryAttribute : Attribute
    {
        public string Title { get; set; }
        public string AsA { get; set; }
        public string IWant  { get; set; }
        public string SoThat { get; set; }
    }
}