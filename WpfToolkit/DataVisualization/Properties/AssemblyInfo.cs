// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

// WPF-specific settings
[assembly: AssemblyDescription("WPF Toolkit Data Visualization Controls")]
[assembly: AssemblyProduct("WPF Toolkit")]
[assembly: AssemblyFileVersion("3.5.50211.1")]
[assembly: AssemblyVersion("3.5.40128.1")]

// WPF-only settings
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly)]

// WPF Toolkit settings
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityCritical]
