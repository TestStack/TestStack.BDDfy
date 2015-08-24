using System;

namespace TestStack.BDDfy
{
    public interface IStoryMetadataScanner
    {
        StoryMetadata Scan(object testObject, Type explicitStoryType = null);
    }
}