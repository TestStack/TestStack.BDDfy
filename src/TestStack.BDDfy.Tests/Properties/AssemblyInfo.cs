using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("TestStack.BDDfy.Tests")]
[assembly: AssemblyTrademark("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1ceb33e8-3aac-4d02-a15b-5b75b5f662ad")]

// BDDfy tests overwrite static configuration for test, cannot use parallel testing
[assembly: CollectionBehavior(DisableTestParallelization = true)]