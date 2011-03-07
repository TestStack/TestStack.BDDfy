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

        public static string CreateSentenceFromCamelName(string name)
        {
            return Regex.Replace(name, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
    }
}