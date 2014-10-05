using System;

namespace TestStack.BDDfy
{
    public class StepArgument
    {
        private readonly Action<object> _set = o => { };
        private readonly Func<object> _get;

        public StepArgument(string name, Type argumentType, Func<object> getValue, Action<object> setValue)
        {
            Name = name;
            _get = getValue;
            if (setValue != null)
                _set = setValue;
            ArgumentType = argumentType;
        }

        public StepArgument(Func<object> value)
        {
            _get = value;
            ArgumentType = typeof(object);
        }

        public string Name { get; private set; }

        public object Value
        {
            get
            {
                try
                {
                    return _get();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public Type ArgumentType { get; private set; }

        public void SetValue(object newValue)
        {
            _set(newValue);
        }
    }
}