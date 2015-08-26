# Story
In this post we will discuss how you can add story support to your BDD behaviors. As mentioned before and as we saw in the post about [Method Name Conventions](/BDDfy/MethodNameConventions.html) BDDfy does not force you to use stories. This could be quite useful for teams that do not work in an Agile environment. Forcing developers to come up with a story definition, while I believe is useful in many cases, could be less than optimal in some situations. For this reason you can `BDDfy` a scenario without associating it with a story; but that is more of an exception than a rule. So let's see how you can create stories and associate them with some scenarios.

##How to create a story definition?
In BDDfy for everything you want to do there are several options; there is one exception to this and that is defining stories. There is only one way to define a story and it is quite simple: to define a story all you need to do is to decorate a class, any class anywhere in your solution, with a `StoryAttribute`. Doing so creates a story that you can then associate with your scenarios. Here is an example of a story:

<pre>
namespace BDDfy.Story
{
    [Story(
        AsA = "As a .net programmer",
        IWant = "I want to use BDDfy",
        SoThat = "So that BDD becomes easy and fun")]
    public class BDDfyRocks
    {
    }
}
</pre>

All I have here is a class decorated with a `Story` attribute. By decorating this class you have setup your story metadata once and forever so you will not have to repeat this info for every scenario.

##So how do I associate a story with a scenario?
There are two ways to achieve this:

###1. Let BDDfy find the association!
BDDfy can associate a story with a scenario if the scenario is BDDfied in a method defined in the story class.

Let's write a scenario:

<pre>
namespace BDDfy.Story
{
    public class ShouldBeAbleToBDDfyMyTestsVeryEasily
    {
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

Please note that there is nothing related to BDDfy in this class. It is just a Plain Old C# Class which will eventually have some assertions in it. I can then BDDfy this scenario from my story class like:

<pre>
using BDDfy.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.Story
{
    [TestClass]
    [Story(
        AsA = "As a .net programmer",
        IWant = "I want to use BDDfy",
        SoThat = "So that BDD becomes easy and fun")]
    public class BDDfyRocks
    {
        [TestMethod]
        public void LetBDDfyFindTheStory()
        {
            new ShouldBeAbleToBDDfyMyTestsVeryEasily().BDDfy();
        }
    }
}
</pre>

There are a few changes here:

 - I added MSTest and BDDfy namespaces on the top.
 - Decorated the story class with TestClass. This is an MSTest requirement.
 - Created a new method which is my scenario and instantiated and bddified my scenario from within the method.

Let's run our only test with R#:

![Let BDDfy find the story](/img/BDDfy/story/let-BDDfy-find-the-story.JPG)

If you compare this to the similar test we ran in the previous post you notice that this report shows a story on the top. The story details are picked up from the `StoryAttribute` on the class.

And as you would expect the story details will appear in the html report too:

![Html report with story](/img/BDDfy/story/separate-story-html-report.JPG)

In the html report scenarios will be categorized by their stories. You can also expand or collapse them by clicking on them or by clicking on the expand all and collapse all which will expand and collapse them all respectively.

... but how does BDDfy know how to associate the story with the scenario? At runtime BDDfy walks up the stack trace until it finds your test method and then finds the declaring class and checks to see if it is decorated with a `StoryAttribute` and if yes it associates the two. This brings us to the next approach of associating the stories and scenarios which is the recommended approach; but before going further I would like to ask you to read [this article](http://www.mehdi-khalili.com/that-tricky-stacktrace) about the intricacy of stack trace. I will wait here until you read that.

Read it?! At runtime JIT may decide to flatten a few method calls and for that reason BDDfy may or may not be able to find your story class. Basically if you are running or intending to ever run your tests in release mode then you must use the second approach.

###2. Tell BDDfy which story to use!
If you may run your tests in release mode, to avoid disappointment, you may want to explicitly associate a story with a scenario. This approach has some other advantages that I will explain shortly.

In order to specify the story you should use an overload of `BDDfy` method which accepts a type argument for story. Here is the same example but using this overload:

<pre>
using BDDfy.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.Story
{
    [TestClass]
    [Story(
        AsA = "As a .net programmer",
        IWant = "I want to use BDDfy",
        SoThat = "So that BDD becomes easy and fun")]
    public class BDDfyRocks
    {
        [TestMethod]
        public void TellBDDfyWhatStoryToUse()
        {
            new ShouldBeAbleToBDDfyMyTestsVeryEasily().BDDfy&lt;BDDfyRocks&gt;();
        }
    }
}

</pre>

The only difference here is that I am passing `BDDfyRocks` type as a type argument to the `BDDfy()` method. This runs the very same steps and provides the very same report as we saw above; except that this method is guaranteed to find the story regardless of your build configuration or CPU architecture.

##How can I override the story title?
By default BDDfy turns your story class name into the title for the story which  appears in the reports. For example `BDDfyRocks` is turned into 'BDDfy rocks'. For what it is worth, the same logic is used to drive scenario and step titles.

In the previous post we overrode a scenario title by passing the custom title into the `BDDfy()` method; but how can we override the story title? It is very simple: `StoryAttribute` has a `Title` property that you can set. If you leave that property alone BDDfy uses your story class name for the title; but if you set it, that value is used instead.

As an example to override the title of the `BDDfyRocks` story we can set the title as follows:

<pre>
[Story(
   Title = "Setting the story title is very easy",
   AsA = "As a .net programmer",
   IWant = "I want to use BDDfy",
   SoThat = "So that BDD becomes easy and fun")]
public class BDDfyRocks
{
}
</pre>

##How can I reuse the same story for scenarios in different projects?
This is the only question I get asked every now and then about using stories. This usually happens when there are more than one test project in the solution and two tests/behaviors in two different projects happen to be related to the same story. This is where the second approach shines. When specifying the story in the `BDDfy()` the framework does not really care whether your scenario is being run within the story or is in the same class or in the same project. It is happy as long as it can see the story (which means as long as your code compiles).

As an example the same scenario above could be written as (assuming that `AScenarioRunFromAnotherProject` lives in a different project):

<pre>
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.Story
{
    [TestClass]
    public class AScenarioRunFromAnotherProject
    {
        [TestMethod]
        public void TellBDDfyWhatStoryToUse()
        {
            new ShouldBeAbleToBDDfyMyTestsVeryEasily().BDDfy&lt;BDDfyRocks&gt;();
        }
    }
}
</pre>

This works the exact same way as if the story is defined in the same project or on the same class.
