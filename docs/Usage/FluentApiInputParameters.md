# Fluent API Input Params

In [the last post](/fluent-api.html) we discussed BDDfy's Fluent API and had a look at a very simple usage where we ported the example done using [Method Name Convention](/method-name-conventions.html) to use Fluent API. In this post we will discuss how you can provide input parameters to your step methods when using Fluent API.

##Using input parameters
In order to specify your step methods in Fluent API you provide BDDfy with some method call lambda expressions. In the examples we have seen so far we only called methods that did not have any input parameters; but BDDfy also supports input parameters. BDDfy inspects the provided expression and [extracts the input parameters](http://www.mehdi-khalili.com/extracting-input-arguments-from-expressions) you have passed and when it comes the execution time it calls those methods using the provided inputs. Alright let's see this in action. The example this time will be about modeling [Alcohol Laws in Australia](http://en.wikipedia.org/wiki/Alcohol_laws_of_Australia). As usual the examples you see here are created to show you the API usage.

Let's see what the code would look like using Fluent API without overriding the titles and injecting input parameters:

<pre>
using NUnit.Framework;

namespace BDDfy.FluentApiInputParameters
{
    public class AlcoholLawsApplyToLicencedPremises
    {
        [Test]
        public void AlcoholIsNotSoldToMinors()
        {
            this.Given(_ => GivenIWasBornOn01011998())
                .When(_ => WhenIAskToBuyAlcoholInALicencedPremiseOn01012012())
                .Then(_ => ThenIDoNotGetTheAlocohol())
                    .And(_ => AndIAmWalkedOut())
                .BDDfy();
        }

        private void AndIAmWalkedOut()
        {
        }

        private void ThenIDoNotGetTheAlocohol()
        {
        }

        private void WhenIAskToBuyAlcoholInALicencedPremiseOn01012012()
        {
        }

        private void GivenIWasBornOn01011998()
        {
        }
    }
}
</pre>

Hmmm, the method names are rather ugly and not very readable! How about the reports?

![Ugly report from the first attempt](/img/BDDfy/fluent-api-input-params/first-attempt-ugly-report.JPG)

The reports are a bit more readable due to spacing between words; but the dates are still cryptic. We can do better than that. Below I have added a new method to the same class in which I have overridden the default step title for the 'Given' and 'When' methods:

<pre>
[Test]
public void AlcoholIsNotSoldToMinorsTake2()
{
    this.Given(_ => GivenIWasBornOn01011998(), "Given I was born on 01-01-1998")
        .When(_ => WhenIAskToBuyAlcoholInALicencedPremiseOn01012012(), "When I ask to buy alcohol in a licenced permise on 01-01-2012")
        .Then(_ => ThenIDoNotGetTheAlocohol())
            .And(_ => AndIAmWalkedOut())
        .BDDfy("Alcohol is not sold to minors");
}
</pre>

Please note that I also overrode the scenario title. This had to happen because I named my scenario method <code>AlcoholIsNotSoldToMinorsTake2</code> which by default would result into scenario title 'Alcohol is not sold to minors take 2' which obviously is not what I want. So I overrode it to the title I'd be happy with.

In this implementation the method names are still as unreadable as before; but the reports look a bit better as the dates are more readable:

![Step titles are overridden](/img/BDDfy/fluent-api-input-params/step-titles-are-overridden.JPG)

In this implementation the dates, that should be more like variables, are still part of my method names. This means that I cannot reuse the same method to write a test for happy path where for example someone over 18 years old can buy alcohol. So what can we do? Input parameters to the rescue. Let's change our methods to accept input parameters. Add the following code to the same class:

<pre>
[Test]
public void AlcoholIsNotSoldToMinorsTake3()
{
    this.Given(_ => GivenIWasBornOn(new DateTime(1998, 1, 1)))
        .When(_ => WhenIAskToBuyAlcoholInALicencedPremiseOn(new DateTime(2012, 1, 1)))
        .Then(_ => ThenIDoNotGetTheAlocohol())
            .And(_ => AndIAmWalkedOut())
        .BDDfy("Alcohol is not sold to minors");
}

private void WhenIAskToBuyAlcoholInALicencedPremiseOn(DateTime occurrenceDate)
{
}

private void GivenIWasBornOn(DateTime dateOfBirth)
{
}
</pre>

Again I had to override the scenario title. This time I have implemented two methods that look exactly the same as my previous 'Given' and 'When' methods except that they accept the date as parameters. So in my scenario method I am providing the dates inline as input parameters. The code is now much more readable and the report looks like:

![Using input parameters](/img/BDDfy/fluent-api-input-params/using-input-parameters.JPG)

As you can see in the report the input parameters are <code>ToString</code>ed and appended to step titles. This implementation solved the problems mentioned above. BDDfy can see that you are passing some input parameters to your lambda expressions; so it extracts them out and uses them to generate a step title. In this particular case, due to using <code>DateTime</code>, the result is less than perfect because of the time element; but other primitives will result in a much nicer output.

... but that is not a good excuse and I want my dates to look good too, I hear you say. You can of course override the custom title behavior by passing a custom title in, like we did before; but we also want the custom title to be input-parameter aware. In other words we want the title to be formed based on the provided parameters:

<pre>
[Test]
public void AlcoholIsNotSoldToMinorsTake4()
{
    this.Given(_ => GivenIWasBornOn(new DateTime(1998, 1, 1)), "Given I was born on {0:dd-MMM-yyyy}")
        .When(_ => WhenIAskToBuyAlcoholInALicencedPremiseOn(new DateTime(2012, 1, 1)), "When I ask to buy alcohol in a licenced premise on {0:dd-MMM-yyyy}")
        .Then(_ => ThenIDoNotGetTheAlocohol())
            .And(_ => AndIAmWalkedOut())
        .BDDfy("Alcohol is not sold to minors");
}
</pre>

This time I have overridden the default title by providing a custom one; but the custom title has a placeholder for my input parameter which also accepts [string formatter](http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=VS.100).aspx). The report looks like (this time I use the html report for a change):

