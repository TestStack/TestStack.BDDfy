using System.Collections.Generic;
using System.Linq;

namespace Bddify.Core
{
    public class Bddee
    {
        public Bddee(object bddeeObject, IEnumerable<ExecutionStep> steps)
        {
            Object = bddeeObject;
            _steps = steps.ToList();

            ScenarioSentence = NetToString.CreateSentenceFromTypeName(bddeeObject.GetType().Name);
        }

        public string ScenarioSentence { get; private set; }

        public object Object { get; set; }
        private readonly List<ExecutionStep> _steps;
        public IEnumerable<ExecutionStep> Steps
        {
            get { return _steps; }
        }

        public StepExecutionResult Result
        {
            get
            {
                return (StepExecutionResult)Steps.Max(s => (int)s.Result);
            }
        }
    }
}