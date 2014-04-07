###In development
####Improvements
 - [#61](https://github.com/TestStack/TestStack.BDDfy/pull/61), [#62](https://github.com/TestStack/TestStack.BDDfy/pull/62) - rationalized BDDfy namespaces to require less namespaces for some features and to make features more discoverable. Breaking change.
 - [#63](https://github.com/TestStack/TestStack.BDDfy/pull/63) - renamed a number of types to more accurately reflect their role and usage. Breaking change.
 - [#64](https://github.com/TestStack/TestStack.BDDfy/pull/64), [#65](https://github.com/TestStack/TestStack.BDDfy/pull/65) - cleans up NuGet packages and sets up automatic restore.
 - [#67](https://github.com/TestStack/TestStack.BDDfy/pull/67) - makes StoryMetadata generic to allow different story narratives.
 - [#71](https://github.com/TestStack/TestStack.BDDfy/pull/71) - config point for resolving jquery thru CDN or embedding it so as to remove dependency on jQuery.
 - [#72](https://github.com/TestStack/TestStack.BDDfy/pull/72) - minifies bddfy .css and .js files to make HTML report source more readable.
 - [#80](https://github.com/TestStack/TestStack.BDDfy/pull/80) - removed .net 3.5 support. Breaking change.

####New Features
 - [#81](https://github.com/TestStack/TestStack.BDDfy/pull/81) - ability to do inline assertions using the fluent API.

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
