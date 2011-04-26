using System.Linq;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.GwtAttributes;
using NUnit.Framework;

namespace Bddify.Tests.Scanner.StepText
{
    public class ExecutableAttributeScannerTests
    {
        class ScenarioWithVaryingStepTexts
        {
            [Given]
            public void ThePascalCaseForMethodName() { }
            [When]
            public void Step_Name_Uses_Underscore_With_Pascal_Case() { }
            [Then]
            public void with_lower_case_underscored_method_name() { }

            [Given]
            [RunStepWithArgs(1, 2, 3)]
            [RunStepWithArgs(3, 4, 5)]
            public void StepIsRunWithArgumentsWithoutProvidedText(int input1, int input2, int input3) { }

            [Given]
            [RunStepWithArgs(1, 2, 3, StepTextTemplate = "The step text gets argument {0}, {1} and then {2}")]
            [RunStepWithArgs(3, 4, 5, StepTextTemplate = "The step text gets argument {0}, {1} and then {2}")]
            public void StepIsRunWithArgumentsWithProvidedText(int input1, int input2, int input3) { }
        }

        static void VerifyMethod(string expectedReadableMethodName, bool exists = true)
        {
            var scanner = new ExecutableAttributeScanner();
            var steps = scanner.Scan(typeof(ScenarioWithVaryingStepTexts)).ToList();
            var theStep = steps.Where(s => s.ReadableMethodName == expectedReadableMethodName);

            if (exists)
                Assert.That(theStep.Count(), Is.EqualTo(1));
            else
                Assert.That(theStep.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TheMethodWithPascalCaseIsSeparatedAndTurnedIntoLowerCaseExceptTheFirstWord()
        {
            VerifyMethod("The pascal case for method name");
        }

        [Test]
        public void TheMethodWithUnderscoreAndPascalCaseIsSeparatedButCaseIsRetained()
        {
            VerifyMethod("Step Name Uses Underscore With Pascal Case");
        }

        [Test]
        public void TheMethodWithUnderscoreAndLowerCaseWordsIsSeparatedAndCaseIsRetained()
        {
            VerifyMethod("with lower case underscored method name");
        }

        [Test]
        public void TheMethodWithArgumentWithoutProvidedTextGetsArgumentsAppendedToTheMethodName()
        {
            VerifyMethod("Step is run with arguments without provided text 1, 2, 3");
            VerifyMethod("Step is run with arguments without provided text 3, 4, 5");
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
            VerifyMethod("Step is run with arguments with provided text 1, 2, 3", false);
            VerifyMethod("Step is run with arguments with provided text 3, 4, 5", false);
        }
    }
}