namespace Bddify.Scanners
{
    public class GwtExectuableAttribute : ExecutableAttribute
    {
        protected const string And = " and";

        public GwtExectuableAttribute(int order) : base(order)
        {
        }
    }

    public class GivenAttribute : GwtExectuableAttribute
    {
        public GivenAttribute() : base(1) { }
    }

    public class AndGivenAttribute : GwtExectuableAttribute
    {
        public AndGivenAttribute() : base(2) { }
    }

    public class WhenAttribute : GwtExectuableAttribute
    {
        public WhenAttribute() : base(3) { }
    }

    public class AndWhenAttribute : GwtExectuableAttribute
    {
        public AndWhenAttribute() : base(4) { }
    }

    public class ThenAttribute : GwtExectuableAttribute
    {
        public ThenAttribute() : base(5)
        {
            Asserts = true;
        }
    }

    public class AndThenAttribute : GwtExectuableAttribute
    {
        public AndThenAttribute() : base(5)
        {
            Asserts = true;
        }
    }
}