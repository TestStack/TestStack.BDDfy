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
// (INCLUDING, BUT NOT LIMITED TO, PROCULREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using Bddify.Scanners;
using Bddify.Scanners.StepScanners;
using Bddify.Scanners.StepScanners.ExecutableAttribute;
using Bddify.Scanners.StepScanners.MethodName;

namespace Bddify.Configuration
{
    public class Scanners
    {
        private readonly StepScannerFactory _executableAttributeScanner = new StepScannerFactory(() => new ExecutableAttributeStepScanner());
        public StepScannerFactory ExecutableAttributeScanner { get { return _executableAttributeScanner; } }

        private readonly StepScannerFactory _methodNameStepScanner = new StepScannerFactory(() => new DefaultMethodNameStepScanner());
        public StepScannerFactory DefaultMethodNameStepScanner { get { return _methodNameStepScanner; } }

        private readonly List<Func<IStepScanner>> _addedStepScanners = new List<Func<IStepScanner>>();
        public Scanners Add(Func<IStepScanner> stepScannerFactory)
        {
            _addedStepScanners.Add(stepScannerFactory);
            return this;
        }

        public IEnumerable<IStepScanner> GetStepScanners(object objectUnderTest)
        {
            var execAttributeScanner = ExecutableAttributeScanner.ConstructFor(objectUnderTest);
            if (execAttributeScanner != null)
                yield return execAttributeScanner;

            var defaultMethodNameScanner = DefaultMethodNameStepScanner.ConstructFor(objectUnderTest);
            if (defaultMethodNameScanner != null)
                yield return defaultMethodNameScanner;

            foreach (var addedStepScanner in _addedStepScanners)
            {
                yield return addedStepScanner();
            }
        }

        public Func<IStoryMetaDataScanner> StoryMetaDataScanner = () => new StoryAttributeMetaDataScanner();
    }
}