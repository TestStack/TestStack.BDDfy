namespace TestStack.BDDfy.Configuration
{
    public class SequentialKeyGenerator : IKeyGenerator
    {
        private int _currentScenarioNumber = 1;
        private int _currentStepNumber = 1;

        public string GetScenarioId()
        {
            var id = string.Format("scenario-{0}", _currentScenarioNumber);
           _currentScenarioNumber++;
            _currentStepNumber = 1;
            return id;
        }

        public string GetStepId()
        {
            var id = string.Format("step-{0}-{1}", _currentScenarioNumber, _currentStepNumber);
            _currentStepNumber++;
            return id;
        }

        public void Reset()
        {
            _currentScenarioNumber = 1;
            _currentStepNumber = 1;
        }
    }
}