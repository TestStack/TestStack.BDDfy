﻿namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static ExampleTable WithExamples(this object testObject, ExampleTable table)
        {
            table.TestObject = testObject;
            return table;
        }

        public static ExampleTable WithExamples(this object testObject, string table)
        {
            return testObject.WithExamples(ExampleTable.Parse(table));
        }
    }
}
