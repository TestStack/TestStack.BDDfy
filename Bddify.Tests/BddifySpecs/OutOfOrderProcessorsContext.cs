using System;
using System.Collections.Generic;
using Bddify.Core;
using Bddify.Scanners;
using Bddify.Tests.BddifySpecs.Exceptions;
using NSubstitute;
using NUnit.Framework;

namespace Bddify.Tests.BddifySpecs
{
    public class OutOfOrderProcessorsContext
    {
        private List<ProcessType> _list;

        [SetUp]
        public void Setup()
        {
            _list = new List<ProcessType>();
            var exceptionHandler = Substitute.For<IProcessor>();
            exceptionHandler.ProcessType.Returns(ProcessType.HandleExceptions);
            exceptionHandler.When(p => p.Process(Arg.Any<Bddee>())).Do(i => _list.Add(ProcessType.HandleExceptions));

            var reporter = Substitute.For<IProcessor>();
            reporter.ProcessType.Returns(ProcessType.Report);
            reporter.When(p => p.Process(Arg.Any<Bddee>())).Do(i => _list.Add(ProcessType.Report));
            
            var runner = Substitute.For<IProcessor>();
            runner.ProcessType.Returns(ProcessType.Execute);
            runner.When(p => p.Process(Arg.Any<Bddee>())).Do(i => _list.Add(ProcessType.Execute));

            var bddify = new Bddifier(new ExceptionThrowingTest<Exception>(), new GwtScanner(), new IProcessor[] { reporter, exceptionHandler, runner });
            bddify.Run();
        }

        [Test]
        public void ProcessorsAreAllCalled()
        {
            Assert.That(_list.Count, Is.EqualTo(3));
        }

        [Test]
        public void RunnerProcessorIsCalledFirst()
        {
            Assert.That(_list[0], Is.EqualTo(ProcessType.Execute));
        }

        [Test]
        public void ReporterIsCalledSecond()
        {
            Assert.That(_list[1], Is.EqualTo(ProcessType.Report));
        }

        [Test]
        public void ExceptionHandlerIsCalledLast()
        {
            Assert.That(_list[2], Is.EqualTo(ProcessType.HandleExceptions));
        }
    }
}