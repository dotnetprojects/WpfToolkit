//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// This file specifies various assembly level attributes.
//
//---------------------------------------------------------------------------

using System;
using System.Resources;
using System.Security;
using System.Windows;
using System.Windows.Markup;

// Needed to turn on checking of security critical call chains
[assembly:SecurityCritical]

// Needed to enable xbap scenarios
[assembly:AllowPartiallyTrustedCallers]

[assembly:CLSCompliant(true)]
[assembly:NeutralResourcesLanguage("en-US")]

[assembly:ThemeInfo(
    // Specifies the location of theme specific resources
    ResourceDictionaryLocation.SourceAssembly,
    // Specifies the location of non-theme specific resources:
    ResourceDictionaryLocation.SourceAssembly)]

[assembly:XmlnsDefinition("http://schemas.microsoft.com/wpf/2008/toolkit", "Microsoft.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/wpf/2008/toolkit", "Microsoft.Windows.Controls.Primitives")]

// This line adds the public classes in this assembly and the System.Windows namespace to 
// the default WPF namespace.  This makes it XAML compatible with Silverlight where VisualStateManager
// is part of the default namespace.
[assembly:XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows")]

