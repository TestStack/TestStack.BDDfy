using System;
using System.Reflection;
using Shouldly;
using TestStack.BDDfy.Processors;
using Xunit;

namespace TestStack.BDDfy.Tests.Configuration;

public class ExceptionResolverTests
{
    [Fact]
    public void WhenTargetInvocationException_ResolveRoot_WhenInnerNotAvailable()
    {
        var ex = new TargetInvocationException(null);
        var resolved = ExceptionResolver.Resolve(ex);
        resolved.ShouldBeOfType<TargetInvocationException>();
    }

    [Fact]
    public void WhenNotTargetInvocationException_ResolveRoot_EvenWhenInnerIsAvailable()
    {
        var ex = new System.InvalidCastException("error", new AmbiguousMatchException());
        var resolved = ExceptionResolver.Resolve(ex);
        resolved.ShouldBeOfType<InvalidCastException>();
    }
}
