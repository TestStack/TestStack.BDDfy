BDDfy (pronounced B D Defy) is the simplest BDD framework for .Net EVER! The name comes from the fact that it allows you to turn your tests into BDD behaviors simply.

A few quick facts about BDDfy:

 - It can run with any testing framework. Actually you don't have to use a testing framework at all. You can just apply it on your POCO (test) classes!
 - It does not need a separate test runner. You can use your runner of choice. For example, you can write your BDDfy tests using NUnit and run them using NUnit console or GUI runner, Resharper or TD.Net and regardless of the runner, you will get the same result.
 - It can run standalone scenarios. In other words, although BDDfy supports stories, you do not necessarily have to have or make up a story to use it. This is useful for developers who work in non-Agile environments but would like to get some decent testing experience.
 - You can use underscored or pascal or camel cased method names for your steps.
 - You do not have to explain your scenarios or stories or steps in string, but you can if you need full control over what gets printed into console and HTML reports.
 - BDDfy is very extensible: it's core barely has any logic in it and it delegates all it's responsibilities to it's extensions all of which are configurable; e.g. if you don't like the reports it generates, you can write your custom reporter in a few lines of code.

Using BDDfy, it is easier to switch to BDD. So if you are on a project with a couple of hundred tests already written and you think using BDD could make your tests more valuable, then BDDfy can help you with that. You are still going to need to make some changes; but hopefully they will be minimal.

##Installation
To use BDDfy:

 - Install NuGet if you have not already.
 - Go to 'Tools', 'Library Package Manager', and click 'Package Manager Console'.
 - In the console, type `Install-Package TestStack.BDDfy` and enter.

This adds BDDfy assembly and its dependencies to your test project. If this is the first time you are using BDDfy you may want to check out the samples on NuGet. Just run `Install-Package TestStack.BDDfy.Samples` and it will load two fully working samples to your project.

