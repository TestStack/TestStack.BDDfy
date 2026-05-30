using Shouldly;
using System;
using TestStack.BDDfy.Configuration;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    public class StepsWithChainedMethods
    {
        private string diagnostics;
        private SutStepBuilder _sutStepBuilder = new();
        private class SutStepBuilder
        {
            public string color { get; set; }
            public string horn { get; set; }
            public SutStepBuilder with_color(string color) { this.color = color; return this; }
            public SutStepBuilder with_horn(string horn) { this.horn = horn; return this; }
        }

        [Fact]
        public void HonkingTheHornShouldSound()
        {
            //Configurator.Processors.Add(()=> new XUnitOutputReporter());

            this.Given(_ => i_have_a_car().with_color("red").with_horn("crazy"))
                .When(_ => i_honk_the_horn())
                .Then(_ => diagnostics_will_log("red car honked crazy horn"))
                .BDDfy();
        }

        private void diagnostics_will_log(string v) => diagnostics.ShouldBe(v);

        private void i_honk_the_horn()
        {
            diagnostics = $"{_sutStepBuilder.color} car honked {_sutStepBuilder.horn} horn";
        }

        private SutStepBuilder i_have_a_car() => _sutStepBuilder;
    }
}
