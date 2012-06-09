using System;

namespace TestStack.BDDfy.Configuration
{
    public class ComponentFactory<TComponent, TMaterial> where TComponent : class 
    {
        private bool _active = true;
        private Predicate<TMaterial> _runsOn = o => true;
        readonly Func<TComponent> _factory;
        internal ComponentFactory(Func<TComponent> factory)
        {
            _factory = factory;
        }

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
            if (!_active || !_runsOn(material))
                return null;

            return _factory();
        }

        public void RunsOn(Predicate<TMaterial> runOn)
        {
            _runsOn = runOn;
        }
    }
}