using Shouldly;
using System;
using System.IO;
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
            outputDirectory = outputDirectory is null ? null : Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), $"{outputDirectory}");
            fileWriter.OutputReport("Test content", reportName, outputDirectory);
            var expectedPath = Path.Combine(outputDirectory ?? string.Empty, reportName);
            File.Exists(expectedPath).ShouldBeTrue();
            File.ReadAllText(expectedPath).ShouldBe("Test content");
        }
    }
}