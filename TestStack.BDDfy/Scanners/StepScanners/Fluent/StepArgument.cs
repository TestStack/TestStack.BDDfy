using System;
using System.Reflection;

namespace TestStack.BDDfy
{
    public class StepArgument
    {
        private readonly Action<object> _set = o => { };

        public StepArgument(FieldInfo member, object declaringObject)
        {
            Name = member.Name;
            Value = member.GetValue(declaringObject);
            _set = o => member.SetValue(declaringObject, o);
            ArgumentType = member.FieldType;
        }

        public StepArgument(PropertyInfo member, object declaringObject)
        {
            Name = member.Name;
            Value = member.GetGetMethod(true).Invoke(declaringObject, null);
            _set = o => member.GetSetMethod(true).Invoke(declaringObject, new[] { o });
            ArgumentType = member.PropertyType;
        }

        public StepArgument(object value)
        {
            Value = value;
            ArgumentType = typeof(object);
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public Type ArgumentType { get; private set; }

        public void SetValue(object newValue)
        {
            _set(newValue);
        }
    }
}