namespace Bddify.Core
{
    public enum ExecutionOrder
    {
        SetupState = 1,
        ConsequentSetupState = 2,
        Transition = 3,
        ConsequentTransition = 4,
        Assertion = 5,
        ConsequentAssertion = 6
    }
}