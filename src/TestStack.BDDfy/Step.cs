using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TestStack.BDDfy.Configuration;
using System.Linq;
namespace TestStack.BDDfy
{
    public class Step
    {
        private StepTitle _title;
        private LambdaExpression _stepAction;
        private Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> _createTitle;
        private string _stepTextTemplate;
        private string _stepPrefix;
        private bool _includeInputsInStepTitle;
        private Func<LambdaExpression, MethodInfo> _getMethodInfo;
        private object _testObject;
        private bool _reports;


        public Step(
            Func<object, object> action,
            StepTitle title,
            bool asserts,
            ExecutionOrder executionOrder,
            bool shouldReport,
            List<StepArgument> arguments)
        {
            Id = Configurator.IdGenerator.GetStepId();
            Asserts = asserts;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Result = Result.NotExecuted;
            Action = action;
            Arguments = arguments;
            _title = title;
        }

        public Step(Step step)
        {
            Id = step.Id;
            _title = step._title;
            Asserts = step.Asserts;
            ExecutionOrder = step.ExecutionOrder;
            ShouldReport = step.ShouldReport;
            Result = Result.NotExecuted;
            Action = step.Action;
            Arguments = step.Arguments;
        }

        public Step(LambdaExpression stepAction, Func<object, object> action, Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> createTitle, string stepTextTemplate, string stepPrefix, bool includeInputsInStepTitle, Func<LambdaExpression, MethodInfo> getMethodInfo, object testObject, string title, bool assert, ExecutionOrder executionOrder, bool shouldReport, List<StepArgument> args)
        {
            _stepAction = stepAction;
            Action = action;
            _createTitle = createTitle;
            _stepTextTemplate = stepTextTemplate;
            _includeInputsInStepTitle = includeInputsInStepTitle;
            _getMethodInfo = getMethodInfo;
            _testObject = testObject;
            this.Asserts = assert;
            _title = new StepTitle(title);
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Arguments = args;
            _stepPrefix = stepPrefix;
        }

        public Step(LambdaExpression stepAction, string stepTextTemplate, bool includeInputsInStepTitle, Func<LambdaExpression, MethodInfo> getMethodInfo, string stepPrefix, Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> createTitle, Func<object, object> action, StepTitle title, bool assert, ExecutionOrder executionOrder, bool shouldReport, List<StepArgument> args)
        {
            _stepAction = stepAction;
            _stepTextTemplate = stepTextTemplate;
            _includeInputsInStepTitle = includeInputsInStepTitle;
            _getMethodInfo = getMethodInfo;
            _stepPrefix = stepPrefix;
            Action = action;
            _title = title;
            Asserts = assert;
            ExecutionOrder = executionOrder;
            ShouldReport = shouldReport;
            Arguments = args;
            _createTitle = createTitle;
            
        }

        public void ResetTitle()
        {
            if (_stepAction != null)
            {
                var action = _stepAction.Compile();
                var inputArguments = new StepArgument[0];
                if (_includeInputsInStepTitle)
                {
                    inputArguments = _stepAction.ExtractArguments(_testObject).ToArray();
                }

                _title = _createTitle(_stepTextTemplate, _includeInputsInStepTitle, _getMethodInfo(_stepAction), inputArguments, _stepPrefix);
            }
        }

        public string Id { get; private set; }
        internal Func<object, object> Action { get; set; }
        public bool Asserts { get; private set; }
        public bool ShouldReport { get; private set; }
        public string Title
        {
            get
            {
                return _title.ToString();
            }
        }

        public ExecutionOrder ExecutionOrder { get; private set; }

        public Result Result { get; set; }
        public Exception Exception { get; set; }
        public int ExecutionSubOrder { get; set; }
        public TimeSpan Duration { get; set; }
        public List<StepArgument> Arguments { get; private set; }
    }
}