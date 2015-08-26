# Executable Attributes

Before I start I should mention that this post is going to look rather similar to the one about [Method Name Conventions](/BDDfy/MethodNameConventions.html) because these two methods are similar in nature. Also the samples provided in this post are by no means perfect BDD samples and are used only to demonstrate the API usage.

As mentioned in the previous post BDDfy can scan your tests in one of two ways: using Reflective API and Fluent API. Reflective API uses some hints to scan your classes. You can provide these hints in two ways: using method name conventions and/or attributes. We have discussed [method name conventions](/BDDfy/MethodNameConventions.html) before and in this post we will only concentrate on `ExecutableAttribute`.

In the reflective mode you could use Method Name Conventions to provide BDDfy with some hints about your test steps. That is all good; but in some cases you may need to explicitly specify your test steps and/or their title (or at least some of them):

##You may need more control over the step title
When using Method Name Conventions BDDfy uses your method name to generate the step title. This works in a lot cases; but in some cases you may need more control over your step title. For example your step title may need to have special characters; e.g. "Given the ATM doesn't have enough cash". For BDDfy to be able to derive this your method should be called "GivenTheAtmDoesn'tHaveEnoughCash" which of course is not a valid method name. Even if that was a valid method name the result would be "Given the atm doesn't have enough cash" where 'atm' is all lower case!

##What if Method Name Conventions do not make sense?
In vast majority of cases method name conventions make your methods and tests more readable and maintainable; but there may be cases when you want to use different method names because they may read better. As an example you may have a scenario that is written as:

<pre>
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.ExecutableAttributes
{
    [TestClass]
    public class NonCompliantMethodNames
    {
        [TestMethod]
        public void ShouldBeAbleToRunScenariosWithNonCompliantMethodNames()
        {
            this.BDDfy();
        }

        void GivenSomeMethodsDoNotComplyWithBDDfyMethodNameConventions()
        {
        }

        void WhenExecutableAttributeIsApplied()
        {
        }

        void BDDfyCanPickupTheSteps()
        {
        }

        void ThatAreDecoratedWithTheAttributes()
        {
        }
    }
}
</pre>

The last two methods, which are my 'Then' and 'And then' steps, do not comply with method name conventions and as such will not be picked up by BDDfy!

##So what are ExecutableAttributes?
In cases when method name conventions do not cut it for you, you may use `ExecutableAttribute` to nominate any method as a step. So let's just change the above example so it works:

<pre>
using BDDfy.Core;
using BDDfy.Scanners.StepScanners.ExecutableAttribute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.ExecutableAttributes
{
    [TestClass]
    public class NonCompliantMethodNamesWithExecutableAttributes
    {
        [TestMethod]
        public void ShouldBeAbleToRunScenariosWithNonCompliantMethodNames()
        {
            this.BDDfy();
        }

        void GivenSomeMethodsDoNotComplyWithBDDfyMethodNameConventions()
        {
        }

        void WhenExecutableAttributeIsApplied()
        {
        }

        [Executable(ExecutionOrder.Assertion, "BDDfy can pickup the steps")]
        void BDDfyCanPickupTheSteps()
        {
        }

        [Executable(ExecutionOrder.ConsecutiveAssertion, "which are decorated with the attribute")]
        void ThatAreDecoratedWithTheAttributes()
        {
        }
    }
}
</pre>

The only difference between this class and the previous implementation is the addition of the `ExecutableAttribute` on the last two methods. Now if you run the test you can see that these two methods have been picked up and run as part of scenario:

![Executable attribute](/img/BDDfy/executable-attributes/NonCompliantMethodNamesWithExecutableAttributes.jpg)

