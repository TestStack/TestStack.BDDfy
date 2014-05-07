using System;
using System.Reflection;

namespace TestStack.BDDfy
{
    public class StepArgument
    {
        private readonly Action<object> _set = o => { };
        private readonly Func<object> _get;

        public StepArgument(FieldInfo member, object declaringObject)
        {
            Name = member.Name;
            _get = () => member.GetValue(declaringObject);
            _set = o => member.SetValue(declaringObject, o);
            ArgumentType = member.FieldType;
        }

        public StepArgument(PropertyInfo member, object declaringObject)
        {
            Name = member.Name;
            _get = () => member.GetGetMethod(true).Invoke(declaringObject, null);
            _set = o => member.GetSetMethod(true).Invoke(declaringObject, new[] { o });
            ArgumentType = member.PropertyType;
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