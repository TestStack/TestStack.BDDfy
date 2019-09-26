using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestStack.BDDfy.Configuration
{
    public class Scanners
    {
        public readonly StepScannerFactory _ = new StepScannerFactory(() => new ExecutableAttributeStepScanner());
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
        internal Scanners SetCustomStepTitleCreatorFunction( Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> customStepTitleCreatorFunction)
        {
            CustomStepTitleCreatorFunction= customStepTitleCreatorFunction;
            return this;
        }

        public Scanners SetDefaultStepTitleCreatorFunction(Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> defaultStepTitleCreatorFunction)
        {
            DefaultStepTitleCreatorFunction = defaultStepTitleCreatorFunction;
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

        public Func<IStoryMetadataScanner> StoryMetadataScanner = () => new StoryAttributeMetadataScanner();

        internal Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> CustomStepTitleCreatorFunction = null;
        internal Func<string, bool, MethodInfo, StepArgument[], string, StepTitle> DefaultStepTitleCreatorFunction = null;

        public Func<string, string> Humanize = NetToString.Convert;
    }
}