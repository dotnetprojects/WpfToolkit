// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Automation.Peers;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// An implementation of ISelectionAdapter for the DataGrid control. This 
    /// adapter, unlike the standard SelectorSelectionAdapter, actually derives 
    /// directly from DataGrid.
    /// </summary>
    public class DataGridSelectionAdapter : DataGrid, ISelectionAdapter
    {
        /// <summary>
        /// Gets or sets a value indicating whether the selection should be 
        /// ignored. Since the DataGrid automatically selects the first row 
        /// whenever the data changes, this simple implementation only works 
        /// with key navigation and mouse clicks. This prevents the text box 
        /// of the AutoCompleteBox control from being updated continuously.
        /// </summary>
        private bool IgnoreAnySelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the selection change event 
        /// should not be fired.
        /// </summary>
        private bool IgnoringSelectionChanged { get; set; }

        /// <summary>
        /// Occurs when the currently selected item changes.
        /// </summary>
        public new event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// An event that indicates that a selection is complete and has been 
        /// made, effectively a commit action.
        /// </summary>
        public event RoutedEventHandler Commit;

        /// <summary>
        /// An event that indicates that the selection operation has been 
        /// canceled.
        /// </summary>
        public event RoutedEventHandler Cancel;

        /// <summary>
        /// Initializes a new instance of the SelectorSelectionAdapter class.
        /// </summary>
        public DataGridSelectionAdapter()
        {
            base.SelectionChanged += OnSelectionChanged;
            MouseLeftButtonUp += OnSelectorMouseLeftButtonUp;
        }

        /// <summary>
        /// Gets or sets the selected item through the adapter.
        /// </summary>
        public new object SelectedItem
        {
            get
            {
                return base.SelectedItem;
            }

            set
            {
                IgnoringSelectionChanged = true;
                base.SelectedItem = value;
                IgnoringSelectionChanged = false;
            }
        }

        /// <summary>
        /// Handles the mouse left button up event on the selector control.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnSelectorMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IgnoreAnySelection = false;

            OnSelectionChanged(this, null);
            OnCommit(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Handles the SelectionChanged event on the Selector control.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The selection changed event data.</param>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IgnoringSelectionChanged)
            {
                return;
            }

            if (IgnoreAnySelection)
            {
                return;
            }

            SelectionChangedEventHandler handler = this.SelectionChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        public new IEnumerable ItemsSource
        {
            get { return base.ItemsSource; }
            
            set
            {
                if (base.ItemsSource != null)
                {
                    INotifyCollectionChanged notify = base.ItemsSource as INotifyCollectionChanged;
                    if (notify != null)
                    {
                        notify.CollectionChanged -= OnCollectionChanged;
                    }
                }

                base.ItemsSource = value;

                if (base.ItemsSource != null)
                {
                    INotifyCollectionChanged notify = base.ItemsSource as INotifyCollectionChanged;
                    if (notify != null)
                    {
                        notify.CollectionChanged += OnCollectionChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the CollectionChanged event, resetting the selection 
        /// ignore flag.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IgnoreAnySelection = true;
        }

        /// <summary>
        /// Gets the observable collection set by AutoCompleteBox.
        /// </summary>
        private new ObservableCollection<object> Items
        {
            get { return ItemsSource as ObservableCollection<object>; }
        }

        /// <summary>
        /// Increment the selected index, or wrap.
        /// </summary>
        private void SelectedIndexIncrement()
        {
            SelectedIndex = SelectedIndex + 1 >= Items.Count ? -1 : SelectedIndex + 1;
            ScrollIntoView(SelectedItem, this.Columns[0]);
        }

        /// <summary>
        /// Decrement the SelectedIndex, or wrap around, inside the nested 
        /// SelectionAdapter's control.
        /// </summary>
        private void SelectedIndexDecrement()
        {
            int index = SelectedIndex;
            if (index >= 0)
            {
                SelectedIndex--;
            }
            else if (index == -1)
            {
                SelectedIndex = Items.Count - 1;
            }

            ScrollIntoView(SelectedItem, this.Columns[0]);
        }

        /// <summary>
        /// Process a key down event.
        /// </summary>
        /// <param name="e">The key event arguments object.</param>
        public void HandleKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    OnCommit(this, e);
                    e.Handled = true;
                    break;

                case Key.Up:
                    IgnoreAnySelection = false; 
                    SelectedIndexDecrement();
                    e.Handled = true;
                    break;

                case Key.Down:
                    if ((ModifierKeys.Alt & Keyboard.Modifiers) == ModifierKeys.None)
                    {
                        IgnoreAnySelection = false;
                        SelectedIndexIncrement();
                        e.Handled = true;
                    }
                    break;

                case Key.Escape:
                    OnCancel(this, e);
                    e.Handled = true;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Fires the Commit event.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnCommit(object sender, RoutedEventArgs e)
        {
            RoutedEventHandler handler = Commit;
            if (handler != null)
            {
                handler(sender, e);
            }

            AfterAdapterAction();
        }

        /// <summary>
        /// Fires the Cancel event.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            RoutedEventHandler handler = Cancel;
            if (handler != null)
            {
                handler(sender, e);
            }

            AfterAdapterAction();
        }

        /// <summary>
        /// Change the selection after the actions are complete.
        /// </summary>
        private void AfterAdapterAction()
        {
            IgnoringSelectionChanged = true;
            SelectedItem = null;
            SelectedIndex = -1;
            IgnoringSelectionChanged = false;

            // Reset, to ignore any future changes
            IgnoreAnySelection = true;
        }

        /// <summary>
        /// Initializes a new instance of a DataGridAutomationPeer.
        /// </summary>
        /// <returns>Returns a new DataGridAutomationPeer.</returns>
        public AutomationPeer CreateAutomationPeer()
        {
            return new DataGridAutomationPeer(this);
        }
    }
}