using System;
using System.Collections.Generic;
using Bddify.Core;
using Bddify.Processors;
using Bddify.Scanners;
using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.BddifySpecs.RunScenarioWithArgs
{
    public class RunScenarioWithArgsContext
    {
        private ScenarioWithArgs _testObject;
        private IEnumerable<Scenario> _scenarios;

        enum ArgsNumber
        {
            ArgSet1,
            ArgSet2,
            ArgSet3,
        }

        [RunScenarioWithArgs(ArgsNumber.ArgSet1, 1, 2, 3)]
        [RunScenarioWithArgs(ArgsNumber.ArgSet2, "Test", 4, 5)]
        [RunScenarioWithArgs(ArgsNumber.ArgSet3, .2, 4)]
        private class ScenarioWithArgs
        {
            private int _input3;
            private object _input1;
            private int _input2;
            private ArgsNumber _argsNumber;

            public object Input1
            {
                get { return _input1; }
            }

            public int Input2
            {
                get { return _input2; }
            }

            public ArgsNumber ArgsNumber
            {
                get { return _argsNumber; }
            }

            public int Input3
            {
                get { return _input3; }
            }

            private void RunScenarioWithArgs(ArgsNumber argsNumber, object input1, int input2, int input3 = 100)
            {
                _input1 = input1;
                _input2 = input2;
                _input3 = input3;
                _argsNumber = argsNumber;
            }
        }

        [SetUp]
        public void Setup()
        {
            _testObject = new ScenarioWithArgs();
            _scenarios = new MethodNameScanner().Scan(_testObject);
        }

        [Test]
        public void ArgSet1IsPassed()
        {
            var runner = new TestRunner();
            var scenario = _scenarios.Single(s => (ArgsNumber)s.ArgsSet[0] == ArgsNumber.ArgSet1);
            runner.Process(scenario);
            Assert.That(_testObject.Input1, Is.EqualTo(1));
        }
    }
}