// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace System.Windows.Controls.DataVisualization
{
    /// <summary>
    /// Represents a control that displays a list of items and has a title.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ContentPresenter))]
    [StyleTypedProperty(Property = "TitleStyle", StyleTargetType = typeof(Title))]
    public partial class Legend : HeaderedItemsControl
    {
#if !SILVERLIGHT
        /// <summary>
        /// Initializes the static members of the Legend class.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Dependency properties are initialized in-line.")]
        static Legend()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Legend), new FrameworkPropertyMetadata(typeof(Legend)));
        }

#endif
        /// <summary>
        /// Initializes a new instance of the Legend class.
        /// </summary>
        public Legend()
        {
#if SILVERLIGHT
            DefaultStyleKey = typeof(Legend);
            this.SetBinding(HeaderProperty, new Binding("Title") { Source = this, Mode = BindingMode.TwoWay });
#endif
        }

        #region public Style TitleStyle
        /// <summary>
        /// Gets or sets the Style of the ISeriesHost's Title.
        /// </summary>
        public Style TitleStyle
        {
            get { return GetValue(TitleStyleProperty) as Style; }
            set { SetValue(TitleStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the TitleStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register(
                "TitleStyle",
                typeof(Style),
                typeof(Legend),
                null);
        #endregion public Style TitleStyle

        #region public object Title
        /// <summary>
        /// Gets or sets the object of the Title.
        /// </summary>
        public object Title
        {
            get { return GetValue(TitleProperty) as object; }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Identifies the Title dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(object),
                typeof(Legend),
                new PropertyMetadata(new PropertyChangedCallback(OnTitleChanged)));

        /// <summary>
        /// Updates the legend visibility when the title changes.
        /// </summary>
        /// <param name="sender">The legend with a Title that changed.</param>
        /// <param name="args">Information about the event.</param>
        public static void OnTitleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            Legend legend = (Legend) sender;
#if !SILVERLIGHT
            // Push value through because Binding doesn't work like it does on Silverlight
            legend.Header = legend.Title;
#endif
            legend.UpdateLegendVisibility();
        }
        #endregion public object Title

        /// <summary>
        /// Handles the CollectionChanged event for ItemsSource.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            UpdateLegendVisibility();
            base.OnItemsChanged(e);
        }

        /// <summary>
        /// Updates the Legend's Visibility property according to whether it has anything to display.
        /// </summary>
        private void UpdateLegendVisibility()
        {
            Visibility = (this.Header != null || this.Items.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
