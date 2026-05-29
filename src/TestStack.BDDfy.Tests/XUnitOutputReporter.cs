using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests
{
    public class XUnitOutputReporter(ITestOutputHelper testOutputHelper = null): TextReporter
    {
        private ITestOutputHelper _outputHelper = testOutputHelper ?? Xunit.TestContext.Current.TestOutputHelper;

        protected override void WriteLine(string text = null)
        {
            if (text is not null) _outputHelper.WriteLine(text);
            base.WriteLine(text);
        }

        protected override void Write(string text, params object[] args)
        {
            _outputHelper.Write(text, args);
            base.Write(text, args);
        }

        protected override void WriteLine(string text, params object[] args)
        {
            _outputHelper.WriteLine(text, args);
            base.WriteLine(text, args);
        }
    }
}