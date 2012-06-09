﻿using System;
using System.Collections.Generic;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners;
using TestStack.BDDfy.Scanners.StepScanners;
using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute;
using TestStack.BDDfy.Scanners.StepScanners.MethodName;

namespace TestStack.BDDfy.Configuration
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

        public Func<string, string> Humanize = NetToString.Convert;
    }
}