namespace TestStack.BDDfy
{
    public static class WithExamplesExtensions
    {
        public static IExampleTable WithExamples(this object testObject, ExampleTable table)
        {
            table.TestObject = testObject;
            return table;
        }
    }
}
