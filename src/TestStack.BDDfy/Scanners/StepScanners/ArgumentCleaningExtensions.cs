using TestStack.BDDfy.Configuration;

namespace TestStack.BDDfy
{
    internal static class ArgumentCleaningExtensions
    {
        public const string NullValueRepresentation = "<null>";

        public static object[] FlattenArrays(this IEnumerable<object> inputs)
        {
            return [.. inputs.Select(FlattenArray)];
        }

        public static object FlattenArray(this object? input)
        {
            if (input is Array inputArray)
            {
                var temp = from object arrElement in inputArray select FormatElement(arrElement);
                return string.Join(", ", temp);
            }

            return input.GetSafeValue();
        }

        private static object FormatElement(object? element)
        {
            if (element is Array nestedArray)
            {
                var inner = from object item in nestedArray select FormatElement(item);
                return $"[{string.Join(", ", inner)}]";
            }

            return element.GetSafeValue();
        }

        private static object GetSafeValue(this object? input) => input switch
        {
            null => NullValueRepresentation,
            string s => s == string.Empty ? "<empty>" : (s.Trim() == string.Empty ? $"\"{s}\"" : s),
            _ => input
        };

        internal static string ToTextRepresentation(this object? value)
        {
            return value switch
            {
                null => NullValueRepresentation,
                IFormattable f => f.ToString(null, Configurator.CultureInfo),
                _ => value.ToString() ?? NullValueRepresentation
            };
        }
    }
}