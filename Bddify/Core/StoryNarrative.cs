namespace Bddify.Core
{
    public class StoryNarrative
    {
        public StoryNarrative(string title, string asA, string want, string soThat)
        {
            Title = title;
            AsA = asA;
            IWant = want;
            SoThat = soThat;
        }

        public string Title { get; private set; }
        public string AsA { get; private set; }
        public string IWant { get; private set; }
        public string SoThat { get; private set; }
    }
}