using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace TestStack.BDDfy.Processors
{
    public static class AsyncTestRunner
    {        
        public static void Run(Func<object> performStep)
        {
            var oldSyncContext = SynchronizationContext.Current;
            try
            {
                var asyncSyncContext = new AsyncTestSyncContext();
                SetSynchronizationContext(asyncSyncContext);
                var result = performStep();
                if (result is Task task)
                {
                    try
                    {
                        task.Wait();
                    }
                    catch (AggregateException ae)
                    {
                        var innerException = ae.InnerException;
                        ExceptionProcessor.PreserveStackTrace(innerException);
                        throw innerException;
                    }
                }
                else
                {
                    var ex = asyncSyncContext.WaitForCompletion();
                    if (ex != null)
                    {
                        ExceptionProcessor.PreserveStackTrace(ex);
                        throw ex;
                    }
                }
            }
            finally
            {
                SetSynchronizationContext(oldSyncContext);
            }
        }

        [SecuritySafeCritical]
        private static void SetSynchronizationContext(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
        }
    }
}