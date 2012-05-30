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
using BDDfy.Core;

namespace BDDfy.Scanners.StepScanners.MethodName
{
    public class DefaultMethodNameStepScanner : MethodNameStepScanner
    {
        public DefaultMethodNameStepScanner()
            : base(
                CleanupTheStepText,
                new[]
                {
                    new MethodNameMatcher(s => s.EndsWith("Context", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false),
                    new MethodNameMatcher(s => s.Equals("Setup", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, false),
                    new MethodNameMatcher(s => s.StartsWith("Given", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.SetupState, true),
                    new MethodNameMatcher(s => s.StartsWith("AndGiven", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveSetupState, true),
                    new MethodNameMatcher(s => s.StartsWith("And_Given_", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveSetupState, true),
                    new MethodNameMatcher(s => s.StartsWith("When", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.Transition, true),
                    new MethodNameMatcher(s => s.StartsWith("AndWhen", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveTransition, true),
                    new MethodNameMatcher(s => s.StartsWith("And_When_", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.ConsecutiveTransition, true),
                    new MethodNameMatcher(s => s.StartsWith("Then", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.Assertion, true),
                    new MethodNameMatcher(s => s.StartsWith("And", StringComparison.OrdinalIgnoreCase), true, ExecutionOrder.ConsecutiveAssertion, true),
                    new MethodNameMatcher(s => s.StartsWith("TearDown", StringComparison.OrdinalIgnoreCase), false, ExecutionOrder.TearDown, false),
                })
        {
        }

        static string CleanupTheStepText(string stepText)
        {
            if (stepText.StartsWith("and given ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "given ".Length);

            if (stepText.StartsWith("and when ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and ".Length, "when ".Length);

            if (stepText.StartsWith("AndGiven ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and".Length, "given".Length);

            if (stepText.StartsWith("AndWhen ", StringComparison.OrdinalIgnoreCase))
                return stepText.Remove("and".Length, "when".Length);

            return stepText;
        }
    }
}