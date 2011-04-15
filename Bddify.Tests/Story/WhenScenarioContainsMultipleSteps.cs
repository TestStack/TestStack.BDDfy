using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.Story
{
    public class WhenScenarioContainsMultipleSteps
    {
        private class ScenarioWithMultipleSteps
        {
            void GivenStep()
            {
                
            }

            void WhenStep()
            {
                
            }

            [RunStepWithArgs(1, 2)]
            [RunStepWithArgs(3, 4)]
            [RunStepWithArgs(5, 6)]
            void ThenStep()
            {
                
            }
        }
    }
}