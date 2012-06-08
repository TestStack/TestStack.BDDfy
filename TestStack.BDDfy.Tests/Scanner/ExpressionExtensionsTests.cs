using System;
using System.Linq.Expressions;
using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;

namespace TestStack.BDDfy.Tests.Scanner
{
    public class BaseClass
    {
        protected int InheritedInput1 = 1;
        protected string InheritedInput2 = "2";

        protected int[] InheritedArrayInput1
        {
            get
            {
                return new[] { 3, 4 };
            }
        }

        protected string[] InheritedArrayInput2
        {
            get
            {
                return new[] { "5", "6" };
            }
        }
    }

    public class ExpressionExtensionsTests : BaseClass
    {
        class ClassUnderTest
        {
            public void MethodWithoutArguments()
            {
                
            }

            public void MethodWithInputs(int input1, string input2)
            {
                
            }

            public void MethodWithArrayInputs(int[] input1, string[] input2)
            {
                
            }
        }

        object[] GetArguments(Expression<Action<ClassUnderTest>> action)
        {
            return action.ExtractConstants().ToArray();
        }

        int _input1 = 1;
        string _input2 = "2";
        const string ConstInput2 = "2";

        int[] _arrayInput1 = new[] { 1, 2 };
        public string[] _arrayInput2 = new[] { "3", "4" };

        public int[] ArrayInput1
        {
            get
            {
                return _arrayInput1;
            }
        }

        string[] ArrayInput2
        {
            get
            {
                return _arrayInput2;
            }
        }

        int Input1 { get { return _input1; } }

        public string Input2 { get { return _input2; } }

        int GetInput1(int someInput)
        {
            return someInput + 10;
        }

        string GetInput2(string someInput)
        {
            return someInput + " Input 2";
        }

        [Test]
        public void NoArguments()
        {
            var arguments = GetArguments(x => x.MethodWithoutArguments());
            Assert.That(arguments.Length, Is.EqualTo(0));
        }

        void AssertReturnedArguments(object[] arguments, object expectedArg1, object expectedArg2)
        {
            Assert.That(arguments.Length, Is.EqualTo(2));
            Assert.That(arguments[0], Is.EqualTo(expectedArg1));
            Assert.That(arguments[1], Is.EqualTo(expectedArg2));
        }

        [Test]
        public void InputArgumentsPassedInline()
        {
            var arguments = GetArguments(x => x.MethodWithInputs(1, "2"));
            AssertReturnedArguments(arguments, 1, "2");
        }

        [Test]
        public void InputArgumentsProvidedUsingVariables()
        {
            int input1 = 1;
            const string input2 = "2";
            var arguments = GetArguments(x => x.MethodWithInputs(input1, input2));
            AssertReturnedArguments(arguments, input1, input2);
        }

        [Test]
        public void InputArgumentsProvidedUsingFields()
        {
            var arguments = GetArguments(x => x.MethodWithInputs(_input1, ConstInput2));
            AssertReturnedArguments(arguments, _input1, ConstInput2);
        }

        [Test]
        public void InputArgumentsProvidedUsingProperty()
        {
            var arguments = GetArguments(x => x.MethodWithInputs(Input1, Input2));
            AssertReturnedArguments(arguments, Input1, Input2);
        }
        
        [Test]
        public void InputArgumentsProvidedUsingInheritedFields()
        {
            var arguments = GetArguments(x => x.MethodWithInputs(InheritedInput1, InheritedInput2));
            AssertReturnedArguments(arguments, InheritedInput1, InheritedInput2);
        }
        
        [Test]
        public void InputArgumentsProvidedUsingMethodCallDoesNotThrow()
        {
            Assert.DoesNotThrow(() => GetArguments(x => x.MethodWithInputs(GetInput1(10), GetInput2("Test"))));
        }

        [Test]
        public void ArrayInputsArgumentsProvidedInline()
        {
            var arguments = GetArguments(x => x.MethodWithArrayInputs(new[] { 1, 2 }, new[] { "3", "4" }));
            AssertReturnedArguments(arguments, new[] {1, 2}, new[] {"3", "4"});
        }

        [Test]
        public void ArrayInputArgumentsProvidedUsingVariables()
        {
            var input1 = new[] {1, 2};
            var input2 = new[] {"3", "4"};
            var arguments = GetArguments(x => x.MethodWithArrayInputs(input1, input2));
            AssertReturnedArguments(arguments, input1, input2);
        }

        [Test]
        public void ArrayInputArgumentsProvidedUsingFields()
        {
            var arguments = GetArguments(x => x.MethodWithArrayInputs(_arrayInput1, _arrayInput2));
            AssertReturnedArguments(arguments, _arrayInput1, _arrayInput2);
        }

        [Test]
        public void ArrayInputArgumentsProvidedUsingProperty()
        {
            var arguments = GetArguments(x => x.MethodWithArrayInputs(ArrayInput1, ArrayInput2));
            AssertReturnedArguments(arguments, ArrayInput1, ArrayInput2);
        }

        [Test]
        public void ArrayInputArgumentsProvidedUsingInheritedProperty()
        {
            var arguments = GetArguments(x => x.MethodWithArrayInputs(InheritedArrayInput1, InheritedArrayInput2));
            AssertReturnedArguments(arguments, InheritedArrayInput1, InheritedArrayInput2);
        }
    }
}