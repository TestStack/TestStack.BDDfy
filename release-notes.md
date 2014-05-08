##V4 In Development

####Improvements
 - [#61](https://github.com/TestStack/TestStack.BDDfy/pull/61) & [#62](https://github.com/TestStack/TestStack.BDDfy/pull/62) - rationalized BDDfy namespaces to require less namespaces for some features and to make features more discoverable. **Breaking Change**
	- Some long namespaces were removed from the framework so the API becomes more discoverable. You just need to delete the now-removed namespaces from your using statements.
	- The `Reporters` namespaces that you would use when configuring BDDfy's reports through the `Configurator` class has been moved around to the root namespace. 

 - [#63](https://github.com/TestStack/TestStack.BDDfy/pull/63) - renames a number of types to more accurately reflect their role and usage. **Breaking Change**
	- renames ExecutionStep to Step & its StepTitle to Title
	- renames StepExecutionResult to Result 
	- renames StepAction in Step to Action
	- moves RunStepWithArgs from root to StepScanners
	- renames MetaData to Metadata everywhere
	- moves StoryMetadata to Scanners folder

 - [#67](https://github.com/TestStack/TestStack.BDDfy/pull/67) - makes `StoryMetadata` generic to allow different story narratives
 - [#71](https://github.com/TestStack/TestStack.BDDfy/pull/71) - adds `ResolveJqueryFromCdn`config point for resolving jquery thru CDN or embedding it
 - [#72](https://github.com/TestStack/TestStack.BDDfy/pull/72) - minifies bddfy .css and .js files to make HTML report smaller and the source more readable 
 - [#80](https://github.com/TestStack/TestStack.BDDfy/pull/80) - removes .net 3.5 support. **Breaking Change** 
	- BDDfy V4 won't support .Net 3.5. So if you want to feel all the love that's coming to V4 you should upgrade to .Net 4+.
- [#35](https://github.com/TestStack/TestStack.BDDfy/issues/35) [#89](https://github.com/TestStack/TestStack.BDDfy/pull/89)- Full Cucumber examples support! Blog post on this new functionality at [http://jake.ginnivan.net/blog/2014/05/05/bddfy-examples-support](http://jake.ginnivan.net/blog/2014/05/05/bddfy-examples-support)
- [#69](https://github.com/TestStack/TestStack.BDDfy/issues/69) - Added tags, `.WithTags("Tag1", "Tag2")`. Tags will show in BDDfy reports
- [#121](https://github.com/TestStack/TestStack.BDDfy/issues/121) - Fluent API: Prepend step type to Title, for example `.Given(_ => Foo())` will report as `Given foo`
- Step title arguments in fluent API are reported much better, including:
	- Step arguments are evaluated lazily, meaning if previous steps cause the value to change, the value at the time of step executed will be reported in the step
	- [#137](https://github.com/TestStack/TestStack.BDDfy/issues/137) - Method call arguments are not shown in step title
	- Support fields, properties as well as local variables passed as arguments as steps
	- And a few other cases have been addressed making steps much more resilient
- [#118](https://github.com/TestStack/TestStack.BDDfy/issues/118) - Removed story.Category **Breaking Change**
- [#101](https://github.com/TestStack/TestStack.BDDfy/issues/101) - Step titles are not encoded in HTML reports
- [#87](https://github.com/TestStack/TestStack.BDDfy/issues/87) - Fluent API no longer orders steps by type of step, and is opened up to allow any ordering of steps (as long as the first step is Given or When. For example `.Given.Then.When.Then.And.When.Then` can be done.
- [#78](https://github.com/TestStack/TestStack.BDDfy/issues/78) - Metro HTML report, new report which is a bit more modern. It can be enabled via configuration.
- [#75](https://github.com/TestStack/TestStack.BDDfy/issues/75) - Inline assertions, allowing inline blocks of code - `.Given(() => { .... }, "Given some stuff")`
- [#82](https://github.com/TestStack/TestStack.BDDfy/issues/82) - Strong naming has been removed **Breaking Change**

####New Features
 - [#81](https://github.com/TestStack/TestStack.BDDfy/pull/81) - adds ability to do inline assertions and title only steps using the fluent API

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.19.1...master)

###v3.19.1 - 2014-03-17
####Improvements
 - [#51](https://github.com/TestStack/TestStack.BDDfy/pull/51) - makes html report self-contained
 - [#59](https://github.com/TestStack/TestStack.BDDfy/pull/59) - `TestObject` is now set to null in `StoryCache` to release unused objects

####Bugs
 - [#53](https://github.com/TestStack/TestStack.BDDfy/pull/53) - fixes md report on null story
 - [#59](https://github.com/TestStack/TestStack.BDDfy/pull/59) - fixes diagnostics report crash: it now resolves the name using the new Namespace if Metadata is null 
 
####New Features
 - [#54](https://github.com/TestStack/TestStack.BDDfy/pull/54) - allows for avoiding duplicate text in StoryAttribute 

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.18.5...v3.19.1)

###v3.18.5 - 2014-02-03
####Improvements
 - [#46](https://github.com/TestStack/TestStack.BDDfy/pull/46) - tidies up the layout of the summary pane in the HTML report

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.18.3...v3.18.5)

###v3.18.3 - 2014-01-24
####Bugs
 - [#42](https://github.com/TestStack/TestStack.BDDfy/pull/42) - fixes a bug that caused html report to fail when there is a BDDfy test without an associated story

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.18.2...v3.18.3)

###v3.18.2 - 2014-01-13
####Improvements
 - [#40](https://github.com/TestStack/TestStack.BDDfy/pull/40) - reduces memory footprint by disposing unnecessary objects in StoryCache

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.18.0...v3.18.2)

###v3.18.0 - 2014-01-02
####New Features
 - [#38](https://github.com/TestStack/TestStack.BDDfy/pull/38) - adds a configuration point to stop execution pipeline on a failing test

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.17.1...v3.18.0)

###v3.17.1 - 2013-11-04
####New Features
 - [#32](https://github.com/TestStack/TestStack.BDDfy/pull/32) - adds support for async steps

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.16.14...v3.17.1)

###v3.16.14 - 2013-08-05
####Improvements
 - [#30](https://github.com/TestStack/TestStack.BDDfy/pull/30) - cleans up samples and adds a simple BDDfy Rocks one
 - [#28](https://github.com/TestStack/TestStack.BDDfy/pull/28) - adds nuget tags 
 - [#27](https://github.com/TestStack/TestStack.BDDfy/pull/27) - updates nuspec files with the latest license (MIT) & project url
 - [#26](https://github.com/TestStack/TestStack.BDDfy/pull/26) - removes nuget readme content file 
 - [#24](https://github.com/TestStack/TestStack.BDDfy/pull/24) - removes some text from readme & pointed to the new docos website
 - [#23](https://github.com/TestStack/TestStack.BDDfy/pull/23) - changes the license to MIT

####Bugs
 - [#25](https://github.com/TestStack/TestStack.BDDfy/pull/25) - changes the html report link & fixes the custom scripts
 - [#22](https://github.com/TestStack/TestStack.BDDfy/pull/22) - renames custom script names and fixes the report link - Fixes issue #18

[Commits](https://github.com/TestStack/TestStack.BDDfy/compare/v3.16.5...v3.16.14)

###v3.16.5 - 2013-07-31
####New Features
 - [#16](https://github.com/TestStack/TestStack.BDDfy/pull/16) - new optional diagnostics report and report refactoring
