//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace Microsoft.Windows.Controls.Primitives
{
    /// <summary>
    /// Represents the header for each row of the DataGrid
    /// </summary>
    [TemplatePart(Name = "PART_TopHeaderGripper", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_BottomHeaderGripper", Type = typeof(Thumb))]
    public class DataGridRowHeader : ButtonBase
    {
        static DataGridRowHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(typeof(DataGridRowHeader)));

            ContentProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(OnNotifyPropertyChanged, OnCoerceContent));
            ContentTemplateProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(OnNotifyPropertyChanged, OnCoerceContentTemplate));
            ContentTemplateSelectorProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(OnNotifyPropertyChanged, OnCoerceContentTemplateSelector));
            StyleProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(OnNotifyPropertyChanged, OnCoerceStyle));
            WidthProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(OnNotifyPropertyChanged, OnCoerceWidth));

            ClickModeProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(ClickMode.Press));
            FocusableProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(false));
        }

        #region Automation

        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return new Microsoft.Windows.Automation.Peers.DataGridRowHeaderAutomationPeer(this);
        }

        #endregion

        #region Layout

        /// <summary>
        ///     Property that indicates the brush to use when drawing seperators between headers.
        /// </summary>
        public Brush SeparatorBrush
        {
            get { return (Brush)GetValue(SeparatorBrushProperty); }
            set { SetValue(SeparatorBrushProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for SeperatorBrush.
        /// </summary>
        public static readonly DependencyProperty SeparatorBrushProperty =
            DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(null));

        /// <summary>
        ///     Property that indicates the Visibility for the header seperators.
        /// </summary>
        public Visibility SeparatorVisibility
        {
            get { return (Visibility)GetValue(SeparatorVisibilityProperty); }
            set { SetValue(SeparatorVisibilityProperty, value); }
        }

        /// <summary>
        ///     DependencyProperty for SeperatorBrush.
        /// </summary>
        public static readonly DependencyProperty SeparatorVisibilityProperty =
            DependencyProperty.Register("SeparatorVisibility", typeof(Visibility), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Measure this element and it's child elements.
        /// </summary>
        /// <remarks>
        /// DataGridRowHeader needs to update the DataGrid's RowHeaderActualWidth & use this as it's width so that they all end up the
        /// same size.
        /// </remarks>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var baseSize = base.MeasureOverride(availableSize);

            DataGrid dataGridOwner = DataGridOwner;
            if (dataGridOwner == null)
            {
                return baseSize;
            }

            if (DoubleUtil.IsNaN(dataGridOwner.RowHeaderWidth) &&
                baseSize.Width > dataGridOwner.RowHeaderActualWidth)
            {
                dataGridOwner.RowHeaderActualWidth = baseSize.Width;
            }

            // Regardless of how width the Header wants to be, we use 
            // DataGridOwner.RowHeaderActualWidth to ensure they're all the same size.
            return new Size(dataGridOwner.RowHeaderActualWidth, baseSize.Height);
        }

        #endregion

        #region Row Communication

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Give the Row a pointer to the RowHeader so that it can propagate down change notifications
            DataGridRow parent = ParentRow;

            if (parent != null)
            {
                parent.RowHeader = this;
                SyncProperties();
            }

            // Grippers will now be in the Visual tree.
            HookupGripperEvents();
        }

        /// <summary>
        ///     Update all properties that get a value from the DataGrid
        /// </summary>
        /// <remarks>
        ///     See comment on DataGridRow.OnDataGridChanged
        /// </remarks>
        internal void SyncProperties()
        {
            DataGridHelper.TransferProperty(this, ContentProperty);
            DataGridHelper.TransferProperty(this, StyleProperty);
            DataGridHelper.TransferProperty(this, ContentTemplateProperty);
            DataGridHelper.TransferProperty(this, ContentTemplateSelectorProperty);
            DataGridHelper.TransferProperty(this, WidthProperty);
            CoerceValue(IsRowSelectedProperty);

            // We could be the first row now, so reset the thumb visibility.
            OnCanUserResizeRowsChanged();
        }

        #endregion

        #region Property Change Notification

        /// <summary>
        ///     Notifies parts that respond to changes in the properties.
        /// </summary>
        private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridRowHeader)d).NotifyPropertyChanged(d, e);
        }

        /// <summary>
        ///     Notification for column header-related DependencyProperty changes from the grid or from columns.
        /// </summary>
        internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataGridRow.HeaderProperty || e.Property == ContentProperty)
            {
                DataGridHelper.TransferProperty(this, ContentProperty);
            }
            else if (e.Property == DataGrid.RowHeaderStyleProperty || e.Property == DataGridRow.HeaderStyleProperty || e.Property == StyleProperty)
            {
                DataGridHelper.TransferProperty(this, StyleProperty);
            }
            else if (e.Property == DataGrid.RowHeaderTemplateProperty || e.Property == DataGridRow.HeaderTemplateProperty || e.Property == ContentTemplateProperty)
            {
                DataGridHelper.TransferProperty(this, ContentTemplateProperty);
            }
            else if (e.Property == DataGrid.RowHeaderTemplateSelectorProperty || e.Property == DataGridRow.HeaderTemplateSelectorProperty || e.Property == ContentTemplateSelectorProperty)
            {
                DataGridHelper.TransferProperty(this, ContentTemplateSelectorProperty);
            }
            else if (e.Property == DataGrid.RowHeaderWidthProperty || e.Property == WidthProperty)
            {
                DataGridHelper.TransferProperty(this, WidthProperty);
            }
            else if (e.Property == DataGridRow.IsSelectedProperty)
            {
                CoerceValue(IsRowSelectedProperty);
            }
            else if (e.Property == DataGrid.CanUserResizeRowsProperty)
            {
                OnCanUserResizeRowsChanged();
            }
            else if (e.Property == DataGrid.RowHeaderActualWidthProperty)
            {
                // When the RowHeaderActualWidth changes we need to re-measure to pick up the new value for DesiredSize
                this.InvalidateMeasure();
                this.InvalidateArrange();
                
                // If the DataGrid has not run layout the headers parent may not position the cells correctly when the header size changes.
                // This will cause the cells to be out of sync with the columns. To avoid this we will force a layout of the headers parent panel.
                var parent = this.Parent as UIElement;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        #endregion 

        #region Property Coercion callbacks

        /// <summary>
        ///     Coerces the Content property.  We're choosing a value between Row.Header and the Content property on RowHeader.
        /// </summary>
        private static object OnCoerceContent(DependencyObject d, object baseValue)
        {
            var header = d as DataGridRowHeader;
            return DataGridHelper.GetCoercedTransferPropertyValue(
                header, 
                baseValue, 
                ContentProperty,
                header.ParentRow, 
                DataGridRow.HeaderProperty);
        }

        /// <summary>
        ///     Coerces the ContentTemplate property.
        /// </summary>
        private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
        {
            var header = d as DataGridRowHeader;
            var row = header.ParentRow;
            var dataGrid = row != null ? row.DataGridOwner : null;
            return DataGridHelper.GetCoercedTransferPropertyValue(
                header, 
                baseValue, 
                ContentTemplateProperty,
                row, 
                DataGridRow.HeaderTemplateProperty,
                dataGrid, 
                DataGrid.RowHeaderTemplateProperty);
        }

        /// <summary>
        ///     Coerces the ContentTemplateSelector property.
        /// </summary>
        private static object OnCoerceContentTemplateSelector(DependencyObject d, object baseValue)
        {
            var header = d as DataGridRowHeader;
            var row = header.ParentRow;
            var dataGrid = row != null ? row.DataGridOwner : null;
            return DataGridHelper.GetCoercedTransferPropertyValue(
                header, 
                baseValue, 
                ContentTemplateSelectorProperty,                
                row, 
                DataGridRow.HeaderTemplateSelectorProperty,
                dataGrid, 
                DataGrid.RowHeaderTemplateSelectorProperty);
        }

        /// <summary>
        ///     Coerces the Style property.
        /// </summary>
        private static object OnCoerceStyle(DependencyObject d, object baseValue)
        {
            var header = d as DataGridRowHeader;
            return DataGridHelper.GetCoercedTransferPropertyValue(
                header, 
                baseValue, 
                StyleProperty,
                header.ParentRow, 
                DataGridRow.HeaderStyleProperty,
                header.DataGridOwner, 
                DataGrid.RowHeaderStyleProperty);
        }

        /// <summary>
        ///     Coerces the Width property.
        /// </summary>
        private static object OnCoerceWidth(DependencyObject d, object baseValue)
        {
            var header = d as DataGridRowHeader;
            return DataGridHelper.GetCoercedTransferPropertyValue(
                header, 
                baseValue, 
                WidthProperty,
                header.DataGridOwner, 
                DataGrid.RowHeaderWidthProperty);
        }

        #endregion

        #region Selection

        /// <summary>
        ///     Indicates whether the owning DataGridRow is selected.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public bool IsRowSelected
        {
            get { return (bool)GetValue(IsRowSelectedProperty); }
        }

        private static readonly DependencyPropertyKey IsRowSelectedPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "IsRowSelected", 
                typeof(bool), 
                typeof(DataGridRowHeader),
                new FrameworkPropertyMetadata(false, null, new CoerceValueCallback(OnCoerceIsRowSelected)));

        /// <summary>
        ///     The DependencyProperty for the IsRowSelected property.
        /// </summary>
        public static readonly DependencyProperty IsRowSelectedProperty = IsRowSelectedPropertyKey.DependencyProperty;

        private static object OnCoerceIsRowSelected(DependencyObject d, object baseValue)
        {
            DataGridRowHeader header = (DataGridRowHeader)d;
            DataGridRow parent = header.ParentRow;
            if (parent != null)
            {
                return parent.IsSelected;
            }

            return baseValue;
        }

        /// <summary>
        ///     Called when the header is clicked.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            // The base implementation took capture. This prevents us from doing
            // drag selection, so release it.
            if (Mouse.Captured == this)
            {
                ReleaseMouseCapture();
            }

            DataGrid dataGridOwner = DataGridOwner;
            DataGridRow parentRow = ParentRow;
            if ((dataGridOwner != null) && (parentRow != null))
            {
                dataGridOwner.HandleSelectionForRowHeaderAndDetailsInput(parentRow, /* startDragging = */ true);
            }
        }

        #endregion

        #region Row Resizing

        /// <summary>
        /// Find grippers and register drag events
        ///
        /// The default style for DataGridRowHeader is
        /// +-------------------------------+
        /// +-------------------------------+ 
        /// +           Gripper             + 
        /// +-------------------------------+
        /// +            Header             +
        /// +-------------------------------+ 
        /// +           Gripper             + 
        /// +-------------------------------+
        /// +-------------------------------+
        /// 
        /// The reason we have two grippers is we can't extend the bottom gripper to straddle the line between two 
        /// headers; the header below would render on top of it.
        /// We resize a Row by grabbing the gripper to the bottom; the top gripper thus adjusts the height of
        /// the row above it.
        /// </summary>
        private void HookupGripperEvents()
        {
            UnhookGripperEvents();

            _topGripper = GetTemplateChild(TopHeaderGripperTemplateName) as Thumb;
            _bottomGripper = GetTemplateChild(BottomHeaderGripperTemplateName) as Thumb;

            if (_topGripper != null)
            {
                _topGripper.DragStarted += new DragStartedEventHandler(OnRowHeaderGripperDragStarted);
                _topGripper.DragDelta += new DragDeltaEventHandler(OnRowHeaderResize);
                _topGripper.DragCompleted += new DragCompletedEventHandler(OnRowHeaderGripperDragCompleted);
                _topGripper.MouseDoubleClick += new MouseButtonEventHandler(OnGripperDoubleClicked);
                SetTopGripperVisibility();
            }

            if (_bottomGripper != null)
            {
                _bottomGripper.DragStarted += new DragStartedEventHandler(OnRowHeaderGripperDragStarted);
                _bottomGripper.DragDelta += new DragDeltaEventHandler(OnRowHeaderResize);
                _bottomGripper.DragCompleted += new DragCompletedEventHandler(OnRowHeaderGripperDragCompleted);
                _bottomGripper.MouseDoubleClick += new MouseButtonEventHandler(OnGripperDoubleClicked);
                SetBottomGripperVisibility();
            }
        }

        /// <summary>
        /// Clear gripper event
        /// </summary>
        private void UnhookGripperEvents()
        {
            if (_topGripper != null)
            {
                _topGripper.DragStarted -= new DragStartedEventHandler(OnRowHeaderGripperDragStarted);
                _topGripper.DragDelta -= new DragDeltaEventHandler(OnRowHeaderResize);
                _topGripper.DragCompleted -= new DragCompletedEventHandler(OnRowHeaderGripperDragCompleted);
                _topGripper.MouseDoubleClick -= new MouseButtonEventHandler(OnGripperDoubleClicked);
                _topGripper = null;
            }

            if (_bottomGripper != null)
            {
                _bottomGripper.DragStarted -= new DragStartedEventHandler(OnRowHeaderGripperDragStarted);
                _bottomGripper.DragDelta -= new DragDeltaEventHandler(OnRowHeaderResize);
                _bottomGripper.DragCompleted -= new DragCompletedEventHandler(OnRowHeaderGripperDragCompleted);
                _bottomGripper.MouseDoubleClick -= new MouseButtonEventHandler(OnGripperDoubleClicked);
                _bottomGripper = null;
            }
        }

        private void SetTopGripperVisibility()
        {
            if (_topGripper != null)
            {
                DataGrid dataGrid = DataGridOwner;
                DataGridRow parent = ParentRow;
                if (dataGrid != null && parent != null && 
                    dataGrid.CanUserResizeRows && dataGrid.Items.Count > 1 &&
                    !object.ReferenceEquals(parent.Item, dataGrid.Items[0]))
                {
                    _topGripper.Visibility = Visibility.Visible;
                }
                else
                {
                    _topGripper.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SetBottomGripperVisibility()
        {
            if (_bottomGripper != null)
            {
                DataGrid dataGrid = DataGridOwner;
                if (dataGrid != null && dataGrid.CanUserResizeRows)
                {
                    _bottomGripper.Visibility = Visibility.Visible;
                }
                else
                {
                    _bottomGripper.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        ///     This is the row that the top gripper should be resizing.
        /// </summary>
        private DataGridRow PreviousRow
        {
            get
            {
                DataGridRow row = ParentRow;
                if (row != null)
                {
                    DataGrid dataGrid = row.DataGridOwner;
                    if (dataGrid != null)
                    {
                        int index = dataGrid.ItemContainerGenerator.IndexFromContainer(row);
                        if (index > 0)
                        {
                            return (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index - 1);
                        }
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Returns either this header or the one before it depending on which Gripper fired the event.
        /// </summary>
        /// <param name="sender"></param>
        private DataGridRow RowToResize(object gripper)
        {
            return (gripper == _bottomGripper) ? this.ParentRow : PreviousRow;
        }

        // Save the original height before resize
        private void OnRowHeaderGripperDragStarted(object sender, DragStartedEventArgs e)
        {
            DataGridRow rowToResize = RowToResize(sender);
            if (rowToResize != null)
            {
                rowToResize.OnRowResizeStarted();
                e.Handled = true;
            }
        }

        private void OnRowHeaderResize(object sender, DragDeltaEventArgs e)
        {
            DataGridRow rowToResize = RowToResize(sender);
            if (rowToResize != null)
            {
                rowToResize.OnRowResize(e.VerticalChange);
                e.Handled = true;
            }
        }

        // Restores original height if canceled.
        private void OnRowHeaderGripperDragCompleted(object sender, DragCompletedEventArgs e)
        {
            DataGridRow rowToResize = RowToResize(sender);
            if (rowToResize != null)
            {
                rowToResize.OnRowResizeCompleted(e.Canceled);
                e.Handled = true;
            }
        }

        private void OnGripperDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DataGridRow rowToResize = RowToResize(sender);
            if (rowToResize != null)
            {
                rowToResize.OnRowResizeReset();
                e.Handled = true;
            }
        }

        private void OnCanUserResizeRowsChanged()
        {
            SetTopGripperVisibility();
            SetBottomGripperVisibility();
        }

        #endregion

        #region Helpers

        internal DataGridRow ParentRow
        {
            get
            {
                return DataGridHelper.FindParent<DataGridRow>(this);
            }
        }

        private DataGrid DataGridOwner
        {
            get
            {
                DataGridRow parent = ParentRow;
                if (parent != null)
                {
                    return parent.DataGridOwner;
                }

                return null;
            }
        }

        #endregion

        private Thumb _topGripper, _bottomGripper;
        private const string TopHeaderGripperTemplateName = "PART_TopHeaderGripper";
        private const string BottomHeaderGripperTemplateName = "PART_BottomHeaderGripper";
    }
}