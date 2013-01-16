using System;
using TestStack.BDDfy.Core;

namespace TestStack.BDDfy.Scanners
{
    public interface IStoryMetaDataScanner
    {
        StoryMetaData Scan(object testObject, Type explicitStoryType = null);
    }
}