// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Data;
using System.Globalization;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Sample application for Accordion.
    /// </summary>
#if SILVERLIGHT
    [Sample("Accordion Playaround sample", DifficultyLevel.Basic)]
#endif
    [Category("Accordion")]
    public partial class AccordionSample : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccordionSample"/> class.
        /// </summary>
        public AccordionSample()
        {
            InitializeComponent();

            icSelectedIndices.SetBinding(
                ItemsControl.ItemsSourceProperty,
                new Binding("SelectedIndices") { Source = acc, Mode = BindingMode.OneWay });

            tbSelectedIndex.SetBinding(
                TextBox.TextProperty,
                new Binding("SelectedIndex") { Source = acc, Mode = BindingMode.TwoWay });

            acc.SetBinding(
                Accordion.SelectedIndexProperty,
                new Binding("Index") { Source = this, Mode = BindingMode.TwoWay });

            cbSelectionMode.SelectedItem = cbSelectionMode.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.Equals(acc.SelectionMode.ToString()));

            cbExpandDirection.SelectedItem = cbExpandDirection.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.Equals(acc.ExpandDirection.ToString()));

            cbSelectionSequence.SelectedItem = cbSelectionSequence.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.Equals(acc.SelectionSequence.ToString()));
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                tbSelectedIndex.Text = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Backingfield for Index.
        /// </summary>
        private int index;

        /// <summary>
        /// Expands the direction changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Hooked up in Xaml.")]
        private void ExpandDirectionChanged(object sender, SelectionChangedEventArgs e)
        {
            acc.ExpandDirection = (ExpandDirection)Enum.Parse(
                typeof(ExpandDirection),
                ((ComboBoxItem)cbExpandDirection.SelectedItem).Content.ToString(),
                true);
        }

        /// <summary>
        /// Sets the height.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Hooked up in Xaml.")]
        private void SetHeight(object sender, System.Windows.RoutedEventArgs e)
        {
            acc.Height = 500;
        }

        /// <summary>
        /// Removes the height.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Hooked up in Xaml.")]
        private void RemoveHeight(object sender, System.Windows.RoutedEventArgs e)
        {
            acc.ClearValue(Control.HeightProperty);
        }

        /// <summary>
        /// Selections the mode changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Hooked up in Xaml.")]
        private void SelectionModeChanged(object sender, SelectionChangedEventArgs e)
        {
            acc.SelectionMode = (AccordionSelectionMode)Enum.Parse(
                typeof(AccordionSelectionMode),
                ((ComboBoxItem)cbSelectionMode.SelectedItem).Content.ToString(),
                true);
        }

        /// <summary>
        /// Selects all.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Hooked up in Xaml.")]
        private void SelectAll(object sender, RoutedEventArgs e)
        {
            acc.SelectAll();
        }

        /// <summary>
        /// Unselects all.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Hooked up in Xaml.")]
        private void UnselectAll(object sender, RoutedEventArgs e)
        {
            acc.UnselectAll();
        }

        /// <summary>
        /// React to selectionSequence event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Hooked up in Xaml.")]
        private void SelectionSequenceChanged(object sender, SelectionChangedEventArgs e)
        {
            acc.SelectionSequence = (SelectionSequence)Enum.Parse(
                typeof(SelectionSequence),
                ((ComboBoxItem)cbSelectionSequence.SelectedItem).Content.ToString(),
                true);
        }

        /// <summary>
        /// Removes the width.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Xaml.")]
        private void RemoveWidth(object sender, RoutedEventArgs e)
        {
            acc.ClearValue(Control.WidthProperty);
        }

        /// <summary>
        /// Sets the width.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called by Xaml.")]
        private void SetWidth(object sender, RoutedEventArgs e)
        {
            acc.Width = 300;
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            button.Height += 50;
        }
    }
}
