using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;

namespace TestStack.BDDfy.Tests.FluentScanner
{
    public enum SomeEnumForTesting
    {
        Value1,
        Value2
    }

    public class SomeClassWithStaticMembers
    {
        public static string StringProp { get { return "asdfsadf"; } }
        public static int IntProp { get { return 1; } }
    }

    [Story(
        Title = "BDDfy using fluent API",
        AsA = "As a programmer",
        IWant = "I want to be able to use fluent api to scan for steps",
        SoThat = "So that I can be in full control of what is passed in")]
    public class BDDfyUsingFluentApi
    {
        private string[] _arrayInput1;
        private int[] _arrayInput2;
        private int _primitiveInput2;
        private string _primitiveInput1;
        private SomeEnumForTesting _enumInput;
        private Action _action;

        public void GivenAnAction(Action actionInput)
        {
            _action = actionInput;    
        }

        public void ThenCallingTheActionThrows<T>() where T : Exception
        {
            Assert.Throws<T>(() => _action());
        }

        public void GivenPrimitiveInputs(string input1, int input2)
        {
            _primitiveInput1 = input1;
            _primitiveInput2 = input2;
        }

        public void GivenEnumInputs(SomeEnumForTesting input)
        {
            _enumInput = input;
        }

        public void GivenArrayInputs(string[] input1, int[] input2)
        {
            _arrayInput1 = input1;
            _arrayInput2 = input2;
        }

        public void GivenEnumerableInputs(IEnumerable<string> input1, IEnumerable<int> input2)
        {
            _arrayInput1 = input1.ToArray();
            _arrayInput2 = input2.ToArray();
        }

        public void ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(string expectedInput1, int expectedInput2)
        {
            Assert.That(_primitiveInput1, Is.EqualTo(expectedInput1));
            Assert.That(_primitiveInput2, Is.EqualTo(expectedInput2));
        }

        public void ThenEnumArgumentIsPassedInProperlyAndStoredOnTheSameObjectInstance(SomeEnumForTesting expectedInput)
        {
            Assert.That(_enumInput, Is.EqualTo(expectedInput));
        }

        public void ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(IEnumerable<string> expectedInput1, IEnumerable<int> expectedInput2)
        {
            Assert.That(_arrayInput1, Is.EqualTo(expectedInput1));
            Assert.That(_arrayInput2, Is.EqualTo(expectedInput2));
        }

        string _primitiveInput1Field = "1";
        int _primitiveInput2Field = 2;

        SomeEnumForTesting _enumInputField = SomeEnumForTesting.Value2;

        public string PrimitiveInput1Property { get { return _primitiveInput1Field; } }
        public int PrimitiveInput2Property { get { return _primitiveInput2Field; } }

        public SomeEnumForTesting EnumInputProperty { get { return _enumInputField; } }

        string[] _arrayInput1Field = new[] { "1", "2" };
        int[] _arrayInput2Field = new[] { 3, 4 };

        private IEnumerable<string> EnumerableString = new[] {"1", null, "2"};
        private IEnumerable<int> EnumerableInt = new[] {1, 2};

        public string[] ArrayInput1Property { get { return _arrayInput1Field; } }
        public int[] ArrayInput2Property { get { return _arrayInput2Field; } }

        [Test]
        public void PassingPrimitiveArgumentsInline()
        {
            this.Given(x => x.GivenPrimitiveInputs("1", 2), "Given inline input arguments {0} and {1}")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .BDDfy();
        }

        [Test]
        public void PassingPublicStaticPrimitiveArguments()
        {
            this.Given(x => x.GivenPrimitiveInputs(string.Empty, 2), "Given inline input arguments {0} and {1}")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(string.Empty, 2))
                .BDDfy();
        }

