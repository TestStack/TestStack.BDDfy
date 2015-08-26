# Customizing Reports
This post looks at how to customize the HTML Report and how to create your own custom reports. You can customize the HTML report via configuration or by applying custom CSS or JavaScript. You can create custom reports either by implementing a new Processor or Batch Processor and adding them into their respective pipelines (see the [Architecture Overview](/BDDfy/Customizing/ArchitectureOverview.html) for details about the differences between these).

## Customizing the HTML Report ##
The HTML report is the most sophisticated report in [BDDfy](http://teststack.github.io/pages/BDDfy.html) and therefore provides a lot more things that you can configure. Its configuration is defined by the IHtmlReportConfiguration interface.

	public interface IHtmlReportConfiguration
	{
	    string ReportHeader { get; }
	    string ReportDescription { get; }
	    string OutputPath { get; }
	    string OutputFileName { get; }
	    bool RunsOn(Story story);
	}

You can create a new configuration by implementing that interface or you can inherit from the  DefaultHtmlReportConfiguration class, used to configure the standard HTML Report, and just override specific properties.  Here is an example of a custom configuration, taken from the ATM sample, available on github [here](https://github.com/TestStack/TestStack.BDDfy/tree/master/TestStack.BDDfy.Samples/Atm).

    public class HtmlReportConfig : DefaultHtmlReportConfiguration
    {
        public override bool RunsOn(Core.Story story)
        {
            return story.MetaData.Type.Namespace != null && story.MetaData.Type.Namespace.EndsWith("Atm");
        }

        /// <summary>
        /// Change the output file name
        /// </summary>
        public override string OutputFileName
        {
            get
            {
                return "ATM.html";
            }
        }

        /// <summary>
        /// Change the report header to your project
        /// </summary>
        public override string ReportHeader
        {
            get
            {
                return "ATM Solutions";
            }
        }

        /// <summary>
        /// Change the report description
        /// </summary>
        public override string ReportDescription
        {
            get
            {
                return "A reliable solution for your offline banking needs";
            }
        }
    }

which produces the following customised report, which you will find in your bin directory named ATM.html:

![BDDfy functional decomposition](/img/BDDfy/Customizing/bddfy-sample-atm-html-custom.png)


The HTML report is a [Batch Processor](/BDDfy/Customizing/ArchitectureOverview.html) and is implemented by the HtmlReporter class. To plug the new report into BDDfy you need to create a new HtmlReporter and pass the custom configuration into its constructor. As I explained in the [Reports post](/BDDfy/Usage/Reports.html), the place to apply that configuration to BDDfy is the Configurator class, which is called before the tests run.

	Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig()));

The use of the Add method means this is adding a second HTML Report processor into the Batch Processor pipeline, so the default report runner will still run. If you actually want the new report to replace the default report, then you will also need to disable the default report.

	Configurator.BatchProcessors.HtmlReport.Disable();

## Custom CSS and JavaScript ##
You can customize a lot more about the HTML report. BDDfy uses the BDDfy.css file to style the report and BDDfy.js and jQuery to add interactivity to it. You will find these files in the bin directory alongside the HTML report. You can customise the styles by adding a bddifyCustom.css class and the behaviour by adding a bddifyCustom.js file. These files also need to be in the same directory as the HTML report file. This will affect all the reports in the project.


## Create a custom report by creating a new Processor  ##
One way to create a custom report is to implement a new Processor and plug it into the Processor pipeline. You just have to implement the one Process() method and set the Process Type to Report.

An example of doing this is provided in the BDDfy Tic Tac Toe sample project with the Custom Text Reporter. The sample is available on [github](https://github.com/TestStack/TestStack.BDDfy/tree/master/TestStack.BDDfy.Samples) or [nuget](http://nuget.org/packages/TestStack.BDDfy.Samples/).

    /// <summary>
    /// This is a custom reporter that shows you how easily you can create a custom report.
    /// Just implement IProcessor and you are done
    /// </summary>
    public class CustomTextReporter : IProcessor
    {
        private static readonly string Path;

        private static string OutputDirectory
        {
            get
            {
                string codeBase = typeof(CustomTextReporter).Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        static CustomTextReporter()
        {
            Path = System.IO.Path.Combine(OutputDirectory, "BDDfy-text-report.txt");

            if(File.Exists(Path))
                File.Delete(Path);

            var header =
                " A custom report created from your test assembly with no required configuration " +
                Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine;
            File.AppendAllText(Path, header);
        }

        public void Process(Story story)
        {
            // use this report only for tic tac toe stories
            if (!story.MetaData.Type.Name.Contains("TicTacToe"))
                return;

            var scenario = story.Scenarios.First();
            var scenarioReport = new StringBuilder();
            scenarioReport.AppendLine(string.Format(" SCENARIO: {0}  ", scenario.Title));

            if (scenario.Result != StepExecutionResult.Passed && scenario.Steps.Any(s => s.Exception != null))
            {
                scenarioReport.Append(string.Format("    {0} : ", scenario.Result));
                scenarioReport.AppendLine(scenario.Steps.First(s => s.Result == scenario.Result).Exception.Message);
            }

            scenarioReport.AppendLine();

            foreach (var step in scenario.Steps)
                scenarioReport.AppendLine(string.Format("   [{1}] {0}", step.StepTitle, step.Result));

            scenarioReport.AppendLine("--------------------------------------------------------------------------------");
            scenarioReport.AppendLine();

            File.AppendAllText(Path, scenarioReport.ToString());
        }

        public ProcessType ProcessType
        {
            get { return ProcessType.Report; }
        }
    }

This produces the BDDfy-text-report.txt text file report which is output to the bin directory:

![BDDfy custom text report](/img/BDDfy/Customizing/bddfy-custom-text-report.png)

## Create a custom report by creating a new Batch Processor  ##
While that is one way that you can create a custom report, probably the better way to do it is to implement a new Batch Processor. The Processor runs as each test is being executed and allows you to build up the report, whereas a Batch Processor has the advantage of running after all of the tests have finished, meaning that you have access to total and summary information, such as diagnostics.

As an example of creating a custom report by creating a new Batch Processor, I was recently messing around with running tests in parallel and, as you might expect, the normal console report was quite jumbled. Multiple console report Processors were writing to the console at the same time and different test results were overlapping. The solution was to run the console report after all of the tests had run by creating a new Console Reporter as a Batch Processor rather than a Processor.

    public class MyConsoleReporter : IBatchProcessor
    {
        public void Process(IEnumerable<Story> stories)
        {
            var reporter = new ConsoleReporter();
            stories
                .ToList()
                .ForEach(story => reporter.Process(story));
        }
    }

Then I just needed to add it to the Batch Processor pipeline and disable the built-in console report:

    Configurator.Processors.ConsoleReport.Disable();
    Configurator.BatchProcessors.Add(new MyConsoleReporter());


That's a bit of a hack for demo purposes. The HTML, MarkDown, and Diagnostics reports are all implemented as Batch Processors and I would recommend checking them out for examples of how to create a new report.
