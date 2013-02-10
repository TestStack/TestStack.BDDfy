using System;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    class BasicTag : IDisposable
    {
        private readonly Action _closeTagAction;

        public BasicTag(Action closeTagAction)
        {
            _closeTagAction = closeTagAction;
        }

        public void Dispose()
        {
            _closeTagAction();
        }
    }
}