It is also worth mentioning that the 'Given' and 'When' steps were picked up too using method name conventions! BDDfy runs the Method Name Convention and `ExecutableAttribute` scanners in a pipeline (using [Chain of Responsibility](http://en.wikipedia.org/wiki/Chain-of-responsibility_pattern)) which means that for a method to be considered a step it has to  either match the naming conventions or be decorated with `Executable` attribute.

If a method matches both criteria then the one decorated with `ExecutableAttribute` wins. For example let's change the 'When' step to start with 'Then' (not that it makes sense; but for demonstration purposes only):

<pre>
using BDDfy.Core;
using BDDfy.Scanners.StepScanners.ExecutableAttribute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.ExecutableAttributes
{
    [TestClass]
    public class ExecutableAttributesOverridingTheNamingConvention
    {
        [TestMethod]
        public void ShouldBeAbleToRunScenariosWithNonCompliantMethodNames()
        {
            this.BDDfy();
        }

        void GivenSomeMethodsDoNotComplyWithBDDfyMethodNameConventions()
        {
        }

        [Executable(ExecutionOrder.Transition, "When the methods are decorated with ExecutableAttributes")]
        void ThenExecutableAttributeIsApplied()
        {
        }

        [Executable(ExecutionOrder.Assertion, "BDDfy can pickup the steps")]
        void BDDfyCanPickupTheSteps()
        {
        }

        [Executable(ExecutionOrder.ConsecutiveAssertion, "which are decorated with the attribute")]
        void ThatAreDecoratedWithTheAttributes()
        {
        }
    }
}
</pre>

Using Method Name Conventions this method would be picked up as a 'Then' step; but since it is decorated with `ExecutableAttribute` with `ExecutionOrder.Transition` (which translates to 'When') it is run as a 'When' step and gets the title specified in the attribute instead of the one driven from the method name by convention.

![Executable attribute overriding the method name convention](/img/BDDfy/executable-attributes/ExecutableAttributesOverridingTheNamingConvention.jpg)

##But dude, ExecutableAttribute is too technical
Fair enough. That is more of an implementation detail plus an extension point that you should not normally care about. For that reason BDDfy comes with a set of predefined subclasses of `ExecutableAttribute` which are easier to use: `Given`, `AndGiven`, `When`, `AndWhen`, `Then` and `AndThen`. These attributes have default constructors that allow you to set a method as a step without having to provide a title in which case the title will be driven by the method name. They also have a constructor that allows you the specify the title which will override the default convention. Let's see that in action. Changing the last example to use these attributes:

<pre>
using BDDfy.Core;
using BDDfy.Scanners.StepScanners.ExecutableAttribute;
using BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.ExecutableAttributes
{
    [TestClass]
    public class NonCompliantMethodNamesWithGwtAttributes
    {
        [TestMethod]
        public void ShouldBeAbleToRunScenariosWithNonCompliantMethodNames()
        {
            this.BDDfy("Non compliant method names with GWT attributes");
        }

        void GivenSomeMethodsDoNotComplyWithBDDfyMethodNameConventions()
        {
        }

        void WhenExecutableAttributeIsApplied()
        {
        }

        [Then]
        void BDDfyCanPickupTheSteps()
        {
        }

        [AndThen("which are decorated with the Given, AndGiven, When, AndWhen, Then or AndThen attributes ")]
        void ThatAreDecoratedWithTheAttributes()
        {
        }
    }
}
</pre>

The only change here was to replace the `ExecutableAttribute` usage with `Then` and `AndThen` attributes. And here is the report:

![Using Given, When, Then attributes](/img/BDDfy/executable-attributes/NonCompliantMethodNamesWithGwtAttributes.jpg)

To spice things up a bit I am also overriding the scenario name. This is only to show that you can always override scenario title regardless of your preferred BDDfy API/mode.

##Can I exclude a method from scan?
Just like there are cases when your step method name does not comply with conventions there may be cases where you have a non-step method that just happens to comply with conventions. Consider the following example:

<pre>
using BDDfy.Core;
using BDDfy.Scanners.StepScanners.ExecutableAttribute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.ExecutableAttributes
{
    [TestClass]
    public class ExcludingMethodsFromScan
    {
        [TestMethod]
        public void ShouldBeAbleToRunScenariosWithNonCompliantMethodNames()
        {
            this.BDDfy();
        }

        void GivenSomeMethodsComplyWithBDDfyMethodNameConventions()
        {
        }

        void AndGivenTheyAreNotRealSteps()
        {
        }

        void WhenIgnoreStepAttributeIsApplied()
        {
            WhenEzuelaIsNotSpelledLikeThisDude();
        }

        void WhenEzuelaIsNotSpelledLikeThisDude()
        {
        }

        void ThenBDDfyIgnoresTheMethod()
        {
        }
    }
}
</pre>

<small>Ok, admittedly that is a silly example; but it shows my point :o)</small>

There is a method called `WhenEzuelaIsNotSpelledLikeThisDude` which matches BDDfy's method name conventions. By default this method would be picked up as a step and run by BDDfy which is not what you want because it is there to support other methods in your scenario and is not a step. In cases like this, if you do not want to rename your method, you can use `IgnoreStepAttribute` like below:

<pre>
using BDDfy.Core;
using BDDfy.Scanners.StepScanners;
using BDDfy.Scanners.StepScanners.ExecutableAttribute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDDfy.ExecutableAttributes
{
    [TestClass]
    public class ExcludingMethodsFromScan
    {
        [TestMethod]
        public void ShouldBeAbleToRunScenariosWithNonCompliantMethodNames()
        {
            this.BDDfy();
        }

        void GivenSomeMethodsComplyWithBDDfyMethodNameConventions()
        {
        }

        void AndGivenTheyAreNotRealSteps()
        {
        }

        void WhenIgnoreStepAttributeIsApplied()
        {
            WhenEzuelaIsNotSpelledLikeThisDude();
        }

        [IgnoreStep]
        void WhenEzuelaIsNotSpelledLikeThisDude()
        {
        }

        void ThenBDDfyIgnoresTheMethod()
        {
        }
    }
}
</pre>

I just had to decorate the method with the attribute and now when I run the test that method is no longer picked up and run as a step which is my desired behavior.

![IgnoreStepAttribute](/img/BDDfy/executable-attributes/IgnoreStepAttribute.jpg)

This is not necessarily related to the `Executable` attribute as such and is part of the Reflective Scanner's logic which applies to both method name conventions and executable attributes.

##How to create your own dialect
If you want to use another syntax for your BDD behaviors because you do not like Given, When, Then or because you are writing low-level tests that do not quite fit into the GWT model you can create your own syntax easily. Near the end of this series I am writing a few posts dedicated to BDDfy extension points and how you can override and/or extend the framework easily. So I will just provide a small hint about this here and leave the full story for later.

In this running mode the only thing BDDfy cares about is the `Executable` attribute; so you can very easily subclass it as I have done with the abovementioned attributes and create a new dialect and it will just work. For your reference this is the implementation of a few of the out of the box attributes:

**[GivenAttribute](https://github.com/TestStack/TestStack.BDDfy/blob/master/TestStack.BDDfy/Scanners/StepScanners/ExecutableAttribute/GwtAttributes/GivenAttribute.cs)**
<pre>
public class GivenAttribute : ExecutableAttribute
{
    public GivenAttribute() : this(null) { }
    public GivenAttribute(string stepTitle) : base(Core.ExecutionOrder.SetupState, stepTitle) { }
}
</pre>

**[AndGivenAttibute](https://github.com/TestStack/TestStack.BDDfy/blob/master/TestStack.BDDfy/Scanners/StepScanners/ExecutableAttribute/GwtAttributes/AndGivenAttribute.cs)**
<pre>
public class AndGivenAttribute : ExecutableAttribute
{
    public AndGivenAttribute() : this(null) { }
    public AndGivenAttribute(string stepTitle) : base(Core.ExecutionOrder.ConsecutiveSetupState, stepTitle) { }
}
</pre>

**[WhenAttribute](https://github.com/TestStack/TestStack.BDDfy/blob/master/TestStack.BDDfy/Scanners/StepScanners/ExecutableAttribute/GwtAttributes/WhenAttribute.cs)**
<pre>
public class WhenAttribute : ExecutableAttribute
{
    public WhenAttribute() : this(null) { }
    public WhenAttribute(string stepTitle) : base(Core.ExecutionOrder.Transition, stepTitle) { }
}
</pre>

It is very easy to implement your own attribute and create your own dialect with BDDfy. You can also very easily override the existing Method Name Conventions to create a new dialect for conventions; but that is a matter of another post :)



  [10]: http://hg.mehdi-khalili.com/BDDfy/src/83fd2f4566c4/BDDfy/Scanners/StepScanners/ExecutableAttribute/GwtAttributes/GivenAttribute.cs
  [11]: http://hg.mehdi-khalili.com/BDDfy/src/83fd2f4566c4/BDDfy/Scanners/StepScanners/ExecutableAttribute/GwtAttributes/AndGivenAttribute.cs
  [12]: http://hg.mehdi-khalili.com/BDDfy/src/83fd2f4566c4/BDDfy/Scanners/StepScanners/ExecutableAttribute/GwtAttributes/WhenAttribute.cs
