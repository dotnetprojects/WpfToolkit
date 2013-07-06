// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

// Global suppressions for this sample
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value1")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value2")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value3")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value4")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value5")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value6")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value7")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value8")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.AutoCompleteBoxSample.#Value9")]

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// The AutoCompleteGettingStarted sample page shows several common uses 
    /// of the AutoCompleteBox control.
    /// </summary>
#if SILVERLIGHT
    [Sample("(0)AutoCompleteBox", DifficultyLevel.Basic)]
#endif
    [Category("AutoCompleteBox")]
    public partial class AutoCompleteBoxSample : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the AutoCompleteGettingStarted class.
        /// </summary>
        public AutoCompleteBoxSample()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Hook up to the Loaded event.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Words
            WordComplete.ItemsSource = Words.GetAliceInWonderland();

            // Sliders
            SetDelay.ValueChanged += (s, args) => DynamicDelayAutoCompleteBox.MinimumPopulateDelay = (int)Math.Floor(SetDelay.Value);
            SetPrefixLength.ValueChanged += (s, args) => WordComplete.MinimumPrefixLength = (int)Math.Floor(SetPrefixLength.Value);
        }

        /// <summary>
        /// Called when an AutoCompleteBox's selected value changes. Uses the 
        /// Tag property to identify the content presenter to be updated.
        /// </summary>
        /// <param name="sender">The source AutoCompleteBox control.</param>
        /// <param name="e">The event data.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Event is wired up in XAML.")]
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteBox acb = (AutoCompleteBox)sender;

            // In these sample scenarios, the Tag is the name of the content 
            // presenter to use to display the value.
            string contentPresenterName = (string)acb.Tag;
            ContentPresenter cp = FindName(contentPresenterName) as ContentPresenter;
            if (cp != null)
            {
                cp.Content = acb.SelectedItem;
            }
        }
    }
}