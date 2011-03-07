using System.Text.RegularExpressions;

namespace Bddify
{
    public static class BddifyExtensions
    {
        public static void Bddify(this object bddee)
        {
            Bddifier.PrintOutput = Bddifier.DefaultPrintOutput;
            var bdder = new Bddifier();
            bdder.Run(bddee);
        }
    }
}