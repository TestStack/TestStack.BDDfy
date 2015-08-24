# Architecture Overview
## Introduction ##
This post provides an overview of the main components of the BDDfy architecture to provide some context for the rest of this section and to illustrate the extensibility points.

![BDDfy functional decomposition](/img/BDDfy/Customizing/bddfy-functional-decomposition.png)

The unit of operation in BDDfy is the Story. A Story has metadata (information about the Story) and a collection of Scenarios. Each Scenario represents a test class and contains metadata and a collection of Execution Steps, which are the methods on the test class. There are three types of architectural components in BDDfy: Scanners, Processors and Batch Processors. For each test class BDDfy composes a Story unit with various Scanners and passes it to the Processors in a processor pipeline. Once all of the test classes have been scanned and processed the Batch Processors run aggregate operations against all of the Stories.

Scanners turn a call to BDDfy (from a method) into a Scenario which could potentially be related to a Story. BDDfy doesn't need Stories but if there is one it uses it. If a Scenario is not related to a Story then it is associated with a dummy placeholder. Each Story is then passed to the Processors, which perform various operations, including executing the tests, and populate the Stories, Scenarios and Steps with the test execution results. Once all of the tests have been scanned and processed, the Batch Processors take the collection of Stories and process their results. This could be any sort of aggregate operation, but currently all the batch processors are reports.

## Scanners ##
Most of the BDDfy in Action series so far has covered the various Scanners, so I won’t go into much detail here. Suffice to say, BDDfy uses Scanners to scan each test class to find all of the methods on it and turn the test class into a Scenario. The different Scanners are shown here:

![BDDfy scanners](/img/BDDfy/Customizing/bddfy-scanners.png)

### Story Scanner ###
BDDfy creates a Story Scanner for each test object. This is the Scanner that actually scans the test object and turns it into a Story. It composes together the Story Metadata Scanner and the appropriate Scenario Scanner – [Fluent](/BDDfy/Usage/FluentAPI.html) or [Reflective](/BDDfy/Usage/MethodNameConventions.html).

The Story Metadata Scanner gets information from the Story attribute, if one exists on the class.

A **Story** has the following properties:

- **Story Metadata**: Information about the Story such as Title, As a, I want, So that
- **Scenarios**: The collection of Scenarios related to the Story
- **Result**: A Story’s Result is a Step Execution Result and is determined by the highest Step Execution Result of its Scenarios.
- **Category**: The Story Category

**Step Execution Results** have a numerical hierarchy and can be (in ascending order):

- Not Executed (0)
- Passed (1)
- Not Implemented (2)
- Inconclusive (3)
- Failed (4)

The Test Runner Processor assigns a numerical Step Execution Result to every Execution Step. The result of a Scenario is then determined by the highest value of from its Steps and the result of a Story is determined by the highest result of its Scenarios. For example, if a Step fails, then its parent Scenario and Story will also have a result of Failed.

### Scenario Scanners ###
Scenario Scanners scan the test class and use the information they find to create a Scenario. There is a Fluent Scenario Scanner and a Reflective Scenario Scanner.

A **Scenario** has the following properties:

- **Title**: The Scenario Title
- **Steps**: The collection of Steps (test class methods) related to the Scenario
- **Result**:	A Scenario’s Result is a Step Execution Result and is determined by the highest Step Execution Result of its Steps.
- **Duration**: How long the Scenario took to execute. Used by Diagnostics.

### Step Scanners ###
Step Scanners turn methods into Execution Steps. The Reflective Scanners (the Executable Attribute Step Scanner and the Method Name Scanner) scan the test class to find all the methods on it and turns them into Execution Steps. The Fluent Step Scanner is only a registry and in practice doesn't do any scanning.

An **Execution Step** has the following properties:

- **Title**: The Step Title
- **Result**: The result of executing the Step.
- **Duration**: How long the Step took to execute. Used by Diagnostics.
- **Asserts**: Whether or not the Step is an Assertion Step.
- **Should Report**: Whether the Step should be displayed in reports.
- **Execution Order**: The order that the step should run in relative to the other steps. Can be (in ascending order)
   - Initialize (for example, "Context", "Setup)
   - Setup State ("Given")
   - Consecutive Setup State ("And Given")
   - Transition ("When)
   - Consecutive Transition "And When")
   - Assertion ("Then")
   - Consecutive Assertion ("And Then")
   - Tear Down ("TearDown")
 
## Processors ##
Once a test class has been scanned into a Story, the Story is passed into a **Processor pipeline** where a series of processing steps are performed on it. The Processors are categorized by Type and the order they run in is determined by this Type.

The various **Process Types**, in order, are:

1. Firstly
1. Execute
1. Before Report
1. Report
1. After Report
1. Process Exceptions
1. Finally

![BDDfy processor pipeline](/img/BDDfy/Customizing/bddfy-processor-pipeline.png)


1. **Test Runner (Execute)**: Executes the tests.
1. **Console Reporter (Report)**: Displays the test result in the console.
1. **Exception Processor (Process Exceptions)**:	Handles exceptions.
1. **Story Cache (Finally)**: Saves each Story for later processing by the Batch Processors.
1. **Disposer	(Finally)**: Cleans up the Story and its Scenarios.

## Batch Processors ##
Once all of the tests have been scanned and processed, the Batch Processors take all of the Stories and process their results (technically speaking they run in the [AppDomain DomainUnload event](http://msdn.microsoft.com/en-GB/library/system.appdomain.domainunload.aspx)). This could be any sort of result processing, but currently all the batch processors are reports. The built-in Batch Processors are displayed in the diagram below.

![BDDfy batch processor pipeline](/img/BDDfy/Customizing/bddfy-batch-processor-pipeline.png)

- **HTML Reporter**: Creates the HTML report
- **Markdown Reporter**: Creates the Markdown report
- **Diagnostics Reporter**: Creates the Diagnostics report

## Configurator ##
The static Configurator class allows you to configure Scanners, Processors and Batch Processors. It lets you enable,  disable, or replace individual components and it also allows you to add custom implementations.
