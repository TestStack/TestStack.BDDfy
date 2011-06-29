using System;

namespace Bddify.Tests.Exceptions
{
    [Flags]
    public enum ThrowingMethods
    {
        None,
        Given,
        When,
        Then
    }
}