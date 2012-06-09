namespace TestStack.BDDfy.Core
{
    public enum ExecutionOrder
    {
        SetupState = 1,
        ConsecutiveSetupState = 2,
        Transition = 3,
        ConsecutiveTransition = 4,
        Assertion = 5,
        ConsecutiveAssertion = 6,
        TearDown = 7
    }
}