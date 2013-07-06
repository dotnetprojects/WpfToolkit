// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Windows.Automation.Peers", Justification = "Official automation peers namespace.")]

// UpDownBase does not exist on WPF
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.UpDownBase", Justification = "UpDownBase does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.UpDownBase`1", Justification = "UpDownBase does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.UpDownParseErrorEventArgs", Justification = "UpDownBase does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Automation.Peers.UpDownBaseAutomationPeer`1", Justification = "UpDownBase does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.UpDownParsingEventArgs`1")]

// Spinner does not exist on WPF
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.Spinner", Justification = "NumericUpDown does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.SpinDirection", Justification = "NumericUpDown does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.SpinEventArgs", Justification = "NumericUpDown does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.ButtonSpinner", Justification = "NumericUpDown does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.ValidSpinDirections", Justification = "ValidSpinDirections does not exist on WPF.")]

// NumericUpDown does not exist on WPF
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Automation.Peers.NumericUpDownAutomationPeer", Justification = "Spinner does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.NumericUpDown", Justification = "Spinner does not exist on WPF.")]

// time input controls do not exist in wpf
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.Picker")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.PopupButtonMode")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.TimeGlobalizationInfo")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.TimeParser")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.TimeParserCollection")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.TimePicker")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.TimePickerFormat")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.TimeUpDown")]

// DomainUpDown does not exist on WPF
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Automation.Peers.DomainUpDownAutomationPeer", Justification = "DomainUpDown does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.DomainUpDown", Justification = "DomainUpDown does not exist on WPF.")]
[assembly: SuppressMessage("Compatibility", "SWC4000:GeneralWPFCompatibilityRule", MessageId = "System.Windows.Controls.InvalidInputAction", Justification = "DomainUpDown does not exist on WPF.")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.InteractionHelper.#AllowKeyDown(System.Windows.Input.KeyEventArgs)", Justification = "Complete implementation used in other assemblies.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.InteractionHelper.#AllowKeyUp(System.Windows.Input.KeyEventArgs)", Justification = "Complete implementation used in other assemblies.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.InteractionHelper.#AllowMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs)", Justification = "Complete implementation used in other assemblies.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.InteractionHelper.#ClickCount", Justification = "Complete implementation used in other assemblies.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.InteractionHelper.#LastClickPosition", Justification = "Complete implementation used in other assemblies.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.InteractionHelper.#LastClickTime", Justification = "Complete implementation used in other assemblies.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.InteractionHelper.#OnMouseLeftButtonDownBase()", Justification = "Complete implementation used in other assemblies.")]

// TypeConverter linked in
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.TypeConverters.#CanConvertFrom`1(System.Type)", Justification = "Linked in file.")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "System.Windows.Controls.TypeConverters.#ConvertFrom`1(System.ComponentModel.TypeConverter,System.Object)", Justification = "Linked in file.")]

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "System.Windows.Controls.Primitives", Justification = "Expecting more types.")]
[assembly: SuppressMessage("Layout", "SWC3000:CanvasDoesNotRespectLayout", MessageId = "ControlTemplate<TargetType=inputToolkit:TimeUpDown>", Justification = "Canvas is used because of it does not interfere with layout considerations.")]

[assembly: SuppressMessage("General", "SWC1001:XmlDocumentationCommentShouldBeSpelledCorrectly", MessageId = "hh", Justification = "Indicates hours in a DateTimeFormat.")]
[assembly: SuppressMessage("General", "SWC1001:XmlDocumentationCommentShouldBeSpelledCorrectly", MessageId = "ss", Justification = "Indicates seconds in a DateTimeFormat.")]
[assembly: SuppressMessage("General", "SWC1001:XmlDocumentationCommentShouldBeSpelledCorrectly", MessageId = "Headered")]
[assembly: SuppressMessage("General", "SWC1001:XmlDocumentationCommentShouldBeSpelledCorrectly", MessageId = "Silverlight")]

