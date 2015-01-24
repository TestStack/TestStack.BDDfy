BDDfy is the simplest BDD framework to use, customize and extend! 

A few quick facts about BDDfy:
 - It can run with any testing framework. Actually you don't have to use a testing framework at all. You can just apply it on your POCO (test) classes!
 - It does not need a separate test runner. You can use your runner of choice. For example, you can write your BDDfy tests using NUnit and run them using NUnit console or GUI runner, Resharper or TD.Net and regardless of the runner, you will get the same result.
 - It can run standalone scenarios. In other words, although BDDfy supports stories, you do not necessarily have to have or make up a story to use it. This is useful for developers who work in non-Agile environments but would like to get some decent testing experience.
 - You can use underscored or pascal or camel cased method names for your steps.
 - You do not have to explain your scenarios or stories or steps in string, but you can if you need full control over what gets printed into console and HTML reports.
 - BDDfy is very extensible: the core barely has any logic in it and delegates all its responsibilities to the extensions all of which are configurable; e.g. if you don't like the reports it generates, you can write your custom reporter in a few lines of code.

## Usage
To use BDDfy install TestStack.BDDfy nuget package: `Install-Package TestStack.BDDfy`

This adds BDDfy assembly and its dependencies to your test project. If this is the first time you are using BDDfy you may want to check out the samples on NuGet. Just run `Install-Package TestStack.BDDfy.Samples` and it will load two fully working samples to your project.

Now that you have installed BDDfy, write your first test (this test is borrowed from ATM sample that you can install using nuget package TestStack.BDDfy.Samples):

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
    	
    	[Fact]
    	public void Execute()
    	{
    	    this.BDDfy();
    	}
    }

And this gives you a report like:

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

This is just the console report. Have a look at your output folder and you should see a nice html report too.

If you want more control you can also use BDDfy's Fluent API. Here is another example done using the Fluent API:

	[Fact]
	public void CardHasBeenDisabled()
	{
	    this.Given(s => s.GivenTheCardIsDisabled())
	        .When(s => s.WhenTheAccountHolderRequests(20))
	        .Then(s => s.CardIsRetained(true), "Then the ATM should retain the card")
	            .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
	        .BDDfy(htmlReportName: "ATM");
	}

which gives you a report like:

	Scenario: Card has been disabled
    	Given the card is disabled
    	When the account holder requests 20
    	Then the ATM should retain the card
      		And the atm should say the card has been retained

This is only the tip of iceberg. Absolutely everything you do with BDDfy is extensible and customizable. 
You might see full documentation of BDDfy on the [TestStack documentation website](http://docs.teststack.net/bddfy/index.html).
Oh and while you are there don't forget to checkout other cool projects from [TestStack](http://teststack.net/).

## Authors 
* [Mehdi Khalili](https://github.com/MehdiK)
* [Michael Whelan](https://github.com/mwhelan)

## License
BDDfy is released under the MIT License. See the bundled license.txt file for details.
