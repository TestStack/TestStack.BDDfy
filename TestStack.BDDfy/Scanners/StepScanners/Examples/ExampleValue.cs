using System;

namespace TestStack.BDDfy
{
    public class ExampleValue
    {
        private readonly object _underlyingValue;
        private readonly Func<int> _getRowIndex;

        public ExampleValue(string header, object underlyingValue, Func<int> getRowIndex)
        {
            Header = header;
            _underlyingValue = underlyingValue;
            _getRowIndex = getRowIndex;
        }

        public string Header { get; private set; }

        public bool MatchesName(string name)
        {
            return Sanitise(name).Equals(Sanitise(Header), StringComparison.InvariantCultureIgnoreCase);
        }

        private string Sanitise(string value)
        {
            return value.Replace(" ", string.Empty).Replace("_", string.Empty);
        }

        public object GetValue(Type targetType)
        {
            var stringValue = _underlyingValue as string;
            if (_underlyingValue == null || (_underlyingValue is string && string.IsNullOrEmpty(stringValue)))
            {
                if (targetType.IsValueType &&
                    !(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof (Nullable<>)))
                {
                    var valueAsString = string.IsNullOrEmpty(stringValue) ? "<null>" : string.Format("\"{0}\"", _underlyingValue);
                    throw new ArgumentException(string.Format("Cannot convert {0} to {1} (Column: '{2}', Row: {3})", valueAsString, targetType.Name, Header, _getRowIndex()));
                }
                return null;
            }

            if (targetType.IsInstanceOfType(_underlyingValue))
                return _underlyingValue;

            if (targetType.IsEnum && _underlyingValue is string)
                return Enum.Parse(targetType, (string)_underlyingValue);

            if (targetType == typeof (DateTime))
                return DateTime.Parse(stringValue);

            return Convert.ChangeType(_underlyingValue, targetType);
        }

        public override string ToString()
        {
            return string.Join("{0}: {1}", Header, _underlyingValue);
        }
    }
}