![Input parameter with string formatter](/img/BDDfy/fluent-api-input-params/input-parameter-with-string-formatter.JPG)

The report this time is much more readable. I also used a date format that avoids confusion for American readers where 01-05-1998, for example, indicates 5th of January. The formatters are standard .Net formatters  and this allows you to make your reports more readable when dealing with things like currency, date, time, decimal numbers and so on. It is worth highlighting that you do not necessarily have to provide a format string and can only use the placeholders.

When you do not override the default title behavior BDDfy appends the input parameters at the end of the step title; but in some cases you want the parameter to appear in the middle of the title. For example assume I have a scenario where my 'Given' part should read as "Given I was born between 'May 1950' and 'June 1990'". If I wrote it as:

<pre>
this.Given(_ =>
    GivenIWasBornBetween(
        new DateTime(1950, 5, 1),
        new DateTime(1990, 6, 1))
</pre>

Then my title would be "Given I was born between 01/05/1950 12:00:00 AM, 01/06/1990 12:00:00 AM"!!! To get the result I wanted I would have to override my title as:

<pre>
this.Given(_ =>
    GivenIWasBornBetween(
        new DateTime(1950, 5, 1),
        new DateTime(1990, 6, 1),
    "Given I was born between '{0:MMMM yyyy}' and '{1:MMMM yyyy}'")
</pre>

which gives me "Given I was born between 'May 1950' and 'June 1990'" including the quotes.

So we can easily format our title using placeholders and formatters; but what if I do not want my input parameters to be shown in the report?!! There are cases where you want to provide input parameters to a method but do not want it to be included in the title. This is usually the case when you are reusing the same method, that takes input parameters, for several scenarios.

<pre>
[Test]
public void AlcoholIsNotSoldToMinorsParametersNotShownInTheReportTake1()
{
    this.Given(_ => GivenIAmAMinor(new DateTime(1998, 1, 1)))
        .When(_ => WhenIAskToBuyAlcoholInALicencedPremise())
        .Then(_ => ThenIDoNotGetTheAlocohol())
            .And(_ => AndIAmWalkedOut())
        .BDDfy("Alcohol is not sold to minors");
}

private void GivenIAmAMinor(DateTime dateOfBirth)
{
}

private void WhenIAskToBuyAlcoholInALicencedPremise()
{
}
</pre>

In this example, for the 'Given' step I do not care when "*I*" was born but still want to pass the date of birth to the step method. If I ran this scenario as is I would get:

![Not showing input parameters in the report - failed attempt](/img/BDDfy/fluent-api-input-params/avoiding-input-parameters-in-the-title-take-1.JPG)

The 'Given' part does not look good at all as the date should not appear in the end. To remove the date from the end I will use another overload. The [overload's signature for the <code>Given</code>](https://github.com/TestStack/TestStack.BDDfy/blob/master/TestStack.BDDfy/Scanners/StepScanners/Fluent/FluentScanner.cs#L145) method looks like:

<pre>
Given(Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle)
</pre>

... and my changed scenario is:

<pre>
[Test]
public void AlcoholIsNotSoldToMinorsParametersNotShownInTheReportTake2()
{
    this.Given(_ => GivenIAmAMinor(new DateTime(1998, 1, 1)), false)
        .When(_ => WhenIAskToBuyAlcoholInALicencedPremise())
        .Then(_ => ThenIDoNotGetTheAlocohol())
            .And(_ => AndIAmWalkedOut())
        .BDDfy("Alcohol is not sold to minors");
}
</pre>

which results into:

![Not showing input parameters in the report](/img/BDDfy/fluent-api-input-params/avoiding-input-parameters-in-the-title-take-2.JPG)
