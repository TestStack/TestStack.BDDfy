using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests.ScannerSpecs
{
    // ToDo: this should be cleaned up
    public class ScannerTests
    {
        private class TypeWithAttribute
        {
            [Given]
            void Given() { }

            [When]
            void When() { }

            [Then]
            void Then() { }
        }

        private class TypeWithoutAttribute
        {
            void Given() {}
            void When() {}
            void Then(){}
            void AndSomething() {}
        }

        [Test]
        public void ScannerReturnsMethodsWithAttributes()
        {
            var steps = new GwtScanner().Scan(typeof(TypeWithAttribute)).ToList();
            Assert.That(steps.Count, Is.EqualTo(3));
            Assert.That(steps[0].Method.Name, Is.EqualTo("Given"));
            Assert.That(steps[1].Method.Name, Is.EqualTo("When"));
            Assert.That(steps[2].Method.Name, Is.EqualTo("Then"));
        }

        [Test]
        public void ScannerReturnsMethodsWithoutAttributes()
        {
            var steps = new GwtScanner().Scan(typeof(TypeWithoutAttribute)).ToList();
            Assert.That(steps.Count, Is.EqualTo(4));
            Assert.That(steps[0].Method.Name, Is.EqualTo("Given"));
            Assert.That(steps[1].Method.Name, Is.EqualTo("When"));
            Assert.That(steps[2].Method.Name, Is.EqualTo("Then"));
            Assert.That(steps[3].Method.Name, Is.EqualTo("AndSomething"));
        }
    }
}