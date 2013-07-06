// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Media;

[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteComboBox.#ComboToggleButton", Justification = "Artifact of using a name inside the custom control template.")]

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// The AutoCompleteComboBox sample page shows the use of custom data
    /// objects, data templates, and a completely custom control template that 
    /// acts and looks much like a ComboBox with AutoCompleteBox capabilities.
    /// </summary>
#if SILVERLIGHT
    [Sample("(3)Styling", DifficultyLevel.Intermediate)]
#endif
    [Category("AutoCompleteBox")]
    public partial class AutoCompleteComboBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the AutoCompleteComboBox class.
        /// </summary>
        public AutoCompleteComboBox()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Handle the Loaded event of the page.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="rea">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs rea)
        {
            ControlApi.ItemFilter = (prefix, item) =>
            {
                if (string.IsNullOrEmpty(prefix))
                { 
                    return true; 
                }
                MemberInfoData pme = item as MemberInfoData;
                if (pme == null)
                {
                    return false; 
                }
                return (pme.Name.ToUpper(CultureInfo.InvariantCulture).Contains(prefix.ToUpper(CultureInfo.InvariantCulture)));
            };

            // Reflect and load data
            ControlApi.ItemsSource = MemberInfoData.GetSetForType(typeof(AutoCompleteBox));
            ControlPicker.ItemsSource = InitializeTypeList();

            // Set the changed handlers
            ControlApi.SelectionChanged += OnApiChanged;
            ControlPicker.SelectionChanged += OnPickerChanged;
            
            // Setup the dictionary converter
            ControlPicker.ValueMemberBinding = new Binding
            {
                Converter = new DictionaryKeyValueConverter<string, Type>()
            };
        }

        /// <summary>
        /// Update the API system.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The selection changed event data.</param>
        private void OnPickerChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ControlPicker.SelectedItem == null)
            {
                ControlApi.ItemsSource = null;
                ControlApi.Text = null;
                IntelliSenseIcon.Content = null;
                ControlApi.IsEnabled = false;
            }
            else
            {
                KeyValuePair<string, Type> pair = (KeyValuePair<string, Type>)ControlPicker.SelectedItem;
                ControlApi.ItemsSource = MemberInfoData.GetSetForType(pair.Value);
                ControlApi.IsEnabled = true;
            }
        }

        /// <summary>
        /// Update the visible content.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The selection changed event data.</param>
        private void OnApiChanged(object sender, SelectionChangedEventArgs e)
        {
            MemberInfoData mim = ControlApi.SelectedItem as MemberInfoData;
            IntelliSenseIcon.Content = ControlApi.SelectedItem == null ? null : mim.Icon;
            SelectedInformation.Text = mim == null ? string.Empty : mim.MemberInfo.Name;
        }

        /// <summary>
        /// Initializes the type list.
        /// </summary>
        /// <returns>Returns a dictionary of string to Type values.</returns>
        private static Dictionary<string, Type> InitializeTypeList()
        {
            Dictionary<string, Type> typeList = new Dictionary<string, Type>();
            Assembly[] assemblies = 
            { 
                typeof(Button).Assembly,
                typeof(TreeView).Assembly,
                typeof(AutoCompleteBox).Assembly,
                typeof(Chart).Assembly,
            };
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.IsSubclassOf(typeof(Control))&& !typeList.ContainsKey(type.FullName))
                    {
                        typeList.Add(type.FullName, type);
                    }
                }
            }
            return typeList;
        }

        /// <summary>
        /// Toggle the drop down for the control, part of the custom template.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Wired up in markup.")]
        private void DropDownToggle_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            AutoCompleteBox acb = null;
            while (fe != null && acb == null)
            {
                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
                acb = fe as AutoCompleteBox;
            }
            if (acb != null)
            {
                if (string.IsNullOrEmpty(acb.SearchText))
                {
                    acb.Text = string.Empty;
                }
                acb.IsDropDownOpen = !acb.IsDropDownOpen;
            }
        }
    }
}