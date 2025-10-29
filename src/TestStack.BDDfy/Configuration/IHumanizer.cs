using System;

namespace TestStack.BDDfy.Configuration
{
    public interface IHumanizer
    {
        Func<string, bool> PreserveCasingWhen { get; set; }
        string Humanize(string input);
    }
}