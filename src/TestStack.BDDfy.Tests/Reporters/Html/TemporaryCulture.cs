#if Culture
using System;
using System.Globalization;
using System.Threading;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    public class TemporaryCulture : IDisposable
    {
        private readonly string _originalCulture;
        public TemporaryCulture(string newCulture)
        {
            _originalCulture = Thread.CurrentThread.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(newCulture);
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(_originalCulture);
        }
    }
}
#endif