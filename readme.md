BDDfy is explained on Mehdi Khalili's blog series in full details which you may find http://www.mehdi-khalili.com/bddify-in-action/introduction

This is a very short tutorial and quickstart.

BDDfy is the simplest BDD framework EVER! To use BDDfy:

 - Install NuGet if you have not already.
 - Go to 'Tools', 'Library Package Manager', and click 'Package Manager Console'.
 - In the console, type 'Install-Package BDDfy' and enter.

This adds BDDfy assembly and its dependencies to your test project. Oh, BTW, BDDfy can work with any and all testing frameworks. In fact, it works even if you are not using any testing framework.

If this is the first time you are using BDDfy you may want to check out some of the samples on NuGet. Just search NuGet for BDDfy and you will see a list of BDDfy samples. You may install one or more samples to see how the framework works. Each sample installs required packages (including BDDfy and NUnit).

##Quick start
Now that you have installed BDDfy, write your first test (this test is borrowed from ATM sample that you can install using nuget package BDDfy.Samples.ATM):

{{{
[Story(
    AsA = "As an Account Holder",
    IWant = "I want to withdraw cash from an ATM",
    SoThat = "So that I can get money when the bank is closed")]
public class AccountHasInsufficientFund
{
    private Card _card;
    private Atm _atm;

    // You can override step text using executable attributes
    [Given(StepText = "Given the account balance is $10")]
    void GivenAccountHasEnoughBalance()
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

    void AndTheCardShouldBeReturned()
    {
        Assert.IsFalse(_atm.CardIsRetained);
    }

    [Test]
    public void Execute()
    {
        this.BDDfy();
    }
}

}}}

And this gives you a report like:
{{{
Story: Account holder withdraws cash
    As an Account Holder
    I want to withdraw cash from an ATM
    So that I can get money when the bank is closed

Scenario: Account has insufficient fund
    Given the account balance is $10
      And the card is valid
    When the account holder requests $20
    Then the atm should not dispense any money
      And the atm should say there are insufficient funds
      And the card should be returned
}}}

This is just the console report. Have a look at your output folder and you should see a nice html report too.

If you want more control you can also use BDDfy's Fluent API. Here is another example done using the Fluent API:

{{{
[Test]
public void CardHasBeenDisabled()
{
    this.Given(s => s.GivenTheCardIsDisabled())
        .When(s => s.WhenTheAccountHolderRequests(20))
        .Then(s => s.CardIsRetained(true), "Then the ATM should retain the card")
            .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
        .BDDfy(htmlReportName: "ATM");
}
}}}

which gives you a report like:
{{{
Scenario: Card has been disabled
    Given the card is disabled
    When the account holder requests 20
    Then the ATM should retain the card
      And the atm should say the card has been retained
}}}

==How does BDDfy work?==
BDDfy uses quite a few conventions to make it frictionless to use. For your convenience, I will try to provide a quick tutorial below:

===Finding steps===
BDDfy scans your bddified classes for steps. Currently it has three ways of finding a step: 
 * Using attributes 
 * Using method name conventions 
 * And using fluent API.

BDDfy runs your steps in the following order: SetupState, ConsecutiveSetupState, Transition, ConsecutiveTransition, Assertion, ConsecutiveAssertion and TearDown. Some of these steps are reported in the console and html reports and some of them are not. Please read below for further information.

====Attributes====
BDDfy looks for a custom attribute called ExecutableAttribute on your method. To make it easier to use, ExecutableAttribute has a few subclasses that you can use: you may apply Given, AndGiven, When, AndWhen, Then, and AndThen attributes on any method you want to make available to BDDfy.

====Method name convention====
BDDfy uses some conventions to find methods that should be turned into steps. Here is the current conventions. The method name:

 * ending with "Context" is considered as a setup method (not reported).
 * "Setup" is considered as as setup method  (not reported). 
 * starting with "Given" is considered as a setup method (reported). 
 * starting with "AndGiven" is considered as a setup method that runs after Context, Setup and Given steps (reported).
 * starting with "When" is considered as a transition method  (reported). 
 * starting with "AndWhen" is considered as a transition method that runs after When steps (reported).
 * starting with "Then" is considered as an asserting method (reported).
 * starting with "And" is considered as an asserting method (reported).
 * starting with "TearDown" is considered as a finally method which is run after all the other steps (not reported).

As you see in the above example you can mix and match the executable attributes and method name conventions to acheive great flexibility and power.

====Fluent API====
Fluent API gives you the absolute power over step selection and their titles. When you use Fluent API for a test, the attributes and method name conventions are ignored for that test. 

Pleasee note that you can have some tests using fluent API and some using a combination of attributes and method name conventions. Each .BDDfy() test works in isolation of others.

==Other conventions==
BDDfy prefers convention over configuration; but it also allows you to configure pretty much all the conventions. Here I will try to list some of the conventions and the way you can override them:

===Method name convention===
As mentioned above, by default BDDfy uses method name convention to find your steps. You can override this using ExecutableAttribute or Fluent API as discussed above.

===Step title convention===
BDDfy extracts steps titles differently depending on the way steps are found:

====When using method name convention====

====When using ExecutableAttributes====

====When using Fluent API====

===Scenario title convention===
