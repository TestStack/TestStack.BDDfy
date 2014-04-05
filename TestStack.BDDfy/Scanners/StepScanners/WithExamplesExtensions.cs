using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static IExamples WithExamples(this object testObject, string[] headers, params object[][] examples)
        {
            return new Examples(testObject, headers, examples);
        }
    }

    public interface IExamples
    {
        object TestObject { get; }
        string[] ExampleHeaders { get; }
        object[][] ExampleRows { get; }
    }

    class Examples : IExamples
    {
        public object TestObject { get; set; }
        public string[] ExampleHeaders { get; set; }
        public object[][] ExampleRows { get; set; }

        public Examples(string[] exampleHeaders, params object[][] examples)
        {
            ExampleHeaders = exampleHeaders;
            ExampleRows = examples;
        }

        public Examples(object testObject, string[] exampleHeaders, params object[][] examples)
        {
            TestObject = testObject;
            ExampleHeaders = exampleHeaders;
            ExampleRows = examples;
        }
    }
}
