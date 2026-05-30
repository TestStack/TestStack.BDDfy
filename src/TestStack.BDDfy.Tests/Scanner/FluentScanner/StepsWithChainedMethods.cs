using Shouldly;
using TestStack.BDDfy.Reporters;
using TestStack.BDDfy.Reporters.MarkDown;
using TestStack.BDDfy.Tests.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    public class StepsWithChainedMethods
    {
        private SutStepBuilder _sutStepBuilder = new();
        private class SutStepBuilder
        {
            public string state;
            private string color;
            private string horn;

            public SutStepBuilder with_color(string color) { this.color = color; return this; }
            public SutStepBuilder with_horn(string horn) { this.horn = horn; return this; }
            internal void at_volume(string volume) { state+= $" at {volume} volume"; }
            internal SutStepBuilder have_a_car() { state = "a"; return this; }
            internal SutStepBuilder honk_the_horn() { state+= $" {color} car honked {horn} horn"; return this; }

            internal void verify_diagnostics(string v) => state.ShouldBe(v);
        }

        [Fact]
        public void StepNamesAreBuiltFromAllChanedMethods()
        {
            var story = this.Given(_ => i().have_a_car().with_color("red").with_horn("crazy"))
                .When(_ => i().honk_the_horn())
                .Then(_ => i().verify_diagnostics("a red car honked crazy horn"))
                .LazyBDDfy();

            var result = story.Run();
            var model = new[] {result}.ToReportModel();
            var fileReport = new FileReportModel(model);
            ReportApprover.Approve(fileReport, new MarkDownReportBuilder());
        }

        [Fact]
        public void StepNameWillBeBuiltFromStepTitle()
        {
            var story = this.Given(_ => i().have_a_car().with_color("red").with_horn("crazy"), "i have a {0} car with a {1} horn")
                .When(_ => i().honk_the_horn().at_volume("high"))
                .Then(_ => i().verify_diagnostics("a red car honked crazy horn at high volume"))
                .LazyBDDfy();

            var result = story.Run();
            var model = new[] { result }.ToReportModel();
            var fileReport = new FileReportModel(model);
            ReportApprover.Approve(fileReport, new MarkDownReportBuilder());
        }

        private SutStepBuilder i() => _sutStepBuilder;
    }
}
