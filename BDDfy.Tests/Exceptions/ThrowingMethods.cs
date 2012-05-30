using System;

namespace BDDfy.Tests.Exceptions
{
    [Flags]
    public enum ThrowingMethods
    {
        None = 0,
        Given = 1,
        When = 2,
        Then = 4
    }
}