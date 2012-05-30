using BDDfy.Core;
using BDDfy.Scanners.StepScanners.MethodName;
using NUnit.Framework;
using System.Linq;

namespace BDDfy.Tests.Scanner.StepText
{
    public class DefaultScanForStepsByMethodNameTests
    {
        class ScenarioWithVaryingStepTexts
        {
            public void GivenThePascalCaseForMethodName() { }
            public void When_Step_Name_Uses_Underscore_With_Pascal_Case() { }
            public void Then_with_lower_case_underscored_method_name() { }
            
            [RunStepWithArgs(1, 2, 3)]
            [RunStepWithArgs(3, 4, 5)]
            public void WhenStepIsRunWithArgumentsWithoutProvidedText(int input1, int input2, int input3) { }

            [RunStepWithArgs(new[] {1, 2, 3, 4, 5})]
            public void WhenStepIsRunWithArrayArgumentsWithoutProvidedText(int[] input) { }

            [RunStepWithArgs(new[] {1, 2, 3, 4, 5}, StepTextTemplate = "With the following inputs {0}")]
            public void WhenStepIsRunWithArrayArgumentsWithProvidedText(int[] input) { }

            [RunStepWithArgs("input string")]
            public void WhenSomeStringIsProvidedAsInput(string input) { }

            [RunStepWithArgs(1, 2, 3, StepTextTemplate = "The step text gets argument {0}, {1} and then {2}")]
            [RunStepWithArgs(3, 4, 5, StepTextTemplate = "The step text gets argument {0}, {1} and then {2}")]
            public void WhenStepIsRunWithArgumentsWithProvidedText(int input1, int input2, int input3) { }

            public void WhenStepNameEndsWithNumber29()
            {
            }
        }

        static void VerifyMethod(string expectedStepTitle, bool exists = true)
        {
            var testObject = new ScenarioWithVaryingStepTexts();
            var scanner = new DefaultMethodNameStepScanner();
            var steps = scanner.Scan(testObject).ToList();
            var theStep = steps.Where(s => s.StepTitle == expectedStepTitle);
            
            if(exists)
                Assert.That(theStep.Count(), Is.EqualTo(1));
            else
                Assert.That(theStep.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TheMethodWithPascalCaseIsSeparatedAndTurnedIntoLowerCaseExceptTheFirstWord()
        {
            VerifyMethod("Given the pascal case for method name");
        }

        [Test]
        public void TheMethodWithUnderscoreAndLowerCaseWordsIsSeparatedAndCaseIsRetained()
        {
            VerifyMethod("Then with lower case underscored method name");
        }

        [Test]
        public void TrailingNumberGetsTheSameTreatmentAsWords()
        {
            VerifyMethod("When step name ends with number 29");
        }

        [Test]
        public void TheMethodWithArgumentWithoutProvidedTextGetsArgumentsAppendedToTheMethodName()
        {
            VerifyMethod("When step is run with arguments without provided text 1, 2, 3");
            VerifyMethod("When step is run with arguments without provided text 3, 4, 5");
        }

        [Test]
        public void TheMethodWithArrayArgumentWithoutProvidedTextGetsArgumentsAppendedToTheMethodName()
        {
            VerifyMethod("When step is run with array arguments without provided text 1, 2, 3, 4, 5");
        }

        [Test]
        public void TheMethodWithArrayArgumentWithProvidedTextUsesArrayToFormatTheTextTemplate()
        {
            VerifyMethod("With the following inputs 1, 2, 3, 4, 5");
        }

        [Test]
        public void TheMethodIsRunWithStringAsProvidedArgumentsWithoutProvidedTextTemplate()
        {
            VerifyMethod("When some string is provided as input input string");
        }

        [Test]
        public void TheMethodWithArgumentWithProvidedTextUsesTheProvidedTextAsTemplate()
        {
            VerifyMethod("The step text gets argument 1, 2 and then 3");
            VerifyMethod("The step text gets argument 3, 4 and then 5");
        }

        [Test]
        public void TheMethodWithArgumentWithProvidedTextDoesNotUseTheMethodName()
        {
            VerifyMethod("When step is run with arguments with provided text 1, 2, 3", false);
            VerifyMethod("When step is run with arguments with provided text 3, 4, 5", false);
        }
    }
}