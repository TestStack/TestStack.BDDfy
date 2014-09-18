using System;
using System.Reflection;

namespace TestStack.BDDfy
{
    public class StepArgument
    {
        private readonly Action<object> _set = o => { };
        private readonly Func<object> _get;

        public StepArgument(FieldInfo member, Func<object> declaringObject)
        {
            Name = member.Name;
            _get = () => member.GetValue(declaringObject == null ? null : declaringObject());
            _set = o => member.SetValue(declaringObject == null ? null : declaringObject(), o);
            ArgumentType = member.FieldType;
        }

        public StepArgument(PropertyInfo member, Func<object> declaringObject)
        {
            Name = member.Name;
            _get = () =>
            {
                if (declaringObject == null)
                    return member.GetGetMethod(true).Invoke(null, null);

                var declaringObjectValue = declaringObject();
                if (declaringObjectValue == null)
                    return null;
                return member.GetGetMethod(true).Invoke(declaringObjectValue, null);
            };
            _set = o =>
            {
                if (declaringObject == null)
                {
                    member.GetSetMethod(true).Invoke(null, new[] {o});
                    return;
                }

                var declaringObjectValue = declaringObject();
                if (declaringObjectValue == null)
                    return;
                member.GetSetMethod(true).Invoke(declaringObjectValue, new[] { o });
            };
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