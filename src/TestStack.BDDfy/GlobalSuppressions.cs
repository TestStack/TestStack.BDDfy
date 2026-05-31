// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "We want most of the classes to sit in root namespace", Scope = "namespace", Target = "~N:TestStack.BDDfy")]
[assembly: SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Scope = "type", Target = "~T:TestStack.BDDfy.Annotations.ImplicitUseTargetFlags")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Scope = "namespace", Target = "~N:TestStack.BDDfy.Annotations")]
