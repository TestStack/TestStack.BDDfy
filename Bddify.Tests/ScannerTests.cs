using NUnit.Framework;
using System.Linq;

namespace Bddify.Tests
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
            var methods = new Scanner().Scan(typeof(TypeWithAttribute)).ToList();
            Assert.That(methods.Count, Is.EqualTo(3));
            Assert.That(methods[0].Name, Is.EqualTo("Given"));
            Assert.That(methods[1].Name, Is.EqualTo("When"));
            Assert.That(methods[2].Name, Is.EqualTo("Then"));
        }

        [Test]
        public void ScannerReturnsMethodsWithoutAttributes()
        {
            var methods = new Scanner().Scan(typeof(TypeWithoutAttribute)).ToList();
            Assert.That(methods.Count, Is.EqualTo(4));
            Assert.That(methods[0].Name, Is.EqualTo("Given"));
            Assert.That(methods[1].Name, Is.EqualTo("When"));
            Assert.That(methods[2].Name, Is.EqualTo("Then"));
            Assert.That(methods[3].Name, Is.EqualTo("AndSomething"));
        }
    }
}