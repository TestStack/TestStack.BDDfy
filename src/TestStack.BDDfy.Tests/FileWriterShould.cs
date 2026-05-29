using System;
using Shouldly;
using TestStack.BDDfy.Reporters.Writers;
using Xunit;

namespace TestStack.BDDfy.Tests
{
    public sealed class FileWriterShould
    {
        [Theory]
        [InlineData("report.txt", null)]
        [InlineData("./report.txt", null)]
        [InlineData("reports/report.txt", null)]
        [InlineData("./reports/report.txt", null)]        
        [InlineData("report.txt", "TestDirectory")]
        [InlineData("./reports/report.txt", "TestDirectory")]
        [InlineData("./reports/report.txt", "TestDirectory/Reports")]
        public void CreatePathIfItDoesNotExist(string reportName, string outputDirectory)
        {
            var fileWriter = new FileWriter();
            outputDirectory = outputDirectory is null ? null : System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString(), $"{outputDirectory}");
            fileWriter.OutputReport("Test content", reportName, outputDirectory);
            var expectedPath = System.IO.Path.Combine(outputDirectory ?? string.Empty, reportName);
            System.IO.File.Exists(expectedPath).ShouldBeTrue();
            System.IO.File.ReadAllText(expectedPath).ShouldBe("Test content");
        }
    }
}