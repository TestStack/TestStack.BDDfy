using System;

namespace TestStack.BDDfy
{
    public interface IStoryMetaDataScanner
    {
        StoryMetaData Scan(object testObject, Type explicitStoryType = null);
    }
}