        [Test]
        public void PassingPublicStaticPrimitivePropertyAsArguments()
        {
            this.Given(x => x.GivenPrimitiveInputs(SomeClassWithStaticMembers.StringProp, SomeClassWithStaticMembers.IntProp), "Given inline input arguments {0} and {1}")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(SomeClassWithStaticMembers.StringProp, SomeClassWithStaticMembers.IntProp))
                .BDDfy();
        }

        [Test]
        public void PassingNullPrimitiveArgumentInline()
        {
            this.Given(x => x.GivenPrimitiveInputs(null, 2), "Given inline input arguments {0} and {1}")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(null, 2))
                .BDDfy();
        }

        [Test]
        public void PassingPrimitiveArgumentsUsingVariables()
        {
            var input1 = "1";
            var input2 = 2;

            this.Given(x => x.GivenPrimitiveInputs(input1, input2), "Given input arguments {0} and {1} are passed in using varialbles")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(input1, input2))
                .BDDfy();
        }

        [Test]
        public void PassingNullAsPrimitiveArgumentsUsingVariables()
        {
            string input1 = null;
            var input2 = 2;

            this.Given(x => x.GivenPrimitiveInputs(input1, input2), "Given input arguments {0} and {1} are passed in using varialbles")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(input1, input2))
                .BDDfy();
        }
 
        [Test]
        public void PassingPrimitiveArgumentsUsingFields()
        {
            this.Given(x => x.GivenPrimitiveInputs(_primitiveInput1Field, _primitiveInput2Field), "Given input arguments {0} and {1} are passed in using fields")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .BDDfy();
        }
 
        [Test]
        public void PassingPrimitiveArgumentsUsingProperties()
        {
            this.Given(x => x.GivenPrimitiveInputs(PrimitiveInput1Property, PrimitiveInput2Property), "Given input arguments {0} and {1} are passed in using properties")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .BDDfy();
        }

        [Test]
        public void PassingEnumArgumentInline()
        {
            this.Given(x => x.GivenEnumInputs(SomeEnumForTesting.Value1), "Given inline enum argument {0}")
                .Then(x => x.ThenEnumArgumentIsPassedInProperlyAndStoredOnTheSameObjectInstance(SomeEnumForTesting.Value1))
                .BDDfy();
        }

        [Test]
        public void PassingEnumArgumentUsingVariable()
        {
            var someEnumForTesting = SomeEnumForTesting.Value1;
            this.Given(x => x.GivenEnumInputs(someEnumForTesting), "Given enum argument {0} provided using variable")
                .Then(x => x.ThenEnumArgumentIsPassedInProperlyAndStoredOnTheSameObjectInstance(someEnumForTesting))
                .BDDfy();
        }

        [Test]
        public void PassingEnumArgumentUsingFields()
        {
            this.Given(x => x.GivenEnumInputs(_enumInputField), "Given enum argument {0} provided using fields")
                .Then(x => x.ThenEnumArgumentIsPassedInProperlyAndStoredOnTheSameObjectInstance(_enumInputField))
                .BDDfy();
        }

        [Test]
        public void PassingArrayArgumentsInline()
        {
            this.Given(x => x.GivenArrayInputs(new[] { "1", "2" }, new[] { 3, 4 }), "Given inline array input arguments")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", "2" }, new[] { 3, 4 }))
                .BDDfy();
        }

        [Test]
        public void PassingEnumerableArguments()
        {
            this.Given(x => x.GivenEnumerableInputs(EnumerableString, EnumerableInt), "Given enumerable input arguments")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(EnumerableString, EnumerableInt))
                .BDDfy();
        }
        
        [Test]
        public void PassingNullArrayArgumentInline()
        {
            this.Given(x => x.GivenArrayInputs(new[] {"1", null, "2"}, new[] {1, 2}), "Given inline input arguments {0} and {1}")
                    .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", null, "2" }, new[] { 1, 2 }))
                .BDDfy();
        }

        [Test]
        public void PassingNullAsArrayArgumentInline()
        {
            this.Given(x => x.GivenArrayInputs(null, new[] {1, 2}), "Given inline input arguments {0} and {1}")
                    .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(null, new[] { 1, 2 }))
                .BDDfy();
        }

        [Test]
        public void PassingArrayArgumentsUsingVariables()
        {
            var input1 = new[] {"1", "2"};
            var input2 = new[] {3, 4};

            this.Given(x => x.GivenArrayInputs(input1, input2), "Given array input arguments {0} and {1} are passed in using variables")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(input1, input2))
                .BDDfy();
        }

        [Test]
        public void PassingNullAsOneOfArrayArgumentUsingVariables()
        {
            var input1 = new[] {null, "2"};
            var input2 = new[] {3, 4};

            this.Given(x => x.GivenArrayInputs(input1, input2), "Given array input arguments {0} and {1} are passed in using variables")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(input1, input2))
                .BDDfy();
        }
 
        [Test]
        public void PassingArrayArgumentsUsingFields()
        {
            this.Given(x => x.GivenArrayInputs(_arrayInput1Field, _arrayInput2Field), "Given array input arguments {0} and {1} are passed in using fields")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", "2" }, new[] { 3, 4 }))
                .BDDfy();
        }
 
        [Test]
        public void PassingArrayArgumentsUsingProperties()
        {
            this.Given(x => x.GivenArrayInputs(ArrayInput1Property, ArrayInput2Property), "Given array input arguments {0} and {1} are passed in using properties")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", "2" }, new[] { 3, 4 }))
                .BDDfy();
        }

        [Test]
        public void WhenTitleIsNotProvidedItIsFetchedFromMethodName()
        {
            var story = 
                this.Given(x => x.GivenPrimitiveInputs("1", 2))
                    .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                    .BDDfy();

            var scenario = story.Scenarios.First();
            Assert.That(scenario.Title, Is.EqualTo(Configurator.Scanners.Humanize(MethodBase.GetCurrentMethod().Name)));
        }

        [Test]
        public void WhenTitleIsProvidedItIsUsedAsIs()
        {
            const string dummyTitle = "some dummy title; blah blah $#^";
            var story = 
                this.Given(x => x.GivenPrimitiveInputs("1", 2))
                    .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                    .BDDfy(dummyTitle);

            var scenario = story.Scenarios.First();
            Assert.That(scenario.Title, Is.EqualTo(dummyTitle));
        }

        private static void ExceptionThrowingAction()
        {
            throw new ApplicationException();
        }

        [Test]
        public void CanPassActionToFluentApi()
        {
            this.Given(x => x.GivenAnAction(ExceptionThrowingAction))
                .Then(x => x.ThenCallingTheActionThrows<ApplicationException>())
                .BDDfy();
        }

        [Test]
        public void CanPassActionAndTitleToFluentApi()
        {
            this.Given(x => x.GivenAnAction(ExceptionThrowingAction), "Given an action that throws AppliationException")
                .Then(x => x.ThenCallingTheActionThrows<ApplicationException>(), "Then calling the action does throw that exception")
                .BDDfy();
        }
    }
}