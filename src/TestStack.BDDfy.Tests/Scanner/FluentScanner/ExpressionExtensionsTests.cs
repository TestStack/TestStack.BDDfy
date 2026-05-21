using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Shouldly;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
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
        private class ContainerType
        {
            public int Target { get; set; }

            public string Target2 { get; set; }

            public ContainerType SubContainer { get; set; }

            public override string ToString()
            {
                return Target2;
            }
        }

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

            public void MethodWithInputs(ContainerType subContainer)
            {

            }

            public void MethodWithNullableArg(decimal? nullableInput)
            {
                
            }

            public Bar Foo { get; set; }

            public class Bar
            {
                public void Baz()
                {
                }
            }
        }

        List<object> GetArgumentValues(Expression<Action<ClassUnderTest>> action, ClassUnderTest instance)
        {
            return action.ExtractArguments(instance).Select(o => o.Value).ToList();
        }

        List<StepArgument> GetArguments(Expression<Action<ClassUnderTest>> action, ClassUnderTest instance)
        {
            return action.ExtractArguments(instance).ToList();
        }

        int _input1 = 1;
        string _input2 = "2";
        const string ConstInput2 = "2";

        int[] _arrayInput1 = { 1, 2 };
        public string[] _arrayInput2 = { "3", "4" };

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

        ContainerType container = new();

        [Fact]
        public void NoArguments()
        {
            var arguments = GetArgumentValues(x => x.MethodWithoutArguments(), new ClassUnderTest());
            arguments.Count.ShouldBe(0);
        }

        void AssertReturnedArguments(List<object> arguments, params object[] expectedArgs)
        {
            arguments.Count.ShouldBe(expectedArgs.Length);
            for (int i = 0; i < expectedArgs.Length; i++)
            {
                arguments[i].ShouldBe(expectedArgs[i]);
            }
        }

        [Fact]
        public void InputArgumentsPassedInline()
        {
            var arguments = GetArgumentValues(x => x.MethodWithInputs(1, "2"), new ClassUnderTest());
            AssertReturnedArguments(arguments, 1, "2");
        }

        [Fact]
        public void InputArgumentsProvidedUsingVariables()
        {
            int input1 = 1;
            const string input2 = "2";
            var arguments = GetArgumentValues(x => x.MethodWithInputs(input1, input2), new ClassUnderTest());
            AssertReturnedArguments(arguments, input1, input2);
        }

        [Fact]
        public void InputArgumentsProvidedUsingFields()
        {
            var arguments = GetArgumentValues(x => x.MethodWithInputs(_input1, ConstInput2), new ClassUnderTest());
            AssertReturnedArguments(arguments, _input1, ConstInput2);
        }

        [Fact]
        public void InputArgumentsProvidedWhenCastIsInvolved()
        {
            // For some reason default(decimal) will cause a different expression when passing to a nullable method than
            // if we have input1 = 1m; No idea why...
            var input1 = default(decimal);
            var arguments = GetArguments(x => x.MethodWithNullableArg(input1), new ClassUnderTest());
            input1 = 1;
            AssertReturnedArguments(arguments.Select(a => a.Value).ToList(), input1);
        }

        [Fact]
        public void InputArgWithImplicitCast()
        {
            int input1 = 1;
            var arguments = GetArgumentValues(x => x.MethodWithNullableArg(input1), new ClassUnderTest());
            AssertReturnedArguments(arguments, input1);
        }

        [Fact]
        public void InputArgumentsProvidedUsingProperty()
        {
            var arguments = GetArgumentValues(x => x.MethodWithInputs(Input1, Input2), new ClassUnderTest());
            AssertReturnedArguments(arguments, Input1, Input2);
        }

        [Fact]
        public void InputArgumentsProvidedUsingInheritedFields()
        {
            var arguments = GetArgumentValues(x => x.MethodWithInputs(InheritedInput1, InheritedInput2), new ClassUnderTest());
            AssertReturnedArguments(arguments, InheritedInput1, InheritedInput2);
        }

        [Fact]
        public void InputArgumentsProvidedUsingMethodCallDoesNotThrow()
        {
            Should.NotThrow(() => GetArgumentValues(x => x.MethodWithInputs(GetInput1(10), GetInput2("Test")), new ClassUnderTest()));
        }

        [Fact]
        public void ArrayInputsArgumentsProvidedInline()
        {
            var arguments = GetArgumentValues(x => x.MethodWithArrayInputs(new[] { 1, 2 }, new[] { "3", "4" }), new ClassUnderTest());
            AssertReturnedArguments(arguments, new[] { 1, 2 }, new[] { "3", "4" });
        }

        [Fact]
        public void ArrayInputArgumentsProvidedUsingVariables()
        {
            var input1 = new[] { 1, 2 };
            var input2 = new[] { "3", "4" };
            var arguments = GetArgumentValues(x => x.MethodWithArrayInputs(input1, input2), new ClassUnderTest());
            AssertReturnedArguments(arguments, input1, input2);
        }

        [Fact]
        public void ArrayInputArgumentsProvidedUsingFields()
        {
            var arguments = GetArgumentValues(x => x.MethodWithArrayInputs(_arrayInput1, _arrayInput2), new ClassUnderTest());
            AssertReturnedArguments(arguments, _arrayInput1, _arrayInput2);
        }

        [Fact]
        public void ArrayInputArgumentsProvidedUsingProperty()
        {
            var arguments = GetArgumentValues(x => x.MethodWithArrayInputs(ArrayInput1, ArrayInput2), new ClassUnderTest());
            AssertReturnedArguments(arguments, ArrayInput1, ArrayInput2);
        }

        [Fact]
        public void ComplexArgument()
        {
            container.Target = 1;
            container.SubContainer = new ContainerType { Target2 = "Foo" };

            var arguments = GetArgumentValues(x => x.MethodWithInputs(container.Target, container.SubContainer.Target2), new ClassUnderTest());
            AssertReturnedArguments(arguments, 1, "Foo");
        }

        [Fact]
        public void ComplexArgumentMethodCall()
        {
            container.Target = 1;
            container.SubContainer = new ContainerType { Target2 = "Foo" };

            var arguments = GetArgumentValues(x => x.MethodWithInputs(container.Target, container.SubContainer.ToString()), new ClassUnderTest());
            AssertReturnedArguments(arguments, 1, "Foo");
        }

        [Fact]
        public void ComplexArgument2()
        {
            container.SubContainer = new ContainerType { Target2 = "Foo" };

            var arguments = GetArgumentValues(x => x.MethodWithInputs(container.SubContainer), new ClassUnderTest());
            AssertReturnedArguments(arguments, container.SubContainer);
        }

        [Fact]
        public void ComplexArgumentWhenContainerIsNull()
        {
            ContainerType nullContainer = null;
            var arguments = GetArgumentValues(x => x.MethodWithInputs(nullContainer.SubContainer), new ClassUnderTest());
            AssertReturnedArguments(arguments, new object[] { null });
        }

        [Fact]
        public void MethodCallValue()
        {
            var arguments = GetArgumentValues(x => x.MethodWithInputs(GetNumberThree(), GetFooString()), new ClassUnderTest());
            AssertReturnedArguments(arguments, new object[] { 3, "Foo" });
        }

        [Fact]
        public void DeepPropertyCall()
        {
            var arguments = GetArgumentValues(x => x.Foo.Baz(), new ClassUnderTest());
            arguments.ShouldBeEmpty();
        }

        private string GetFooString()
        {
            return "Foo";
        }

        private int GetNumberThree()
        {
            return 3;
        }

        [Fact]
        public void ArrayInputArgumentsProvidedUsingInheritedProperty()
        {
            var arguments = GetArgumentValues(x => x.MethodWithArrayInputs(InheritedArrayInput1, InheritedArrayInput2), new ClassUnderTest());
            AssertReturnedArguments(arguments, InheritedArrayInput1, InheritedArrayInput2);
        }

        [Fact]
        public void StaticField()
        {
            Should.NotThrow(() => GetArgumentValues(x => x.MethodWithInputs(GetInput1(10), GetInput2(string.Empty)), new ClassUnderTest()));
        }
    }
}