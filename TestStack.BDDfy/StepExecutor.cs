using System.Diagnostics;

namespace TestStack.BDDfy
{    
    public class StepExecutor : IStepExecutor
    {
        /// <summary>
        /// Executes the step. If you'd like to run your own custom logic before/after each step, 
        /// override this method and call the base method within your implementation.
        /// </summary>
        public virtual object Execute(Step step, object testObject)
        {
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                var result = step.Action(testObject);
                sw.Stop();
                step.Duration = sw.Elapsed;
                return result;
            }
            finally
            {
                sw.Stop();
                step.Duration = sw.Elapsed;
            }
        }
    }
}