// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Markup;

// Silverlight/WPF shared settings
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyTitle("System.Windows.Controls.DataVisualization.Toolkit")]
[assembly: AssemblyTrademark("")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: XmlnsDefinition("clr-namespace:System.Windows.Controls.DataVisualization;assembly=DotNetProjects.DataVisualization.Toolkit", "System.Windows.Controls.DataVisualization")]
[assembly: XmlnsDefinition("clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=DotNetProjects.DataVisualization.Toolkit", "System.Windows.Controls.DataVisualization.Charting")]
[assembly: XmlnsPrefix("clr-namespace:System.Windows.Controls.DataVisualization;assembly=DotNetProjects.DataVisualization.Toolkit", "visualizationToolkit")]
[assembly: XmlnsPrefix("clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=DotNetProjects.DataVisualization.Toolkit", "chartingToolkit")]
#if !NO_XMLNSDEFINITION_URIS
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit", "toolkit")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit", "System.Windows.Controls.DataVisualization")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit", "System.Windows.Controls.DataVisualization.Charting")]
#endif
