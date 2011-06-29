using System.Linq;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;
using NUnit.Framework;

namespace Bddify.Tests.Scanner.StepText
{
    public class ExecutableAttributeScannerTests
    {
        class ScenarioWithVaryingStepTexts
        {
            [Given]
            public void ThePascalCaseForMethodName() { }

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

            [When]
            [RunStepWithArgs(new[] { 1, 2, 3, 4, 5 })]
            public void WhenStepIsRunWithArrayArgumentsWithoutProvidedText(int[] input) { }

            [When]
            [RunStepWithArgs(new[] { 1, 2, 3, 4, 5 }, StepTextTemplate = "With the following inputs {0}")]
            public void WhenStepIsRunWithArrayArgumentsWithProvidedText(int[] input) { }

            [When(StepText = "Running step with arg {0}, {1} and {2} using exec attribute template")]
            [RunStepWithArgs(1, 2, 3)]
            public void RunningStepWithArgsUsingExecAttributeTemplate(int input1, int input2, int input3){}

            [When(StepText = "Running step with arg {0}, {1} and {2} when template is provided by exec attribute and RunStepWithArgs attribute")]
            [RunStepWithArgs(1, 2, 3)]
            [RunStepWithArgs(4, 5, 6, StepTextTemplate = "The template provided on RunStepWithArgs overrides all the others {0}, {1}, {2}")]
            public void RunningStepWithArgsUsingExecAttributeTemplateAndRunStepWithArgsTemplate(int input1, int input2, int input3){}
        }

        static void VerifyMethod(string expectedReadableMethodName, bool exists = true)
        {
            var scanner = new ExecutableAttributeStepScanner();
            var steps = scanner.Scan(new ScenarioWithVaryingStepTexts()).ToList();
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
        public void TheMethodWithUnderscoreAndLowerCaseWordsIsSeparatedAndCaseIsRetained()
        {
            VerifyMethod("with lower case underscored method name");
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

        [Test]
        public void TheMethodWithArgumentWithTextProvidedOnTheExecutableAttributeUsesExecutableAttributeTemplate()
        {
            VerifyMethod("Running step with arg 1, 2 and 3 using exec attribute template");
        }

        [Test]
        public void RunStepWithArgsTemplateOverrideAllOtherTemplates()
        {
            VerifyMethod("Running step with arg 1, 2 and 3 when template is provided by exec attribute and RunStepWithArgs attribute");
            VerifyMethod("Running step with args using exec attribute template and run step with args template 1, 2, 3", false);
            VerifyMethod("The template provided on RunStepWithArgs overrides all the others 4, 5, 6");
        }
    }
}