##Usage
Let's see BDDfy in action. I am going to use [Dan North's ATM sample](http://dannorth.net/introducing-bdd/) for this. I will copy his sample here for your convenience:

<pre>
Story: Account Holder withdraws cash

As an Account Holder
I want to withdraw cash from an ATM
So that I can get money when the bank is closed

Scenario 1: Account has sufficient funds
Given the account balance is $100
  And the card is valid
  And the machine contains enough money
When the Account Holder requests $20
Then the ATM should dispense $20
  And the account balance should be $80
 And the card should be returned

Scenario 2: Account has insufficient funds
Given the account balance is $10
  And the card is valid
  And the machine contains enough money
When the Account Holder requests $20
Then the ATM should not dispense any money
  And the ATM should say there are insufficient funds
  And the account balance should be $10
  And the card should be returned

Scenario 3: Card has been disabled
Given the card is disabled
When the Account Holder requests $20
Then the ATM should retain the card
  And the ATM should say the card has been retained
</pre>

In order to add BDDfy library to your test project:

 - In Visual Studio go to 'Tools', 'Library Package Manager', and click 'Package Manager Console'.
 - In the console, type `Install-Package TestStack.BDDfy` and enter.

This installs BDDfy on your project. As part of installation, BDDfy copies a file called 'BDDfy.ReadMe.txt' in your project root folder. This file explains a bit about how BDDfy works as well as some of its conventions.

I will start with the last scenario for this sample because it is simpler than other scenarios and we can focus more on BDDfy than on the scenario's implementation:

    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestStack.BDDfy;

    namespace BDDfy.Samples.Atm
    {
        [TestClass]
        public class CardHasBeenDisabled
        {
            void GivenTheCardIsDisabled()
            {
                throw new NotImplementedException();
            }

            void WhenTheAccountHolderRequestsMoney()
            {
            }

            void ThenTheAtmShouldRetainTheCard()
            {
            }

            void AndTheAtmShouldSayTheCardHasBeenRetained()
            {
            }

            [TestMethod]
            public void Execute()
            {
                this.BDDfy();
            }
        }
    }

This class represents our scenario and has one test method called Execute (it can be called anything). Inside this method, I have one line of code that calls BDDfy extension method on the instance. Let's run this test to see what happens. I am using ReSharper test runner to run the test:

![Not Implemented Method](/img/BDDfy/not-implemented-method.png)

*<small>Figure 1: CardHasBeenDisabled console report before the scenario is implemented</small>*

That is the console report BDDfy generates. Note that BDDfy tells you that the 'Given' step has not been implemented yet and the other steps were not executed.

By default, BDDfy also generates an HTML report called 'BDDfy.Html' in your project's output folder:

![Not Implemented Method](/img/BDDfy/not-implemented-method-html.png)

*<small>Figure 2: CardHasBeenDisabled Html report before the scenario is implemented</small>*

HTML report shows the summary on the top and the details on the bottom. If you click on scenarios, it also shows you the steps of that scenario along with the step result (and in case of an exception, the stack trace). You have a lot of control over HTML report and can customize a lot of things. You can also inject your own custom css and Javascript to get full control over the styling too.

<blockquote>
Note: As indicated in HTML and console reports, 'Given' step was unsuccessful due to the exception. When there is an exception in 'Given' or 'When' steps BDDfy will not run the remaining steps. It is shown in the console report with '[Not Executed]' in front of steps and in the HTML report with 'Not Executed' icon. This is because if your 'Given' or 'When' steps fail, there is no reason to run other steps. This rule does not apply to asserting steps (i.e. 'Then' parts) which means that you could have three asserting steps with one of them failing and the other two passing. In this case, BDDfy runs all the steps and shows you which of your assertions failed.</blockquote>

###Method naming conventions in reflective API
BDDfy uses reflection to scan your classes for steps. In this mode, known as reflective mode, it has two ways of finding a step: using attributes and method name conventions. The following is the list of method name conventions:

 - Method name ending with `Context` is considered a setup method but doesn't get shown in the reports
 - Method name equaling `Setup` is a setup method but doesn't get shown in in the reports
 - Method name starting with `Given` is a setup step that gets shown in the reports
 - Method name starting with `AndGiven` and 'And_given_' are considered setup steps running after 'Given' steps which is reported.
 - Method name starting with `When` is considered a state transition step and is reported
 - Method name starting with `AndWhen` and `And_when_` are considered state transition steps running after 'When' steps and is reported
 - Method name starting with `Then` is an asserting step and is reported
 - Method name starting with `And` and `AndThen` and `And_then_` are considered an asserting steps running after 'Then' step and is reported
 - Method name starting with `TearDown` is considered as tear down method that is always run at the end but doesn't get shown in the reports.

If you don't like Given When Then dialect you can write your own dialect and register it in a few lines of code.

BDDfy uses method names to generate the step titles and uses the scenario class name to generate the scenario title. Ok, let's implement the steps:

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestStack.BDDfy;

    namespace BDDfy.Samples.Atm
    {
        [TestClass]
        public class CardHasBeenDisabled
        {
            private Card _card;
            Atm _subject;

            void GivenTheCardIsDisabled()
            {
                _card = new Card(false, 100);
                _subject = new Atm(100);
            }

            void WhenTheAccountHolderRequestsMoney()
            {
                _subject.RequestMoney(_card, 20);
            }

            void ThenTheAtmShouldRetainTheCard()
            {
                Assert.IsTrue(_subject.CardIsRetained);
            }

            void AndTheAtmShouldSayTheCardHasBeenRetained()
            {
                Assert.AreEqual(DisplayMessage.CardIsRetained, _subject.Message);
            }

            [TestMethod]
            public void Execute()
            {
                this.BDDfy();
            }
        }
    }

For the purpose of this article, I am going to provide you with the fully implemented domain class here. This is, of course, not the way you would do it in a real test first methodology:

    namespace BDDfy.Samples.Atm
    {
        public class Atm
        {
            public int ExistingCash { get; private set; }

            public Atm(int existingCash)
            {
                ExistingCash = existingCash;
            }

            public void RequestMoney(Card card, int request)
            {
                if (!card.Enabled)
                {
                    //CardIsRetained = true;
                    Message = DisplayMessage.CardIsRetained;
                    return;
                }

                if (card.AccountBalance &lt; request)
                {
                    Message = DisplayMessage.InsufficientFunds;
                    return;
                }

                DispenseValue = request;
                card.AccountBalance -= request;
            }

            public int DispenseValue { get; set; }

            public bool CardIsRetained { get; private set; }

            public DisplayMessage Message { get; private set; }
        }

        public class Card
        {
            public int AccountBalance { get; set; }
            private readonly bool _enabled;

            public Card(bool enabled, int accountBalance)
            {
                AccountBalance = accountBalance;
                _enabled = enabled;
            }

            public bool Enabled
            {
                get { return _enabled; }
            }
        }

        public enum DisplayMessage
        {
            None = 0,
            CardIsRetained,
            InsufficientFunds
        }
    }

Let's run the test again:

![Failed step console report](/img/BDDfy/failed-step-console.png)

*<small>Figure 3. CardHasBeenDisabled scenario with buggy implementation - console report</small>*

![Failed step console report](/img/BDDfy/failed-step-html.png)

*<small>Figure 4. CardHasBeenDisabled with buggy implementation - HTML report</small>*

As mentioned above, BDDfy does not stop the execution when there is an exception on your asserting steps. In this case, you can see that 'Then the atm should retain the card' step has failed; but BDDfy has run the next step and it shows you that it has passed. Of course, the scenario will be red until all its steps pass.

Both console and HTML reports show that my scenario has failed. It seems like I have a bug in my Atm class. So I fix the bug (i.e. uncomment the only commented line in the Atm class) and run the test again and this time I get green result:

![Passing test console report](/img/BDDfy/passing-test-console.png)

*<small>Figure 5. CardHasBeenDisabled green console report</small>*

![Passing test html report](/img/BDDfy/passing-test-html.png)

*<small>Figure 6. CardHasBeenDisabled green HTML report</small>*

###ExecutableAttribute in reflective API
Let's implement another scenario. This time, I will not bore you with the red and green phases:

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestStack.BDDfy;
    using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;

    namespace BDDfy.Samples.Atm
    {
        [TestClass]
        public class AccountHasInsufficientFund
        {
            private Card _card;
            private Atm _atm;

            // You can override step text using executable attributes
            [Given("Given the account balance is $10")]
            void GivenTheAccountBalanceIs10()
            {
                _card = new Card(true, 10);
            }

            void AndGivenTheCardIsValid()
            {

            }

            void AndGivenTheMachineContainsEnoughMoney()
            {
                _atm = new Atm(100);
            }

            [When("When the account holder requests $20")]
            void WhenTheAccountHolderRequests20()
            {
                _atm.RequestMoney(_card, 20);
            }

            void ThenTheAtmShouldNotDispenseAnyMoney()
            {
                Assert.AreEqual(0, _atm.DispenseValue);
            }

            void AndTheAtmShouldSayThereAreInsufficientFunds()
            {
                Assert.AreEqual(DisplayMessage.InsufficientFunds, _atm.Message);
            }

            void AndTheAccountBalanceShouldBe10()
            {
                Assert.AreEqual(10, _card.AccountBalance);
            }

            void AndTheCardShouldBeReturned()
            {
                Assert.IsFalse(_atm.CardIsRetained);
            }

            [TestMethod]
            public void Execute()
            {
                this.BDDfy();
            }
        }
    }

This scenario is a bit more involved. Let's run the test and see the reports:

![ExecutableAttribute console report](/img/BDDfy/exec-attr-console.png)

*<small>Figure 7. AccountHasInsufficientFund console report</small>*

![ExecutableAttribute console report](/img/BDDfy/exec-attr-html.png)

*<small>Figure 8. AccountHasInsufficientFund HTML report</small>*

When reflecting over your test class, BDDfy looks for a custom attribute called `ExecutableAttribute` on the methods and considers the method decorated with this attribute as a step. You can use attributes either when your method name does not comply with the conventions or when you want to provide a step text that reflection would not be able to create for you.

To make it easier to use, `ExecutableAttribute` has a few subtypes that you can use. In this scenario, I used `GivenAttribute`, `WhenAttribute` and `AndThenAttribute` attributes because I wanted to show '$' in the step text that would not be possible using method name reflection. Other available attributes are `AndGivenAttribute`, `AndWhenAttribute` and `ThenAttribute`. If you think some other `ExecutableAttribute` could really help you, then you can very easily implement one.

While we are talking about attributes, there is also an attribute called `IgnoreStepAttribute` that you can apply on a method you want BDDfy to ignore as a step. This is useful when you have a method whose name complies with naming conventions BDDfy uses; but is not really a step.

As you may have noticed, we have not still implemented any story. BDDfy is capable of executing standalone scenarios and generating report from them which I think is quite useful for teams that do not do Agile/BDD but are interested in a better testing experience and reporting. In this example, we have a story though. So let's code it:

    using TestStack.BDDfy.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    namespace BDDfy.Samples.Atm
    {
        [TestClass]
        [Story(
            AsA = "As an Account Holder",
            IWant = "I want to withdraw cash from an ATM",
            SoThat = "So that I can get money when the bank is closed")]
        public class AccountHolderWithdrawsCash
        {
            [TestMethod]
            public void AccountHasInsufficientFund()
            {
                new AccountHasInsufficientFund().BDDfy();
            }

            [TestMethod]
            public void CardHasBeenDisabled()
            {
                new CardHasBeenDisabled().BDDfy();
            }
        }
    }

Any class decorated with a `StoryAttribute` represents a story. Using `StoryAttribute`, you can also specify the story narrative. To associate the story with its scenarios, you should implement a test method per scenario.

That is it. Just before we run these tests, we should get rid of the Execute test methods in our scenario classes as we no longer need them. We only had them there because we implemented those as standalone scenarios. Now that our scenarios are part of a story, they should not run standalone. Let's run the tests again:

![Scenario with story console report](/img/BDDfy/story-console.png)

*<small>Figure 9. Scenarios moved to story - console report</small>*

We now have only one test class which includes two test methods; one per scenario. Also note that the story narrative is now appearing on the top of the console report for each scenario.

![Scenario with story html report](/img/BDDfy/story-html.png)

*<small>Figure 10. Scenarios moved to story - HTML report</small>*

In the HTML report, the story narrative appears only once above the story's scenarios.

Note: In the summary section of the HTML report before we implemented the story, we had two namespaces. After adding the story, the namespace count turned into zero and now we instead have one story. BDDfy only counts namespaces for standalone scenarios.

If you compare the above reports with the ones generated when we had `Execute` methods in the scenarios, you see that these reports group your scenarios by story instead of namespace which makes the reports more readable.

###Fluent API
Let's do our last scenario. For this one, I am going to use the Fluent API BDDfy provides:

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    namespace BDDfy.Samples.Atm
    {
        [TestClass]
        public class AccountHasSufficientFund
        {
            private Card _card;
            private Atm _atm;

            public void GivenTheAccountBalanceIs(int balance)
            {
                _card = new Card(true, balance);
            }

            public void AndTheCardIsValid()
            {
            }

            public void AndTheMachineContainsEnoughMoney()
            {
                _atm = new Atm(100);
            }

            public void WhenTheAccountHolderRequests(int moneyRequest)
            {
                _atm.RequestMoney(_card, moneyRequest);
            }

            public void ThenTheAtmShouldDispense(int dispensedMoney)
            {
                Assert.AreEqual(dispensedMoney, _atm.DispenseValue);
            }

            public void AndTheAccountBalanceShouldBe(int balance)
            {
                Assert.AreEqual(balance, _card.AccountBalance);
            }

            public void AndTheCardShouldBeReturned()
            {
                Assert.IsFalse(_atm.CardIsRetained);
            }
        }
    }

This looks very much like the other scenarios with one difference: the naming conventions are not quite right and you think that BDDfy would fail to match some of these methods - specifically those starting with `And` instead of `AndGiven`. If you were to use reflecting scanners, those methods would have been picked up as asserting steps which meant they would run and report in incorrect order! You could very easily customise BDDfy's naming conventions or rename your methods or use `ExecutableAttribute` to make these methods scannable by reflecting scanners; but I wrote the class like this to show how you can use a fluent API to let BDDfy find your methods/steps:

    [TestMethod]
    public void AccountHasSufficientfund()
    {
        new AccountHasSufficientFund()
            .Given(s => s.GivenTheAccountBalanceIs(100), "Given the account balance is $100")
                .And(s => s.AndTheCardIsValid())
                .And(s => s.AndTheMachineContainsEnoughMoney())
            .When(s => s.WhenTheAccountHolderRequests(20),
        "When the account holder requests $20")
            .Then(s => s.ThenTheAtmShouldDispense(20), "Then the ATM should dispense $20")
                .And(s => s.AndTheAccountBalanceShouldBe(80),
            "And the account balance should be $80")
                .And(s => s.AndTheCardShouldBeReturned())
            .BDDfy();
    }

You may write this method in your scenario class if you want to run it as a standalone scenario. I added it to my `AccountHolderWithdrawsCash` story to make it part of my story.

By default, BDDfy uses two scanners namely `MethodNameStepScanner` and `ExecutableAttributeStepScanner` - which I collectively refer to as reflective scanners . The former scans your scenario class for steps using method name conventions and the latter looks for `ExecutableAttribute` on your methods. There is also a third scanner called `FluentStepScanner` which we used in the above example. You don't have to tell BDDfy which scanner to use: it picks the right scanner according to your code.

Note: Reflective scanners run in a pipeline which means you can mix and match their usage in your scenario; however, when you use `FluentStepScanner`, BDDfy does not use other scanners which means method names and attributes are ignored for scanning methods. In other words, you are in full control of what steps you want run and in what order.

For reporter modules, it does not make any difference what scanner you use; so the HTML and console reports are going to look the same regardless of the scanners.

Using fluent API you can implement your stories/scenarios in an alternative and rather interesting way. Instead of having one class per scenario and a class for your story, you could write one class that represents all your scenarios as well as your story:

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestStack.BDDfy;
    using TestStack.BDDfy.Core;
    using TestStack.BDDfy.Scanners.StepScanners.Fluent;

    namespace BDDfy.Samples.Atm
    {
        [TestClass]
        [Story(
            Title = "Account holder withdraws cash",
            AsA = "As an Account Holder",
            IWant = "I want to withdraw cash from an ATM",
            SoThat = "So that I can get money when the bank is closed")]
        public class AccountHolderWithdrawsCashFluentScanner
        {
            private const string GivenTheAccountBalanceIsTitleTemplate =
                        "Given the account balance is ${0}";
            private const string AndTheMachineContainsEnoughMoneyTitleTemplate =
                        "And the machine contains enough money";
            private const string WhenTheAccountHolderRequestsTitleTemplate =
                        "When the account holder requests ${0}";
            private const string AndTheCardShouldBeReturnedTitleTemplate =
                        "And the card should be returned";

            private Card _card;
            private Atm _atm;

            public void GivenTheAccountBalanceIs(int balance)
            {
                _card = new Card(true, balance);
            }

            public void GivenTheCardIsDisabled()
            {
                _card = new Card(false, 100);
                _atm = new Atm(100);
            }

            public void AndTheCardIsValid()
            {
            }

            public void AndTheMachineContains(int atmBalance)
            {
                _atm = new Atm(atmBalance);
            }

            public void WhenTheAccountHolderRequests(int moneyRequest)
            {
                _atm.RequestMoney(_card, moneyRequest);
            }

            public void TheAtmShouldDispense(int dispensedMoney)
            {
                Assert.AreEqual(dispensedMoney, _atm.DispenseValue);
            }

            public void AndTheAccountBalanceShouldBe(int balance)
            {
                Assert.AreEqual(balance, _card.AccountBalance);
            }

            public void CardIsRetained(bool cardIsRetained)
            {
                Assert.AreEqual(cardIsRetained, _atm.CardIsRetained);
            }

            void AndTheAtmShouldSayThereAreInsufficientFunds()
            {
                Assert.AreEqual(DisplayMessage.InsufficientFunds, _atm.Message);
            }

            void AndTheAtmShouldSayTheCardHasBeenRetained()
            {
                Assert.AreEqual(DisplayMessage.CardIsRetained, _atm.Message);
            }

            [TestMethod]
            public void AccountHasInsufficientFund()
            {
                this.Given(s => s.GivenTheAccountBalanceIs(10),
                GivenTheAccountBalanceIsTitleTemplate)
                        .And(s => s.AndTheCardIsValid())
                        .And(s => s.AndTheMachineContains(100),
                AndTheMachineContainsEnoughMoneyTitleTemplate)
                    .When(s => s.WhenTheAccountHolderRequests(20),
                WhenTheAccountHolderRequestsTitleTemplate)
                    .Then(s => s.TheAtmShouldDispense(0), "Then the ATM should not dispense")
                        .And(s => s.AndTheAtmShouldSayThereAreInsufficientFunds())
                        .And(s => s.AndTheAccountBalanceShouldBe(10))
                        .And(s => s.CardIsRetained(false),
                AndTheCardShouldBeReturnedTitleTemplate)
                    .BDDfy();
            }

            [TestMethod]
            public void AccountHasSufficientFund()
            {
                this.Given(s => s.GivenTheAccountBalanceIs(100),
                GivenTheAccountBalanceIsTitleTemplate)
                        .And(s => s.AndTheCardIsValid())
                        .And(s => s.AndTheMachineContains(100),
                AndTheMachineContainsEnoughMoneyTitleTemplate)
                    .When(s => s.WhenTheAccountHolderRequests(20),
                WhenTheAccountHolderRequestsTitleTemplate)
                    .Then(s => s.TheAtmShouldDispense(20), "Then the ATM should dispense $20")
                        .And(s => s.AndTheAccountBalanceShouldBe(80),
                "And the account balance should be $80")
                        .And(s => s.CardIsRetained(false),
                AndTheCardShouldBeReturnedTitleTemplate)
                    .BDDfy();
            }

            [TestMethod]
            public void CardHasBeenDisabled()
            {
                this.Given(s => s.GivenTheCardIsDisabled())
                    .When(s => s.WhenTheAccountHolderRequests(20))
                    .Then(s => s.CardIsRetained(true), "Then the ATM should retain the card")
                        .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
                    .BDDfy();
            }
        }
    }

This way, you will not need a separate story class or one class per scenario - everything is mixed into one class. Running tests in this class generates the very same console and HTML reports.

This style of writing stories and scenarios helps you be a bit DRYer; but one could argue it violates [SRP](http://en.wikipedia.org/wiki/Single_responsibility_principle). It is important to note that you could achieve [DRY](http://en.wikipedia.org/wiki/Don't_repeat_yourself)ness without using fluent API. In order to do that, you would need to use inheritance or composition to compose your scenario class from classes that would hold the common behaviors. For example, if you put your 'Given' and 'When' steps inside a base class and your 'Then' steps inside a subclass, BDDfy will scan all these steps into your scenario. That would not give you as much freedom as the fluent API though.

###Titles
By default, BDDfy uses the name of the story class for the story title as we saw in the first few samples. You can override this behavior by passing the title into the Story attribute as I have done in the above example. I named my class `AccountHolderWithdrawsCashFluentScanner` to differentiate it from the story class in the other implementation; but I do not want the story title to end with 'fluent scanner'. So I provided the story with a title I will be happy to see in the reports:

    [Story(
        Title = "Account holder withdraws cash",
        AsA = "As an Account Holder",
        IWant = "I want to withdraw cash from an ATM",
        SoThat = "So that I can get money when the bank is closed")]
    public class AccountHolderWithdrawsCashFluentScanner

For scenario titles, BDDfy uses the class name; for example in the first scenario, BDDfy extracted the scenario text 'Card has been disabled' from the class name 'CardHasBeenDisabled'. In the above example, because all your scenarios are fetched from the same class, one would expect BDDfy to give them all the same title! That is not the case though. In this case, BDDfy detects that you are using fluent API and uses the test method's name to generate the scenario title. For example, the `CardHasBeenDisabled` method results into 'Card has been disabled'. That said, if you want to have full control over scenario title, you may pass the title to BDDfy method; e.g.

    [TestMethod]
    public void CardHasBeenDisabled()
    {
        this.Given(s => s.GivenTheCardIsDisabled())
            .When(s => s.WhenTheAccountHolderRequests(20))
            .Then(s => s.CardIsRetained(true), "Then the ATM should retain the card")
                .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
            .BDDfy("Card has been disabled and account holder requests $20");
    }

BDDfy uses step method names for the method title and it is also capable of injecting the input arguments in the title. In the above example, `Given(s => s.GivenTheCardIsDisabled())` results into 'Given the card is disabled' and `When(s => s.WhenTheAccountHolderRequests(20))` results in 'When the account holder requests 20'; but sometimes that is not good enough (e.g., the account holder does not request 20 - s/he requests 20 dollars). In cases like this, if you are using the fluent API, you can pass in the desired title into the step indicator methods; e.g.

`And(s => s.CardIsRetained(false), "And the card should be returned")`

The string that you pass into these methods could also have placeholders for input arguments. This way, you can reuse a string template across several scenarios as I did above. I declared a const on the class level:

`private const string GivenTheAccountBalanceIsTitleTemplate = "Given the account balance is ${0}";`

...and then I used it in the step indicator methods like:

`.Given(s => s.GivenTheAccountBalanceIs(10), GivenTheAccountBalanceIsTitleTemplate)`

...which resulted in the step title: 'Given the account balance is $10'. It is worth mentioning that BDDfy uses the template in a string.Format() method to generate the title; so you may use as many placeholders and wherever in the title as you like as long as they match the method inputs.

As mentioned before, when using reflecting scanners you may use `ExecutableAttribute` or a subtype of it to provide custom step texts. The string provided to these attributes also accepts placeholders that are filled by method input arguments.

Reflective and fluent APIs offer similar functionalities (but some through different means). Below, you may find a quick comparison:

<table>
<thead>
<tr>
<td>Functionality</td>
<td>Reflecting Scanners</td>
<td>Fluent Scanner</td>
</tr>
</thead>

<tbody>
<tr>
<td>Story title from story class name&nbsp;</td>
<td>Yes</td>
<td>Yes</td>
</tr>

<tr>
<td>Story title from Title in StoryAttribute</td>
<td>Yes</td>
<td>Yes</td>
</tr>

<tr>
<td>Scenario title from scenario class name</td>
<td>Yes</td>
<td>No</td>
</tr>

<tr>
<td>Scenario title from test method name</td>
<td>No</td>
<td>Yes</td>
</tr>

<tr>
<td>Custom scenario title passed in <code>BDDfy</code> method</td>
<td>Yes</td>
<td>Yes</td>
</tr>

<tr>
<td>Implementing story and scenarios in one class</td>
<td>No</td>
<td>Yes</td>
</tr>

<tr>
<td>Finding step methods using naming convention</td>
<td>Yes</td>
<td>No</td>
</tr>

<tr>
<td>Finding step methods using attributes</td>
<td>Yes</td>
<td>No</td>
</tr>

<tr>
<td>Finding step methods using lambda expression</td>
<td>No</td>
<td>Yes</td>
</tr>

<tr>
<td>Running step methods with input arguments</td>
<td>Yes - using <code>RunStepWithArgsAttribute</code></td>
<td>Yes - using lambda expression</td>
</tr>

<tr>
<td>Step title using step method name</td>
<td>Yes</td>
<td>Yes</td>
</tr>

<tr>
<td>Using input arguments in the step title</td>
<td>Yes</td>
<td>Yes</td>
</tr>

<tr>
<td>Custom step title</td>
<td>Yes - using attributes</td>
<td>Yes - passing into step indicator methods</td>
</tr>

<tr>
<td>Using the same method for several steps</td>
<td>Yes - using <code>RunStepWithArgsAttribute</code></td>
<td>Yes</td>
</tr>

<tr>
<td>Ignoring a method as step</td>
<td>Yes - using <code>IgnoreAttribute</code></td>
<td>N/A - Do not indicate the method</td>
</tr>

<tr>
<td><code>Dispose </code>method</td>
<td>Yes - Implement a method starting with 'TearDown'</td>
<td>Yes - Use <code>TearDownWith</code> step indicator</td>
</tr>

<tr>
<td>Using inherited step methods</td>
<td>Yes</td>
<td>Yes</td>
</tr>
</tbody>
</table>


You may think that these two APIs are significantly different and that a huge amount of effort has been put to implement both models; but the ONLY difference between these two models is in their step scanners which are not even part of the core. BDDfy is very extensible and the core barely has any logic in it. It instead delegates all its responsibilities to its extensions, one of which is step scanner implementing `IStepScanner`. The same applies to scenario scanner implemening `IScenarioScanner`, and story scanner implementing `IScanner`, report generators, test runner and exception handler etc. All these interfaces contain only one method which makes it rather straightforward to implement a new extension. Step scanners are a very small part of this framework, and if you think you could benefit from a different scanner you could very simply implement it.

The sample we worked through in this article is one of the BDDfy samples. There are a few more samples that are implemented in different ways and use some other BDDfy features I did not explain here and I think are definitely worth looking. Samples are available on TestStack.BDDfy.Samples NuGet package. The samples are all implemented using NUnit; but as shown in this article you can use MSTest or any other testing framework.
