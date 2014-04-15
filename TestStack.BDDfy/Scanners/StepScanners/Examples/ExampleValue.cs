using System;

namespace TestStack.BDDfy
{
    public class ExampleValue
    {
        private readonly object _underlyingValue;

        public ExampleValue(string header, object underlyingValue)
        {
            Header = header;
            _underlyingValue = underlyingValue;
        }

        public string Header { get; private set; }

        public object GetExampleValue(Type targetType)
        {
            var stringValue = _underlyingValue as string;
            if (_underlyingValue == null || (_underlyingValue is string && string.IsNullOrEmpty(stringValue)))
            {
                if (targetType.IsValueType &&
                    !(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof (Nullable<>)))
                {
                    var valueAsString = string.IsNullOrEmpty(stringValue) ? "<null>" : string.Format("\"{0}\"", _underlyingValue);
                    throw new ArgumentException(string.Format("Cannot convert {0} to {1}", valueAsString, targetType.Name));
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