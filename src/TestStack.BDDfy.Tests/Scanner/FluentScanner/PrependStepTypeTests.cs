using System.Runtime.CompilerServices;
using Shouldly;
using TestStack.BDDfy.Reporters;
using Xunit;

namespace TestStack.BDDfy.Tests.Scanner.FluentScanner
{
    public class PrependStepTypeTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void VerifyPrependStepTitles()
        {
            var story = this.Given(_ => GivenAStepWithGivenInIt())
                .Given(_ => AStepWithoutGivenInIt())
                .And(_ => AndGivenAStepWithAndGivenInIt())
                .And(_ => AStepWithoutGivenInIt())
                .But(_ => ButInStep())
                .But(_ => NothingInStep())
                .When(_ => WhenStuff())
                .When(_ => Stuff())
                .Then(_ => ThenWeAreWinning())
                .Then(_ => WeAreWinning())
                .BDDfy();

            var textReporter = new TextReporter();
            textReporter.Process(story);
            textReporter.ToString().ShouldMatchApproved();
        }

        private void GivenAStepWithGivenInIt()
        {
            
        }

        private void AndGivenAStepWithAndGivenInIt()
        {
            
        }

        private void AStepWithoutGivenInIt()
        {
            
        }

        private void ButInStep()
        {
            
        }

        private void NothingInStep()
        {
            
        }

        private void WhenStuff()
        {
            
        }

        private void Stuff()
        {
            
        }

        private void ThenWeAreWinning()
        {
            
        }

        private void WeAreWinning()
        {
            
        }
    }
}