using System;
using Bddify.Core;
using NUnit.Framework;

namespace Bddify.Tests.FluentStepScanner
{
    [Story
    (AsA = "As a programmer",
    IWant = "I want to be able to use fluent api to scan for steps",
    SoThat = "So that I can be in full control of what is passed in")]
    public class BddifyUsingFluentApi
    {
        private string[] _arrayInput1;
        private int[] _arrayInput2;
        private int _primitiveInput2;
        private string _primitiveInput1;

        public void GivenPrimitiveInputs(string input1, int input2)
        {
            _primitiveInput1 = input1;
            _primitiveInput2 = input2;
        }

        public void GivenArrayInputs(string[] input1, int[] input2)
        {
            _arrayInput1 = input1;
            _arrayInput2 = input2;
        }

        public void ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(string expectedInput1, int expectedInput2)
        {
            Assert.That(_primitiveInput1, Is.EqualTo(expectedInput1));
            Assert.That(_primitiveInput2, Is.EqualTo(expectedInput2));
        }

        public void ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(string[] expectedInput1, int[] expectedInput2)
        {
            Assert.That(_arrayInput1, Is.EqualTo(expectedInput1));
            Assert.That(_arrayInput2, Is.EqualTo(expectedInput2));
        }

        [Test]
        public void CallingIncorrectBddifyOverloadShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                this.Scan()
                .Given(x => x.GivenPrimitiveInputs("1", 2))
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .Bddify(scenarioTextTemplate: "using scenarioTextTemplate calls into an incorrect overload"));
        }

        string _primitiveInput1Field = "1";
        int _primitiveInput2Field = 2;

        public string PrimitiveInput1Property { get { return _primitiveInput1Field; } }
        public int PrimitiveInput2Property { get { return _primitiveInput2Field; } }

        string[] _arrayInput1Field = new[] { "1", "2" };
        int[] _arrayInput2Field = new[] { 3, 4 };

        public string[] ArrayInput1Property { get { return _arrayInput1Field; } }
        public int[] ArrayInput2Property { get { return _arrayInput2Field; } }

        [Test]
        public void PassingPrimitiveArgumentsInline()
        {
            this.Scan()
                .Given(x => x.GivenPrimitiveInputs("1", 2), "Given inline input arguments {0} and {1}")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .Bddify("Passing primitive arguments inline");
        }

        [Test]
        public void PassingPrimitiveArgumentsUsingVariables()
        {
            var input1 = "1";
            var input2 = 2;

            this.Scan()
                .Given(x => x.GivenPrimitiveInputs(input1, input2), "Given input arguments {0} and {1} are passed in using varialbles")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .Bddify("Passing primitive arguments using variables");
        }
 
        [Test]
        public void PassingPrimitiveArgumentsUsingFields()
        {
            this.Scan()
                .Given(x => x.GivenPrimitiveInputs(_primitiveInput1Field, _primitiveInput2Field), "Given input arguments {0} and {1} are passed in using fields")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .Bddify("Passing primitive arguments using fields");
        }
 
        [Test]
        public void PassingPrimitiveArgumentsUsingProperties()
        {
            this.Scan()
                .Given(x => x.GivenPrimitiveInputs(PrimitiveInput1Property, PrimitiveInput2Property), "Given input arguments {0} and {1} are passed in using properties")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance("1", 2))
                .Bddify("Passing primitive arguments using properties");
        }

        [Test]
        public void PassingArrayArgumentsInline()
        {
            this.Scan()
                .Given(x => x.GivenArrayInputs(new[] { "1", "2" }, new[] { 3, 4 }), "Given inline array input arguments")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", "2" }, new[] { 3, 4 }))
                .Bddify("Passing array arguments inline");
        }

        [Test]
        public void PassingArrayArgumentsUsingVariables()
        {
            var input1 = new[] {"1", "2"};
            var input2 = new[] {3, 4};

            this.Scan()
                .Given(x => x.GivenArrayInputs(input1, input2), "Given array input arguments {0} and {1} are passed in using variables")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", "2" }, new[] { 3, 4 }))
                .Bddify("Passing array arguments using variables");
        }
 
        [Test]
        public void PassingArrayArgumentsUsingFields()
        {
            this.Scan()
                .Given(x => x.GivenArrayInputs(_arrayInput1Field, _arrayInput2Field), "Given array input arguments {0} and {1} are passed in using fields")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", "2" }, new[] { 3, 4 }))
                .Bddify("Passing array arguments using fields");
        }
 
        [Test]
        public void PassingArrayArgumentsUsingProperties()
        {
            this.Scan()
                .Given(x => x.GivenArrayInputs(ArrayInput1Property, ArrayInput2Property), "Given array input arguments {0} and {1} are passed in using properties")
                .Then(x => x.ThenTheArgumentsArePassedInProperlyAndStoredOnTheSameObjectInstance(new[] { "1", "2" }, new[] { 3, 4 }))
                .Bddify("Passing array arguments using properties");
        }
    }
}