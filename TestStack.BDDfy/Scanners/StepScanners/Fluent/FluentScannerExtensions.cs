// Copyright (C) 2011, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Linq.Expressions;

// ReSharper disable CheckNamespace
// This is in BDDfy namespace to make its usage simpler
namespace TestStack.BDDfy.Scanners.StepScanners.Fluent
// ReSharper restore CheckNamespace
{
    public static class FluentStepScannerExtensions
    {
        static IInitialStep<TScenario> Scan<TScenario>(this TScenario testObject) where TScenario : class, new()
        {
            return new FluentScanner<TScenario>(testObject);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep, string stepTextTemplate)
            where TScenario: class, new()
        {
            return testObject.Scan().Given(givenStep, stepTextTemplate);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep, bool includeInputsInStepTitle)
            where TScenario: class, new()
        {
            return testObject.Scan().Given(givenStep, includeInputsInStepTitle);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep, string stepTextTemplate)
            where TScenario : class, new()
        {
            return testObject.Scan().When(whenStep, stepTextTemplate);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep, bool includeInputsInStepTitle)
            where TScenario : class, new()
        {
            return testObject.Scan().When(whenStep, includeInputsInStepTitle);
        }

        public static IGiven<TScenario> Given<TScenario>(this TScenario testObject, Expression<Action<TScenario>> givenStep)
            where TScenario: class, new()
        {
            return testObject.Given(givenStep, null);
        }

        public static IWhen<TScenario> When<TScenario>(this TScenario testObject, Expression<Action<TScenario>> whenStep)
            where TScenario : class, new()
        {
            return testObject.When(whenStep, null);
        }
    }
}