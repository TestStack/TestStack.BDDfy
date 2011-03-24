namespace Bddify.Scanners.GwtAttributes
{
    public class ThenAttribute : GwtExectuableAttribute
    {
        public ThenAttribute() : base(5)
        {
            Asserts = true;
        }
    }
}