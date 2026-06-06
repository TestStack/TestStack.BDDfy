using System.Diagnostics;
using System.Reflection;

namespace TestStack.BDDfy.Scanners.ScenarioScanners
{
    /// <summary>
    /// Detects parameterized test methods ([InlineData], [TestCase], [Theory], etc.)
    /// and synthesizes an <see cref="Example"/> from their current parameter values
    /// so that separate test invocations are reported as examples of the same scenario.
    /// </summary>
    internal static class ParameterizedTestDetector
    {
        // Attribute type names that indicate parameterized test data (matched by name to avoid hard dependencies)
        private static readonly string[] DataAttributeNames =
        [
            "InlineDataAttribute",      // xUnit
            "TestCaseAttribute",        // NUnit
            "DataRowAttribute",         // MSTest
            "MemberDataAttribute",      // xUnit (member-based)
            "TestCaseSourceAttribute",  // NUnit (source-based)
            "DynamicDataAttribute"      // MSTest (dynamic)
        ];

        private static readonly string[] TheoryAttributeNames =
        [
            "TheoryAttribute",          // xUnit
            "TestAttribute",            // NUnit (when paired with TestCase)
            "DataTestMethodAttribute"   // MSTest
        ];

        /// <summary>
        /// Attempts to detect if the calling test method is parameterized.
        /// Returns the test method info and its current parameter values if detected.
        /// </summary>
        public static ParameterizedTestInfo? Detect(object testObject)
        {
            var testType = testObject.GetType();
            var (method, declaringObject) = FindTestMethod(testType, testObject);
            if (method is null) return null;

            if (!IsParameterizedTestMethod(method)) return null;

            var parameters = method.GetParameters();
            if (parameters.Length == 0) return null;

            // Build example from current parameter values (resolved from the appropriate object)
            var example = BuildExampleFromParameters(declaringObject!, parameters);
            if (example is null) return null;

            // Stable ID based on declaring type + method name (shared across invocations)
            var declaringType = method.DeclaringType ?? testType;
            var stableId = $"scenario-{declaringType.FullName}.{method.Name}".GetHashCode().ToString("x8");

            return new ParameterizedTestInfo(method, stableId, example);
        }

        private static (MethodInfo? method, object? resolvedObject) FindTestMethod(Type testType, object testObject)
        {
            // Walk the call stack to find the test method
            var stackTrace = new StackTrace(false);
            var frames = stackTrace.GetFrames();

            for (int i = frames.Length - 1; i >= 0; i--)
            {
                var frameMethod = frames[i].GetMethod();
                if (frameMethod is not MethodInfo mi || frameMethod.DeclaringType is null)
                    continue;

                if (!IsParameterizedTestMethod(mi))
                    continue;

                // Only detect when the parameterized method belongs to the test object's type (or base)
                if (frameMethod.DeclaringType == testType || testType.IsSubclassOf(frameMethod.DeclaringType))
                    return (mi, testObject);
            }

            return (null, null);
        }

        private static bool IsParameterizedTestMethod(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(true);
            bool hasDataAttribute = false;
            bool hasTheoryAttribute = false;

            foreach (var attr in attributes)
            {
                var attrTypeName = attr.GetType().Name;

                foreach (var name in DataAttributeNames)
                {
                    if (string.Equals(attrTypeName, name, StringComparison.Ordinal))
                    {
                        hasDataAttribute = true;
                        break;
                    }
                }

                foreach (var name in TheoryAttributeNames)
                {
                    if (string.Equals(attrTypeName, name, StringComparison.Ordinal))
                    {
                        hasTheoryAttribute = true;
                        break;
                    }
                }

                if (hasDataAttribute) break;
            }

            // Must have at least a data attribute or (theory + parameters)
            return hasDataAttribute || (hasTheoryAttribute && method.GetParameters().Length > 0);
        }

        private static Example? BuildExampleFromParameters(object testObject, ParameterInfo[] parameters)
        {
            var type = testObject.GetType();
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            var values = new List<ExampleValue>();
            int rowIndex = 0; // Single row per invocation

            foreach (var param in parameters)
            {
                var paramName = param.Name!;
                object? value = ResolveParameterValue(testObject, type, paramName, bindingFlags);
                values.Add(new ExampleValue(paramName, value, () => rowIndex));
            }

            return values.Count > 0 ? new Example([.. values]) : null;
        }

        private static object? ResolveParameterValue(object testObject, Type type, string paramName, BindingFlags flags)
        {
            // Try field (including backing fields from primary constructors)
            var field = FindField(type, paramName, flags);
            if (field is not null)
                return field.GetValue(testObject);

            // Try property
            var prop = FindProperty(type, paramName, flags);
            if (prop is not null)
                return prop.GetValue(testObject);

            return null;
        }

        private static FieldInfo? FindField(Type type, string name, BindingFlags flags)
        {
            // Exact match
            var field = type.GetField(name, flags);
            if (field is not null) return field;

            // Convention: _fieldName
            field = type.GetField("_" + name, flags);
            if (field is not null) return field;

            // Primary constructor parameter backing field: <name>P
            var fields = type.GetFields(flags);
            foreach (var f in fields)
            {
                if (f.Name.Equals($"<{name}>P", StringComparison.Ordinal) ||
                    f.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    return f;
            }

            return null;
        }

        private static PropertyInfo? FindProperty(Type type, string name, BindingFlags flags)
        {
            // Exact match (case-insensitive)
            var properties = type.GetProperties(flags);
            foreach (var p in properties)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return p;
            }
            return null;
        }
    }

    internal sealed class ParameterizedTestInfo(MethodInfo method, string stableScenarioId, Example example)
    {
        public MethodInfo Method { get; } = method;
        public string StableScenarioId { get; } = stableScenarioId;
        public Example Example { get; } = example;
    }
}
