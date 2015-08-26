# Reports

BDDfy provides a rich array of reports to choose from and is also very easy to extend if you want to add your own reports. The two main reports are the Console Report and the HTML Report and both of these are on by default, though you can turn them off if you want to. In addition there is a Markdown Report, and recently a Diagnostics Report using JSON has been added. Both of these are off by default.

##The Reports
I am going to start with an overview of the different reports available in BDDfy, using the ATM sample, available in the [BDDfy source code](https://github.com/TestStack/TestStack.BDDfy/tree/master/Samples/TestStack.BDDfy.Samples/Atm), or on [NuGet][3].

###Console Report
The Console Report is what provides feedback in Visual Studio when you run your tests. If you run the tests with TestDriven.Net then you will see the output from all the tests in the output window.

![BDDfy sample test driven output](/img/BDDfy/Customizing/bddfy-sample-test-driven-output.png)

At the end of the report it will also provide a summary of how many tests passed, failed, or were skipped and how long the tests took to run.

![BDDfy sample test driven summary output](/img/BDDfy/Customizing/bddfy-sample-test-driven-summary-output.png)

If you run the tests in ReSharper then you see the output of each test individually when you select it in the Unit Test Sessions window.

![BDDfy resharper success output](/img/BDDfy/Customizing/bddfy-sample-resharper-output.png)

When the test passes, you just see the Scenario listed out, and its story if it has one. If the test fails, or is not implemented yet, then you will also see details alongside each step of which steps were executed and what their status was and an exception trace detailing the error information.

![BDDfy resharper failure output](/img/BDDfy/Customizing/bddfy-sample-resharper-exception-output.png)

###HTML Report
If you are practicing BDD, then you will probably be interested in living documentation. BDDfy can help with this with its HTML report, which dev teams can share with their customers to see the progress in a very user friendly and accessible way. Every time you run tests with BDDfy it creates an HTML report in the bin directory of the test project. The report has a summary at the top, listing out how many Stories/Scenarios have run, and the totals for each type of execution result. The report is interactive, and lets you expand and collapse individual stories and scenarios or all at once. The report is very customisable and you are able to change the header, description, and the location where the report is saved to. You can also add your own CSS and JavaScript files to really open up the customisation possibilities.

![BDDfy HTML sample report](/img/BDDfy/Customizing/bddfy-sample-atm-html.png)

###Markdown Report
The Markdown Report can be turned on using the BDDfy Configurator (more on that below). The report is written in the [GitHub Flavoured Markdown][9] format. Markdown is a really useful format for documenting (I write this blog in markdown). A possible use for this would be as part of efforts to generate documentation, which might be particularly useful for open source developers to generate wiki documents from their code, for example.

The Markdown Report is output to the bin directory of the test project and is named BDDfy.md. The picture below shows the BDDfy.md file in the MarkPad markdown editor. The left pane shows the raw text view and the pane on the right shows how it would be displayed on a web page.

![BDDfy markdown sample report](/img/BDDfy/Customizing/bddfy-sample-atm-markdown.png)

###Diagnostics Report
The Diagnostics Report is the most recent addition to the BDDfy stable. It is also off by default and can be turned on using the BDDfy Configurator. In BDDfy we can measure how long every step took to execute and then aggregate that data to see how long each Scenario and Story took to execute. This is particularly useful information if you have long running tests, such as browser-based functional tests, and want to identify the parts of the test that are having the worst impact on performance. For example, is it particularly slow when interacting with the database, or is it perhaps the rendering of the web pages?

The Diagnostics Report is created in the JSON format. This is useful if you want to load the data into another system, perhaps to persist test runs to compare performance over time.  The Diagnostics Report is output to the bin directory of the test project and is named Diagnostics.json. Here is the output for the ATM tests.

	{
	    "Stories":
	    [
	        {
	            "Name":"Account holder withdraws cash",
	            "Duration":8,
	            "Scenarios":
	            [
	                {
	                    "Name":"Account has insufficient fund",
	                    "Duration":8,
	                    "Steps":
	                    [
	                        {
	                            "Name":"Given the Account Balance is $10",
	                            "Duration":1
	                        },
	                        {
	                            "Name":"And the Card is valid",
	                            "Duration":0
	                        },
	                        {
	                            "Name":"And the machine contains enough money",
	                            "Duration":0
	                        },
	                        {
	                            "Name":"When the Account Holder requests $20",
	                            "Duration":0
	                        },
	                        {
	                            "Name":"Then the ATM should not dispense any Money",
	                            "Duration":5
	                        },
	                        {
	                            "Name":"And the ATM should say there are Insufficient Funds",
	                            "Duration":0
	                        },
	                        {
	                            "Name":"And the Account Balance should be $20",
	                            "Duration":0
	                        },
	                        {
	                            "Name":"And the Card should be returned",
	                            "Duration":0
	                        }
	                    ]
	                }
	            ]
	        }
	    ]
	}


##Configuring Reports

The Configurator class is the main configuration point for BDDfy and should be called before all your tests run if you are wanting to change the default behaviour. For example, in NUnit you could call it from the SetUpFixture.

BDDfy implements components as processors in a pipeline (using the [Chain of Responsibility pattern][11]) and reports are just another type of processor. Processors can be switched on and off using the Configurator class by calling the Enable or Disable methods. As previously mentioned, the Console Report and the HTML Report are both on by default. If you don’t want them to run then you can disable them like this:

    Configurator.Processors.ConsoleReport.Disable();
    Configurator.BatchProcessors.HtmlReport.Disable();

Similarly, you can turn on the Markdown and Diagnostics reports:

    Configurator.BatchProcessors.MarkDownReport.Enable();
    Configurator.BatchProcessors.DiagnosticsReport.Enable();

While this is great if you want to turn a processor off for all the tests, it isn’t much help if you want to just turn it on or off for some of the tests. Fortunately, there is also the the RunsOn method, which allows you to enable or disable processors using a predicate. This allows a lot of flexibility, and you could even choose to combine predicates so that, for example, half the tests ran with the Console Report and the other half ran with the Markdown Report.

    Configurator.Processors.ConsoleReport
        .RunsOn(scenario => scenario.GetType().Namespace.StartsWith("MyCompany.MyApp.Domain"));
    Configurator.Processors.MarkdownReport
        .RunsOn(scenario => !scenario.GetType().Namespace.StartsWith("MyCompany.MyApp.Domain"));

###Getting reports without running the tests
Having processors run in a pipeline leads to some interesting possibilities. One that I particularly like is that you can get all of the reports without actually running the tests. To do this you just need to turn off the TestRunner processor. The reports will still be generated, the only difference is that they will have a status of Not Executed!

    Configurator.Processors.TestRunner.Disable();

This is really useful when you want to print out the reports as documentation but don’t want to have to wait for the tests to run.
