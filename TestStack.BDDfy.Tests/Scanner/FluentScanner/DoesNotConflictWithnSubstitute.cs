﻿using NSubstitute;
using NUnit.Framework;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    public class DoesNotConflictWithnSubstitute
    {
        private ITestContext _subsitute;
        private ExampleTable _exampleTable;

        [Test]
        public void CanUseFluentApiWithNSubstitute()
        {
            this.Given(_ => GivenSomeStuff())
                .When(_ => WhenSomethingHappens())
                .Then(_ => ThenICanStillUseNSubsitute())
                .BDDfy();
        }

        private void ThenICanStillUseNSubsitute()
        {
            _subsitute.Received().Examples = _exampleTable;
        }

        private void WhenSomethingHappens()
        {
            _exampleTable = new ExampleTable();
            _subsitute.Examples = _exampleTable;
        }

        private void GivenSomeStuff()
        {
            _subsitute = Substitute.For<ITestContext>();
        }
    }
}