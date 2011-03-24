namespace Bddify.Scanners.GwtAttributes
{
    public class AndThenAttribute : GwtExectuableAttribute
    {
        public AndThenAttribute() : base(5)
        {
            Asserts = true;
        }
    }
}