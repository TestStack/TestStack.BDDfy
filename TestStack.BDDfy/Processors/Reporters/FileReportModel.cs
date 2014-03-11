using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Processors.Reporters
{
    public class FileReportModel
    {
        public FileReportModel(IEnumerable<Story> stories)
        {
            _stories = stories;
            Summary = new FileReportSummaryModel(stories);
        }

        readonly IEnumerable<Story> _stories;
        public FileReportSummaryModel Summary { get; private set; }

        public IEnumerable<Story> Stories
        {
            get
            {
                var groupedByNamespace = from story in _stories
                                         where story.MetaData == null
                                         orderby story.Namespace
                                         group story by story.Namespace into g
                                         select g;

                var groupedByStories = from story in _stories
                                       where story.MetaData != null
                                       orderby story.MetaData.Title   
                                       group story by story.MetaData.Type.Name into g
                                       select g;

                var aggregatedStories =
                    from story in groupedByStories.Union(groupedByNamespace)
                    select new Story(
                        story.First().MetaData, // first story in the group is a representative for the entire group
                        story.SelectMany(s => s.Scenarios).OrderBy(s => s.Title).ToArray()) // order scenarios by title
                        {
                            Namespace = story.Key
                        };

                return aggregatedStories;
            }
        }
    }
}