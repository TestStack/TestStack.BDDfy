using System;

namespace TestStack.BDDfy.Configuration
{
    public abstract class ComponentFactory<TComponent, TMaterial> where TComponent : class 
    {
        private const bool DefaultState = true;
        private bool _active = DefaultState;
        private Predicate<TMaterial> _runsOn = o => true;
        readonly Func<TComponent> _factory;
        internal ComponentFactory(Func<TComponent> factory):this(factory, DefaultState) { }

        internal ComponentFactory(Func<TComponent> factory, bool active)
        {
            _factory = factory;
            _active = active;
        }

        public void Disable()
        {
            _active = false;
        }

        public void Enable()
        {
            _active = true;
        }

        public TComponent ConstructFor(TMaterial material)
        {
            if (_active && _runsOn(material))
                return _factory();

            return null;
        }

        public void RunsOn(Predicate<TMaterial> runOn)
        {
            _runsOn = runOn;
        }
    }
}