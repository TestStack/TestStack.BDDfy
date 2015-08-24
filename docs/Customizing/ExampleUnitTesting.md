# Example - Using BDDfy for unit testing
# Introduction #
I’ve been using BDDfy with NUnit for writing acceptance tests for quite awhile now. But for unit testing I have continued to use mspec with [machine fakes](https://github.com/machine/machine.fakes) and Moq for auto-mocking. The more I used BDDfy, the more I liked it, and the less I liked the context switch into another framework. I found myself wanting to write unit tests in the Given When Then format and didn't like having to maintain two sets of helper code for NUnit and mspec. I loved the reporting in BDDfy and started to think it would be pretty cool to have similar reporting for my unit tests. Basically, I wanted a consistent experience across all my automated testing.

MSpec uses the [testcase class per fixture](http://xunitpatterns.com/Testcase%20Class%20per%20Fixture.html) style of testing, which is how I use BDDfy for acceptance testing, so it makes sense to continue with that style for the BDDfy unit tests. When I am doing acceptance tests I have a base ScenarioFor< T> class, where the T represents the System Under Test (SUT). Because these are full system tests, I resolve this SUT using the same inversion of control container that my application uses, which works nicely. The IoC container acts as a [SUT factory](http://blog.ploeh.dk/2009/02/13/SUTFactory/). With unit tests, I also want to have a SUT factory, but instead I want it to be an auto-mocking container.

When we started writing unit tests for the [Seleno](http://teststack.github.io/pages/Seleno.html) project, I thought it would be a good opportunity to try some of these ideas out.

# Specification Base Fixture #
The Specification base fixture class wires up BDDFy. It provides methods that BDDfy knows about in its default configuration for setting up and tearing down the fixture (each test class will implement its own specific Given When Then methods that BDDfy will also find). The Run method has the NUnit Test attribute and so will be called by the testing framework and it just calls BDDFy to run the test. I prefer to use NUnit myself, but you could just as easily substitute XUnit or MsTest attributes if you prefer. One thing I really like about this approach is that I only have to put the TestFixture and Test attributes in this one class and then all of the test classes I create inherit them and don't need any attributes. All the test runners still picks the classes up as tests and ReSharper even puts its little run test icons in each test class as normal. This class is also the one place that BDDfy gets called.

    [TestFixture]
    public abstract class Specification : ISpecification
    {
        [Test]
        public virtual void Run()
        {
            string title = BuildTitle();
            this.BDDfy(title, Category);
        }

        protected virtual string BuildTitle()
        {
            return Title ?? GetType().Name.Humanize(LetterCasing.Title);
        }

        // BDDfy methods
        public virtual void EstablishContext() { }
        public virtual void Setup() { }
        public virtual void TearDown() { }

        public virtual Type Story { get { return GetType(); } }
        public virtual string Title { get; set; }
        public string Category { get; set; }
    }

#Auto-Mocking #
The SpecificationFor<T> class inherits from the Specification class and adds an auto-mocking container for creating the SUT. An auto-mocking container decouples a unit test from the mechanics of creating the SUT and automatically supplies dynamic mocks in place of all of the SUT's dependencies. They are commonly implemented by combining an IoC container with a mocking framework, which is what I will be doing here. I prefer [NSubstitute](http://nsubstitute.github.io/) for mocking these days, so my friend [Rob Moore's](http://robdmoore.id.au/) [AutoSubstitute](http://nuget.org/packages/AutofacContrib.NSubstitute) auto-mocking container is ideal.

The AutoSubstitute field provides access to the container for full access to its functionality. SubstituteFor is provided as a convenience method to gain access to NSubstitute substitutes.

    public abstract class SpecificationFor<T> : Specification
    {
        public T SUT { get; set; }
        protected AutoSubstitute AutoSubstitute;

        protected SpecificationFor()
        {
            AutoSubstitute = CreateContainer();
            InitialiseSystemUnderTest();
        }

        public virtual void InitialiseSystemUnderTest()
        {
             SUT = AutoSubstitute.Resolve<T>();
        }

        public TSubstitute SubstituteFor<TSubstitute>() where TSubstitute : class
        {
            return AutoSubstitute.ResolveAndSubstituteFor<TSubstitute>();
        }

        public override Type Story
        {
            get { return typeof(T); }
        }

        private static AutoSubstitute CreateContainer()
        {
            Action<ContainerBuilder> autofacCustomisation = c => c
                .RegisterType<T>()
                .FindConstructorsWith(t =>  t.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                .PropertiesAutowired();
            return new AutoSubstitute(autofacCustomisation);
        }
    }

Notice that the InitialiseSystemUnderTest is virtual, so if you need to create the SUT yourself rather than having AutoSubstitute do it then you can just override it in your test class. It runs before any of the test methods so they can all safely interact with the SUT confident that it has been created.

# What's in a Story? #
When moving from scenarios to unit tests I had to think what I wanted to do with the Story that is part of BDD. BDDfy actually doesn't require stories, so one option was to do nothing. However, when you don't have a story the report shows the namespace instead, and I don't find that particularly helpful or attractive. A Story's purpose in BDD is to group related Scenarios and to provide metadata about that grouping, so I think the same logic can be applied to unit tests.

By creating a base specification class that related tests inherit from, they will all be grouped together on the report, and the name of that class will be used in the heading, much like a Story grouping would look like on a BDD report. This class is often a convenient place to create variables that are common to all of the test cases. So, for example, if you create a base class called PageNavigatorSpecification, the report grouping will read "Specifications For: Page Navigator." That is achieved by adding the following custom story metadata scanner.

    public class SpecStoryMetaDataScanner : IStoryMetaDataScanner
    {
        public virtual StoryMetaData Scan(object testObject, Type explicityStoryType = null)
        {
            var specification = testObject as ISpecification;
            if (specification == null)
                return null;

            string specificationTitle = CreateSpecificationTitle(specification);
            var story = new StoryAttribute() {Title = specificationTitle};
            return new StoryMetaData(specification.Story, story);
        }

        private string CreateSpecificationTitle(ISpecification specification)
        {
            string suffix = "Specification";
            string title = specification.Story.Name;
            if (title.EndsWith(suffix))
                title = title.Remove(title.Length - suffix.Length, suffix.Length);
            return title;
        }
    }

And here is what the specifications look like in the BDDfy report:

![BDDfy unit test report](/img/BDDfy/Customizing/bddfy-unit-test-report.png)

# Unit Testing #
Here is an example of some tests that we have written for Seleno using this approach. Firstly, an example of a Specification class, the grouping class analagous to the BDDFy story that all the PageReader specification classes will inherit from. There is normally no need to override the Story as the SpecificationFor class will automatically convert the generic T into an English name. In this case though it will convert PageReader< TestViewModel> into PageReader`, so it is a convenient hack to override the property with PageReaderSpecification which will more attractively produce "Page Reader" on the report.

    abstract class PageReaderSpecification : SpecificationFor<PageReader<TestViewModel>>
    {
        public override Type Story
        {
            get { return typeof (PageReaderSpecification); }
        }
    }

Each specification for the PageReader component inherits from the PageReaderSpecification. There are often no Given steps as the auto-mocking container has taken care of instantiating the SUT for you. It has also created a Substitute for the IExecutor dependency of the PageReader class and in the verification phase of the test you can just call SubstituteFor< IExecutor> to call NSubstitute verification methods on the substitute.  

    class When_checking_an_element_exists_and_is_visible_with_property : PageReaderSpecification
    {
        public When_checking_an_element_exists_and_is_visible_with_property()
        {
            SUT.ExistsAndIsVisible(x => x.Item);
        }

        public void Then_it_should_execute_the_relevant_script_with_jquery_id_selector()
        {
            SubstituteFor<IExecutor>()
                .Received()
                .ScriptAndReturn<bool>("$(\"#Item\").is(':visible')");
        }
    }

This test shows that you can also setup Substitute behaviour in the setup phase of the test.

	class When_getting_a_web_element_strongly_typed_text : PageReaderSpecification
    {
        private DateTime _result;
        private readonly DateTime _the03rdOfJanuary2012At21h21 = new DateTime(2012, 01, 03, 21, 21, 00);

        [Given("Given a web element contains the text 03/01/2012 21:21")]
        public void Given_a_web_element_contains_the_text_03_01_2012_21_21()
        {
            SubstituteFor<IElementFinder>()
                .Element(Arg.Any<By>())
                .Returns(SubstituteFor<IWebElement>());

            SubstituteFor<IWebElement>().Text.Returns("03/01/2012 21:21");
        }

        public void When_getting_the_web_element_matching_a_view_model_property()
        {
            _result = SUT.TextAsType(viewModel => viewModel.Modified);
        }

        public void Then_it_should_return_the_corresponding_typed_value_of_the_web_element_text()
        {
            _result.Should().Be(_the03rdOfJanuary2012At21h21);
        }
    }

# Another approach #
It's worth looking at another example of sharing a base context class. As I said above, it is not just for making the report work, it can be quite helpful to share context there. [Matt Honeycutt](http://trycatchfail.com/blog/) has an interesting style for reusing context classes in his very cool [SpecsFor BDD framework](http://specsfor.com/) that this approach also supports:

	public class given
    {
        public abstract class the_command_is_valid : SpecificationFor<CommandProcessor>
        {
            protected void Given_the_command_is_valid()
            {
                SubFor<IValidateCommand<TestCommand>>().Validate(Arg.Any<TestCommand>()).Returns(new ExecutionResult(null));
                SubFor<IValidateCommandFactory>().ValidatorForCommand(Arg.Any<TestCommand>()).Returns(SubFor<IValidateCommand<TestCommand>>());
            }
        }
    }

    public class processing_a__valid_command : given.the_command_is_valid
    {
        private TestCommand _command = new TestCommand();
        private ExecutionResult _result;

        public void when_processing_a_valid_command()
        {
            _result = SUT.Execute(_command);
        }

        public void Then_the_processor_should_find_the_validator_for_the_command()
        {
            SubFor<IValidateCommandFactory>().Received().ValidatorForCommand(_command);
        }

        public void AndThen_validate_the_command()
        {
            SubFor<IValidateCommand<TestCommand>>().Received().Validate(_command);
        }

        public void AndThen_the_processor_should_find_the_handler_for_the_command()
        {
            SubFor<IHandleCommandFactory>().Received().HandlerForCommand(_command);
        }

        public void AndThen_the_command_is_processed_successfully()
        {
            _result.IsSuccessful.Should().BeTrue();
        }

        public void AndThen_the_result_is_logged()
        {
            SubFor<ILog>().Received().Info(Arg.Any<string>());
        }
    }

And this comes out very nicely on the report:

![BDDfy unit test report](/img/BDDfy/Customizing/bddfy-unit-test-report-2.png)


You can find the code on [github](https://github.com/TestStack/TestStack.Seleno/tree/master/src/TestStack.Seleno.Tests/Specify).
