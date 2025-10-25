using System;

namespace TestStack.BDDfy.Reporters.Html
{
    class HtmlReportTag(HtmlTag tag, Action<HtmlTag> closeTagAction): IDisposable
    {
        private readonly HtmlTag _tagName = tag;
        private readonly Action<HtmlTag> _closeTagAction = closeTagAction;

        public void Dispose()
        {
            _closeTagAction(_tagName);
        }
    }
}