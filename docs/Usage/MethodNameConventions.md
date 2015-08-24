# Method Name Conventions

BDDfy can scan your tests in one of two ways: using Reflective API and Fluent API.  Reflective API uses some hints to scan your classes and afterwards pretty much all the burden is on BDDfy's shoulders to find your steps, make sense of them and execute them in order. You can provide these hints in two ways: using method name conventions and/or attributes. For this post we will only concentrate on method name conventions.

BDDfy uses a bit of magic to figure out what your scenario looks like and what it should execute. Despite the magic behind the scenes, using the BDDfy API is extremely simple - it boils down to 14 letters:

    this.BDDfy();

That is all the API you need to know to be able to use BDDfy in Reflective Mode. Well, that and a bit of knowledge about the conventions described below.

##A class per scenario
In the reflective mode BDDfy associates each class with a scenario and you will basically end up with one class per scenario.

Some developers like the Single Responsibility Principle forced nature of this approach and some do not. For those who think this is not very DRY (Don't Repeat Yourself) BDDfy allows you to take full control over this using the Fluent API. I personally use both approaches in every project because each has its pros and cons.

A typical example of using method name convention looks like:

<pre>
using NUnit.Framework;

namespace BDDfy.MethodNameConventions
{
    public class BDDfyRocks
    {
        [Test]
        public void ShouldBeAbleToBDDfyMyTestsVeryEasily()
        {
            this.BDDfy();
        }

        void GivenIHaveNotUsedBDDfyBefore()
        {
        }

        void WhenIAmIntroducedToTheFramework()
        {
        }

        void ThenILikeItAndStartUsingIt()
        {
        }
    }
}
</pre>

The only thing related to BDDfy in this class is <code>this.BDDfy();</code>!!

As mentioned in the [index page](/BDDfy/index.html), BDDfy does not care what testing framework or test runner you use and it provides the same result for all of them. Here is the result of the test run using R#:

![Resharper Result](/img/BDDfy/method-name-conventions/Resharper-result.JPG)

Using that one line of code BDDfy was able to find out what your scenario title and test steps are and how to run them! It also provides the above console report and an html report as below:

![Html report](/img/BDDfy/method-name-conventions/html-report.JPG)

##How does BDDfy do all that?
When using the reflective mode, BDDfy scans your class (which is <code>this</code> you are calling <code>BDDfy()</code> on) and finds all the methods in it. It then adds all the methods which match its conventions to a list. After having gone through the class (and its base classes), it loops over the methods, executes them, and then generates a report.

Here is the complete list of the out of the box conventions. The method name:

 * ending with "Context" is considered as a setup method (not reported).
 * "Setup" is considered as as setup method  (not reported).
 * starting with "Given" is considered as a setup method (reported).
 * starting with "AndGiven" is considered as a setup method that runs after Context, Setup and Given steps (reported).
 * starting with "When" is considered as a transition method  (reported).
 * starting with "AndWhen" is considered as a transition method that runs after When steps (reported).
 * starting with "Then" is considered as an asserting method (reported).
 * starting with "And" is considered as an asserting method (reported).
 * starting with "TearDown" is considered as a finally method which is run after all the other steps (not reported).

Some of these special conventions will lead to the step not being reported. For example if your method name ends with the word 'Context' the step will be picked up by the framework and will be executed; but it will not be reported in console or html report. This was created on a request by a user; but I personally do not use this feature. If I need to setup my state I either do it in the 'Given' steps or in the class constructor if it is not directly related to the scenario state.

It is worth mentioning that these conventions can be easily overridden if your needs require further customisation.

BDDfy by default uses your scenario class name to generate a title for your scenario, however you can easily override this behaviour as we will see further down.

##Another example
Let's expand on the example above and create something a bit more complex. My specification this time reads as:

<pre>
Given I am new to BDD
  And I have not used BDDfy before
When I am introduced to the framework
Then I like it and I start using it
  And I learn BDD through BDDfy
</pre>

Not much difference to what I had before; but now I have two additional 'And' steps: one for 'Given' and one for 'Then'. Going by the conventions explained above you should implement this like below:

<pre>
using NUnit.Framework;

namespace BDDfy.MethodNameConventions
{
    public class BDDfyRocksEvenForBddNewbies
    {
        [Test]
        public void ShouldBeAbleToBDDfyMyTestsVeryEasily()
        {
            this.BDDfy();
        }

        void GivenIAmNewToBdd()
        {
        }

        void AndGivenIHaveNotUsedBDDfyBefore()
        {
        }

        void WhenIAmIntroducedToTheFramework()
        {
        }

        void ThenILikeItAndStartUsingIt()
        {
        }

        void AndILearnBddThroughBDDfy()
        {
        }
    }
}
</pre>

Let's run this. This time I use [TD.Net](http://www.testdriven.net/) to show you the result from another test runner:

![TD.Net result of the expanded test](/img/BDDfy/method-name-conventions/TDNet-expanded-test-result.JPG)

So BDDfy was capable to find the 'AndGiven' method and turn it into an 'And' step that runs after the 'Given' step. The same goes for the 'And' method that is run after the 'Then' step.

##How to use input arguments with method name conventions?
If your test requires input arguments there is a good chance you should be using the fluent API; that said BDDfy provides support for input arguments for the method name convention scanner too.

In order to run the same scenario using different input arguments you need to create a scenario class which is not a test class. The scenario class should accept the input arguments through constructor parameters and then you may assign those to instance fields and use them in your step methods. You then will have another class, which will usually be your story class, to instantiate your scenario class using different input arguments and call BDDfy on the instance. It is hard to explain this and an example shows the usage better.

BDDfy comes with two complete examples that showcase different most BDDfy features. You may install these samples through <code>BDDfy.Samples.TicTacToe</code> and <code>BDDfy.Samples.ATM</code>. This particular feature is used in the [TestStack.BDDfy.Samples](http://nuget.org/packages/TestStack.BDDfy.Samples) sample for testing the winner games. You may see the winner game scenario class [here](https://github.com/TestStack/TestStack.BDDfy/blob/master/TestStack.BDDfy.Samples/TicTacToe/WinnerGame.cs). For brevity I only include the class constructor here:

<pre>
public class WinnerGame : GameUnderTest
{
    private readonly string[] _firstRow;
    private readonly string[] _secondRow;
    private readonly string[] _thirdRow;
    private readonly string _expectedWinner;

    public WinnerGame(string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner)
    {
        _firstRow = firstRow;
        _secondRow = secondRow;
        _thirdRow = thirdRow;
        _expectedWinner = expectedWinner;
    }
</pre>

... and the code from the story that instantiates the class and runs it using different input arguments can be found [here](https://github.com/TestStack/TestStack.BDDfy/blob/master/TestStack.BDDfy.Samples/TicTacToe/TicTacToe.cs#L93). A bit of code copied from that codebase is shown below for your convenience:

<pre>
[Test]
[TestCase("Vertical win in the right", new[] { X, O, X }, new[] { O, O, X }, new[] { O, X, X }, X)]
[TestCase("Vertical win in the middle", new[] { N, X, O }, new[] { O, X, O }, new[] { O, X, X }, X)]
[TestCase("Diagonal win", new[] { X, O, O }, new[] { X, O, X }, new[] { O, X, N }, O)]
[TestCase("Horizontal win in the bottom", new[] { X, X, N }, new[] { X, O, X }, new[] { O, O, O }, O)]
[TestCase("Horizontal win in the middle", new[] { X, O, O }, new[] { X, X, X }, new[] { O, O, X }, X)]
[TestCase("Vertical win in the left", new[] { X, O, O }, new[] { X, O, X }, new[] { X, X, O }, X)]
[TestCase("Horizontal win", new[] { X, X, X }, new[] { X, O, O }, new[] { O, O, X }, X)]
public void WinnerGame(string title, string[] firstRow, string[] secondRow, string[] thirdRow, string expectedWinner)
{
    new WinnerGame(firstRow, secondRow, thirdRow, expectedWinner).BDDfy(title);
}
</pre>

This runs the <code>WinnerGame</code> test class as several scenarios with different inputs. The html report from the sample is shown below:

![Tic Tac Toe html report](/img/BDDfy/method-name-conventions/tictactoe-html-result.JPG)

<small>The report has a story which I have not covered yet.</small>

So far we have been calling <code>BDDfy()</code> with no arguments so you may wonder what the <code>title</code> argument does. As you may guess from its name that argument overrides the scenario title. If we had not passed that argument in we would end up with 7 scenarios all titled 'Winner game' which is not what we want. So we pass in the title we want for the scenario based on the input arguments.

##FAQ
These are some of the FAQs I have received for Method Name Conventions:

#####Should I have my methods in the right order?
Ordinarily, no. BDDfy picks the methods based on the naming convention and regardless of where in the class they appear BDDfy runs and reports them in the right order. However, if you have multiple 'AndGiven', 'AndWhen', or 'And' steps you need to put these methods in the order that you want BDDfy to pick them up.

If for some reason you are using both a Setup method and an xxxContext method to perform some setup then these will run in the order in which they are defined in your class. It is probably a better idea to refactor these into a single method if possible.

#####How I can reuse some of the testing logic?
You may achieve that through scenario inheritance or composition as you would in your business logic code.

When inheriting from a base class that has a few steps BDDfy picks the steps from your base classes as if they were in your scenario class. This is useful when you have several scenarios that share a few steps. This way you put the shared steps in the base class and subclass that in your scenario classes.

Using composition you may put the actual logic in a separate class and use them from your scenario classes. If you are using composition then you may want to consider the fluent API because it does just what you want. I will discuss them in another post in near future.

#####Why does not BDDfy pick up my base class methods?
Because you should define them either as public or protected. BDDfy ignores the base class methods with private access modifier.

#####Can my step methods be static or should they be instance methods?
BDDfy handles both cases. So feel free to use whatever makes sense.

#####Where can I setup my mocks or other bits not directly related to the scenario?
When unit testing you usually end up mocking a few interfaces and setting up a few things that are not necessarily related to the scenario under test, but are necessary for you to be able to test the scenario. I usually put this logic into the class constructor. If what you are setting up is directly related to the scenario then you should put the logic in your 'Given' step(s).
