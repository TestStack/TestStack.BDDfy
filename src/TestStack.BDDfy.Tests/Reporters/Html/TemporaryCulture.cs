using System.Globalization;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests.Reporters.Html
{
    public class TemporaryCulture : IDisposable
    {
        private readonly CultureInfo _originalThreadCulture;
        private readonly CultureInfo? _originalConfiguratorCulture;

        public TemporaryCulture(string threadCulture, string? configuratorCulture = null)
        {
            _originalThreadCulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(threadCulture);
            if (configuratorCulture is not null)
            {
                Configurator.CultureInfo = CultureInfo.CreateSpecificCulture(configuratorCulture);
                _originalConfiguratorCulture = Configurator.CultureInfo;
            }
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _originalThreadCulture;
            
            if (_originalConfiguratorCulture is not null)
                Configurator.CultureInfo = _originalConfiguratorCulture;

            GC.SuppressFinalize(this);
        }
    }
}