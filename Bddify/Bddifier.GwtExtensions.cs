namespace Bddify
{
    public class GwtExectuableAttribute : ExecutableAttribute
    {
        protected const string And = " and";

        public GwtExectuableAttribute(int order, string text) : base(order, text)
        {
            TextPad = 5;
        }
    }

    public class GivenAttribute : GwtExectuableAttribute
    {
        public GivenAttribute() : base(1, "Given") { }
    }

    public class AndGivenAttribute : GwtExectuableAttribute
    {
        public AndGivenAttribute() : base(2, And) { }
    }

    public class WhenAttribute : GwtExectuableAttribute
    {
        public WhenAttribute() : base(3, "When") { }
    }

    public class AndWhenAttribute : GwtExectuableAttribute
    {
        public AndWhenAttribute() : base(4, And) { }
    }

    public class ThenAttribute : GwtExectuableAttribute
    {
        public ThenAttribute() : base(5, "Then")
        {
            Asserts = true;
        }
    }

    public class AndThenAttribute : GwtExectuableAttribute
    {
        public AndThenAttribute() : base(5, And)
        {
            Asserts = true;
        }
    }
}