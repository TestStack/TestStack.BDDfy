namespace TestStack.BDDfy
{
    public class ExampleValue(string header, object? underlyingValue, Func<int> getRowIndex)
    {
        private readonly object? _underlyingValue = underlyingValue;
        private readonly Func<int> _getRowIndex = getRowIndex;

        public string Header { get; private set; } = header;

        public bool MatchesName(string? name) => ExampleTable.HeaderMatches(Header, name);

        public int Row => _getRowIndex() + 1;

        public object? GetValue(Type targetType)
        {
            if (_underlyingValue is null)
            {
                if (targetType.IsValueType() && !(targetType.IsGenericType() && targetType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    throw new ArgumentException(string.Format("Cannot convert <null> to {0} (Column: '{1}', Row: {2})", targetType.Name, Header, Row));
                }

                ValueHasBeenUsed = true;
                return default;
            }

            var valueIsString = _underlyingValue is string;

            ValueHasBeenUsed = true;
            if (targetType.IsInstanceOfType(_underlyingValue))
                return _underlyingValue;

            if (targetType.IsEnum() && valueIsString)
                return Enum.Parse(targetType, (string)_underlyingValue);

            if (targetType == typeof(DateTime) && valueIsString)
                return DateTime.Parse((string)_underlyingValue);

            try
            {
                return Convert.ChangeType(_underlyingValue, targetType);
            }
            catch (InvalidCastException ex)
            {
                throw new UnassignableExampleException(string.Format(
                    "{0} cannot be assigned to {1} (Column: '{2}', Row: {3})", 
                    _underlyingValue?.ToString() ?? "<null>",
                    targetType.Name, Header, Row), ex, this);
            }
        }

        public bool ValueHasBeenUsed { get; private set; }

        public bool IsCompatibleWith(Type targetType)
        {
            if (_underlyingValue is null)
                return !targetType.IsValueType() || (targetType.IsGenericType() && targetType.GetGenericTypeDefinition() == typeof(Nullable<>));

            if (targetType.IsInstanceOfType(_underlyingValue))
                return true;

            if (_underlyingValue is string stringValue)
            {
                if (targetType.IsEnum())
                    return Enum.IsDefined(targetType, stringValue) || Enum.GetNames(targetType).Any(n => n.Equals(stringValue, StringComparison.OrdinalIgnoreCase));

                if (targetType == typeof(DateTime))
                    return DateTime.TryParse(stringValue, out _);
            }

            try
            {
                Convert.ChangeType(_underlyingValue, targetType);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Value => GetValueAsString();

        public override string ToString() => string.Join("{0}: {1}", Header, _underlyingValue);

        public string GetValueAsString() => _underlyingValue.FlattenArray().ToTextRepresentation();
    }
}