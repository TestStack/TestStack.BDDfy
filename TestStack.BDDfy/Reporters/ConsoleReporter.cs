using System;

namespace TestStack.BDDfy.Reporters
{
    public class ConsoleReporter : TextReporter
    {
        protected override void Write(string text, params object[] args)
        {
            Console.Write(text, args);
        }

        protected override void WriteLine(string text = null)
        {
            Console.WriteLine(text);
        }

        protected override void WriteLine(string text, params object[] args)
        {
            Console.WriteLine(text, args);
        }

        public override ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }
    }
}