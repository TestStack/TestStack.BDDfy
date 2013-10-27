using System;
using System.Threading;

namespace TestStack.BDDfy.Scanners.StepScanners
{
    /// <summary>
    /// Implementation from xUnit 2.0
    /// </summary>
    internal class AsyncTestSyncContext : SynchronizationContext
    {
        readonly ManualResetEvent _event = new ManualResetEvent(initialState: true);
        Exception _exception;
        int _operationCount;

        public override void OperationCompleted()
        {
            var result = Interlocked.Decrement(ref _operationCount);
            if (result == 0)
                _event.Set();
        }

        public override void OperationStarted()
        {
            Interlocked.Increment(ref _operationCount);
            _event.Reset();
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
            _event.WaitOne();
            return _exception;
        }
    }
}