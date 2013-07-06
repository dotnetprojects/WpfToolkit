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
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.CustomEvents.#Value1")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.CustomEvents.#Value2")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.CustomEvents.#Value3")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "System.Windows.Controls.Samples.CustomEvents.#Value4")]

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// The PopulationEvents class shows how a developer might hook into the 
    /// population events to provide custom data.
    /// </summary>
#if SILVERLIGHT
    [Sample("(2)Custom Events", DifficultyLevel.Basic)]
#endif
    [Category("AutoCompleteBox")]
    public partial class CustomEvents : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the type.
        /// </summary>
        public CustomEvents()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Handle the Loaded event of the page.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NowAutoComplete.Populating += OnPopulatingSynchronous;
            NowAutoComplete2.Populating += OnPopulatingSynchronous;
            LaterAutoComplete.Populating += OnPopulatingAsynchronous;
            LaterAutoComplete2.Populating += OnPopulatingAsynchronous;
        }

        /// <summary>
        /// The Populating event handler.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnPopulatingSynchronous(object sender, PopulatingEventArgs e)
        {
            AutoCompleteBox source = (AutoCompleteBox)sender;

            source.ItemsSource = new string[]
            {
                e.Parameter + "1",
                e.Parameter + "2",
                e.Parameter + "3",
            };
        }

        /// <summary>
        /// The populating handler.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnPopulatingAsynchronous(object sender, PopulatingEventArgs e)
        {
            AutoCompleteBox source = (AutoCompleteBox)sender;

            // Cancel the populating value: this will allow us to call 
            // PopulateComplete as necessary.
            e.Cancel = true;
            
            // Use the dispatcher to simulate an asynchronous callback when 
            // data becomes available
            Dispatcher.BeginInvoke(
                new Action(delegate()
                {
                    source.ItemsSource = new string[]
                    {
                        e.Parameter + "1",
                        e.Parameter + "2",
                        e.Parameter + "3",
                    };

                    // Population is complete
                    source.PopulateComplete();
                }));
        }

        /// <summary>
        /// Called when an AutoCompleteBox's selected value changes. Uses the 
        /// Tag property to identify the content presenter to be updated.
        /// </summary>
        /// <param name="sender">The source AutoCompleteBox control.</param>
        /// <param name="e">The event data.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Event handler is wired up in XAML.")]
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