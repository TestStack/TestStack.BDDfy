using System;

namespace TestStack.BDDfy
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StoryAttribute : Attribute
    {
        public string Title { get; set; }
        public string AsA { get; set; }
        public string IWant { get; set; }
        public string SoThat { get; set; }
        public string InOrderTo { get; set; }
    }
}