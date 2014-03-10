using System;

namespace TestStack.BDDfy.Core
{
    public class StoryMetaData
    {
// ReSharper disable InconsistentNaming
        private const string I_want_prefix = "I want ";
        private const string So_that_prefix = "So that ";
        // ReSharper restore InconsistentNaming

        public StoryMetaData(Type storyType, StoryAttribute storyAttribute)
        {
            var title = storyAttribute.Title;
            if (string.IsNullOrEmpty(title))
                title = NetToString.Convert(storyType.Name);

            Type = storyType;
            Title = title;

            SetAsA(storyAttribute.AsA);
            SetIWant(storyAttribute.IWant);
            SetSoThat(storyAttribute.SoThat);
        }

        public StoryMetaData(Type storyType, string asA, string iWant, string soThat, string storyTitle = null)
        {
            Title = storyTitle ?? NetToString.Convert(storyType.Name);
            Type = storyType;

            SetAsA(asA);
            SetIWant(iWant);
            SetSoThat(soThat);
        }

        void SetAsA(string asA)
        {
            if (string.IsNullOrWhiteSpace(asA))
                return;

            if (!asA.StartsWith("As a", StringComparison.OrdinalIgnoreCase))
                AsA = "As a ";

            AsA += asA;
        }

        void SetIWant(string iWant)
        {
            if (string.IsNullOrWhiteSpace(iWant))
                return;

            if (!iWant.StartsWith(I_want_prefix, StringComparison.OrdinalIgnoreCase))
                IWant = I_want_prefix;

            IWant += iWant;
        }

        void SetSoThat(string soThat)
        {
            if(string.IsNullOrWhiteSpace(soThat))
                return;

            if (!soThat.StartsWith(So_that_prefix, StringComparison.OrdinalIgnoreCase))
                SoThat = So_that_prefix;

            SoThat += soThat;
        }

        public Type Type { get; private set; }
        public string Title { get; private set; }
        public string AsA { get; private set; }
        public string IWant { get; private set; }
        public string SoThat { get; private set; }
    }
}