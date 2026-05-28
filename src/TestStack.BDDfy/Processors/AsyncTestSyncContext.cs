using System;
using System.Threading;

namespace TestStack.BDDfy
{
    /// <summary>
    /// Implementation from xUnit 2.0
    /// </summary>
    internal class AsyncTestSyncContext : SynchronizationContext
    {
        readonly object _lock = new();
        Exception _exception;
        int _operationCount;

        public override void OperationCompleted()
        {
            lock (_lock)
            {
                _operationCount--;
                if (_operationCount == 0)
                    Monitor.PulseAll(_lock);
            }
        }

        public override void OperationStarted()
        {
            lock (_lock)
            {
                _operationCount++;
            }
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            // The call to Post() may be the state machine signaling that an exception is
            // about to be thrown, so we make sure the operation count gets incremented
            // before the QUWI, and then decrement the count when the operation is done.
            OperationStarted();

            ThreadPool.QueueUserWorkItem(s =>
            {
                try
                {
                    Send(d, state);
                }
                finally
                {
                    OperationCompleted();
                }
            });
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            try
            {
                d(state);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        public Exception WaitForCompletion()
        {
            lock (_lock)
            {
                while (_operationCount > 0)
                    Monitor.Wait(_lock);
            }

            return _exception;
        }
    }
}