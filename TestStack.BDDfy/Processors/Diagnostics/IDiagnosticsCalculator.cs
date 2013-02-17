using System.Collections.Generic;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    public interface IDiagnosticsCalculator
    {
        IList<StoryDiagnostic> GetDiagnosticData(FileReportModel model);
    }
}