using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Reporters
{
    public class FileReportModel(ReportModel reportModel)
    {
        readonly IEnumerable<ReportModel.Story> _stories = reportModel.Stories;
        public FileReportSummaryModel Summary { get; private set; } = new FileReportSummaryModel(reportModel);
        public DateTime RunDate { get; set; } = DateTime.Now;

        public IEnumerable<ReportModel.Story> Stories
        {
            get
            {
                var groupedByNamespace = from story in _stories
                                         where story.Metadata == null
                                         orderby story.Namespace
                                         group story by story.Namespace into g
                                         select g;

                var groupedByStories = from story in _stories
                                       where story.Metadata != null
                                       orderby story.Metadata.Title   
                                       group story by story.Metadata.Type.Name into g
                                       select g;

                var aggregatedStories =
                    from story in groupedByStories.Union(groupedByNamespace)
                    select new ReportModel.Story() 
                        {
                            Metadata = story.First().Metadata,
                            Namespace = story.Key,
                            Result = story.First().Result,
                            Scenarios = story.SelectMany(s => s.Scenarios).OrderBy(s => s.Title).ToList() // order scenarios by title,
                        };

                return aggregatedStories;
            }
        }
    }
}