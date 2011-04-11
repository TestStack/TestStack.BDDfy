using System;

namespace Bddify.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class StoryAttribute : Attribute
    {
        private readonly string _title;
        private readonly string _asA;
        private readonly string _iWant;
        private readonly string _soThat;

        public StoryAttribute(string title, string asA, string iWant, string soThat)
        {
            _title = title;
            _asA = asA;
            _iWant = iWant;
            _soThat = soThat;
        }

        public string AsA
        {
            get { return _asA; }
        }

        public string IWant
        {
            get { return _iWant; }
        }

        public string SoThat
        {
            get { return _soThat; }
        }

        public string Title
        {
            get { return _title; }
        }
    }
}