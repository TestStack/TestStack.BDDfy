using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Bddify
{
    public class Bddifier
    {
        public static readonly Action<string> DefaultPrintOutput = Console.WriteLine;
        public static Action<string> PrintOutput = DefaultPrintOutput;

        public static Func<string, string> CreateSentenceFromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_' }));
        public static Func<string, string> CreateSentenceFromCamelName = name => Regex.Replace(name, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        public static Func<string, string> CreateSentenceFromName = name => CreateSentenceFromCamelName(CreateSentenceFromUnderscoreSeparatedWords(name));
        public static Func<string, string> CreateSentenceFromTypeName = CreateSentenceFromName;
        public static Func<MethodInfo, string> ReportByMethodInfo =
            method =>
            {
                var header = (ExecutableAttribute)method.GetCustomAttributes(typeof(ExecutableAttribute), false).FirstOrDefault();
                if (header == null)
                    return CreateSentenceFromName(method.Name);

                return string.Format("{0} {1}", header.Text.PadRight(header.TextPad), CreateSentenceFromName(method.Name));
            };

        private readonly IEnumerable<IProcessor> _processors;
        private readonly object _testObject;
        private readonly IScanner _scanner;

        public Bddifier(object testObject, IScanner scanner, IEnumerable<IProcessor> processors)
        {
            _processors = processors;
            _testObject = testObject;
            _scanner = scanner;
        }

        public void Run()
        {
            var steps = _scanner.Scan(_testObject.GetType());
            Bddee = new Bddee(_testObject, steps);

            //run processors in the right order regardless of the order they are provided to the Bddifer
            foreach (var processor in _processors.OrderBy(p => (int)p.ProcessType))
                processor.Process(Bddee);
        }

        public Bddee Bddee { get; private set; }
    }
}