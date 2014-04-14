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
            if (_underlyingValue == null || (_underlyingValue is string && string.IsNullOrEmpty(_underlyingValue as string)))
            {
                if (targetType.IsValueType && !(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    throw new ArgumentException("Cannot convert TODO");
                return null;
            }

            if (targetType.IsInstanceOfType(_underlyingValue))
                return _underlyingValue;

            return Convert.ChangeType(_underlyingValue, targetType);
        }

        public override string ToString()
        {
            return string.Join("{0}: {1}", Header, _underlyingValue);
        }
    }
}