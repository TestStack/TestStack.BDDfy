using System.Runtime.CompilerServices;
using Shouldly;
using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy.Tests
{
    public class Startup
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            Configurator.Processors.Add(() => new XUnitOutputReporter());
            Configurator.Processors.ConsoleReport.Enable();

            ShouldlyConfiguration.ShouldMatchApprovedDefaults.WithFilenameGenerator((testMethodInfo, discriminator, type, extension) =>
            {
                var tfm = $"net{Environment.Version.Major}.{Environment.Version.Minor}";
                var receivedFolder = Path.Combine(AppContext.BaseDirectory, "ApprovalTests");
                Directory.CreateDirectory(receivedFolder);

                var baseName = $"{testMethodInfo.DeclaringTypeName}.{testMethodInfo.MethodName}";
                if (!string.IsNullOrEmpty(discriminator))
                    baseName += $"_{discriminator}";

                return type == "received"
                    ? Path.Combine(receivedFolder, $"{baseName}.{type}.{extension}")
                    : $"{testMethodInfo.SourceFileDirectory}{Path.DirectorySeparatorChar}{baseName}.{type}.{extension}";
            });
        }
    }
}