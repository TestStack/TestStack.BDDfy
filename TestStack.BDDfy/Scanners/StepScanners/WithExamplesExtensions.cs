using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static IExamples WithExamples(this object testObject, params object[][] examples)
        {
            return new Examples(testObject, examples);
        }
    }

    public interface IExamples
    {
        object TestObject { get; set; }
        object[][] ExampleRows { get; set; }
    }

    class Examples : IExamples
    {
        public object TestObject { get; set; }
        public object[][] ExampleRows { get; set; }

        public Examples(params object[][] examples)
        {
            ExampleRows = examples;
        }

        public Examples(object testObject, params object[][] examples)
        {
            TestObject = testObject;
            ExampleRows = examples;
        }
    }
}
