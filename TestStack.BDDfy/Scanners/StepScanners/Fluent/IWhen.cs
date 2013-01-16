﻿using System;
using System.Linq.Expressions;

namespace TestStack.BDDfy.Scanners.StepScanners.Fluent
{
    public interface IWhen<TScenario> : IFluentScanner<TScenario>
    {
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, bool includeInputsInStepTitle);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, bool includeInputsInStepTitle);
#if NET35
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep);
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate);
#else
        IAndWhen<TScenario> And(Expression<Action<TScenario>> andWhenStep, string stepTextTemplate = null);
        IThen<TScenario> Then(Expression<Action<TScenario>> thenStep, string stepTextTemplate = null);
#endif
    }
}