namespace TestStack.BDDfy.Reporters
{
    public class FileReportModel(ReportModel reportModel)
    {
        public FileReportSummaryModel Summary { get; private set; } = new FileReportSummaryModel(reportModel);
        public DateTime RunDate { get; set; } = DateTime.Now;

        public IEnumerable<ReportModel.Story> Stories
        {
            get
            {
                var withMetadata = reportModel.Stories
                    .Where(s => s.Metadata is not null)
                    .OrderBy(s => s.Metadata!.Title);

                var withoutMetadata = reportModel.Stories
                    .Where(s => s.Metadata is null)
                    .OrderBy(s => s.Namespace);

                foreach (var story in withMetadata.Concat(withoutMetadata))
                {
                    story.Scenarios.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.Ordinal));
                    yield return story;
                }
            }
        }
    }
}