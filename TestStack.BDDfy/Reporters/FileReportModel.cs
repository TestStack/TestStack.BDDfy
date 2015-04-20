using System;
using System.Collections.Generic;
using System.Linq;

namespace TestStack.BDDfy.Reporters
{
    public class FileReportModel
    {
        public FileReportModel(ReportModel reportModel)
        {
            _stories = reportModel.Stories;
            Summary = new FileReportSummaryModel(reportModel);
            RunDate = DateTime.Now;
        }

        readonly IEnumerable<ReportModel.Story> _stories;
        public FileReportSummaryModel Summary { get; private set; }
        public DateTime RunDate { get; set; }

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
                    select new ReportModel.Story() // order scenarios by title
                        {
                            Namespace = story.Key
                        };

                return aggregatedStories;
            }
        }
    }
}