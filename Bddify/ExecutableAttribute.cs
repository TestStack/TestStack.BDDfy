using System;

namespace Bddify
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutableAttribute : Attribute
    {
        public ExecutableAttribute(int order, string text)
        {
            Text = text;
            Order = order;
        }

        public int Order { get; private set; }
        public bool Asserts { get; set; }
        public string Text { get; private set; }
        public int TextPad { get; set; }
    }
}