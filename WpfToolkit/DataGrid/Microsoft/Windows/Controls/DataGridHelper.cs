//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Windows.Controls.Primitives;
using MS.Internal;

namespace Microsoft.Windows.Controls
{
    /// <summary>
    ///     Helper code for DataGrid.
    /// </summary>
    internal static class DataGridHelper
    {
        #region GridLines

        // Common code for drawing GridLines.  Shared by DataGridDetailsPresenter, DataGridCellsPresenter, and Cell

        /// <summary>
        ///     Returns a size based on the given one with the given double subtracted out from the Width or Height.
        ///     Used to adjust for the thickness of grid lines.
        /// </summary>
        public static Size SubtractFromSize(Size size, double thickness, bool height)
        {
            if (height)
            {
                return new Size(size.Width, Math.Max(0.0, size.Height - thickness));
            }
            else
            {
                return new Size(Math.Max(0.0, size.Width - thickness), size.Height);
            }
        }

        /// <summary>
        ///     Test if either the vertical or horizontal gridlines are visible.
        /// </summary>
        public static bool IsGridLineVisible(DataGrid dataGrid, bool isHorizontal)
        {
            if (dataGrid != null)
            {
                DataGridGridLinesVisibility visibility = dataGrid.GridLinesVisibility;

                switch (visibility)
                {
                    case DataGridGridLinesVisibility.All:
                        return true;
                    case DataGridGridLinesVisibility.Horizontal:
                        return isHorizontal;
                    case DataGridGridLinesVisibility.None:
                        return false;
                    case DataGridGridLinesVisibility.Vertical:
                        return !isHorizontal;
                }
            }

            return false;
        }

        #endregion 

        #region Notification Propagation

        public static bool ShouldNotifyCells(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.Cells);
        }

        public static bool ShouldNotifyCellsPresenter(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.CellsPresenter);
        }

        public static bool ShouldNotifyColumns(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.Columns);
        }

        public static bool ShouldNotifyColumnHeaders(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.ColumnHeaders);
        }

        public static bool ShouldNotifyColumnHeadersPresenter(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.ColumnHeadersPresenter);
        }

        public static bool ShouldNotifyColumnCollection(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.ColumnCollection);
        }

        public static bool ShouldNotifyDataGrid(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.DataGrid);
        }

        public static bool ShouldNotifyDetailsPresenter(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.DetailsPresenter);
        }

        public static bool ShouldRefreshCellContent(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.RefreshCellContent);
        }

        public static bool ShouldNotifyRowHeaders(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.RowHeaders);
        }

        public static bool ShouldNotifyRows(NotificationTarget target)
        {
            return TestTarget(target, NotificationTarget.Rows);
        }

        public static bool ShouldNotifyRowSubtree(NotificationTarget target)
        {
            NotificationTarget value = 
                NotificationTarget.Rows | 
                NotificationTarget.RowHeaders | 
                NotificationTarget.CellsPresenter |
                NotificationTarget.Cells |
                NotificationTarget.RefreshCellContent |
                NotificationTarget.DetailsPresenter;

            return TestTarget(target, value);
        }

        private static bool TestTarget(NotificationTarget target, NotificationTarget value)
        {
            return (target & value) != 0; 
        }

        #endregion 

        #region Tree Helpers

        /// <summary>
        ///     Walks up the templated parent tree looking for a parent type.
        /// </summary>
        public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            FrameworkElement parent = element.TemplatedParent as FrameworkElement;

            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = parent.TemplatedParent as FrameworkElement;
            }

            return null;
        }

        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }

        /// <summary>
        ///     Helper method which determines if any of the elements of
        ///     the tree is focusable and has tab stop
        /// </summary>
        public static bool TreeHasFocusAndTabStop(DependencyObject element)
        {
            if (element == null)
            {
                return false;
            }

            UIElement uielement = element as UIElement;
            if (uielement != null)
            {
                if (uielement.Focusable && KeyboardNavigation.GetIsTabStop(uielement))
                {
                    return true;
                }
            }
            else
            {
                ContentElement contentElement = element as ContentElement;
                if (contentElement != null && contentElement.Focusable && KeyboardNavigation.GetIsTabStop(contentElement))
                {
                    return true;
                }
            }

            int childCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(element, i) as DependencyObject;
                if (TreeHasFocusAndTabStop(child))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Cells Panel Helper

        /// <summary>
        ///     Invalidates a cell's panel if its column's width changes sufficiently. 
        /// </summary>
        /// <param name="cell">The cell or header.</param>
        /// <param name="e"></param>
        public static void OnColumnWidthChanged(IProvideDataGridColumn cell, DependencyPropertyChangedEventArgs e)
        {
            Debug.Assert((cell is DataGridCell) || (cell is DataGridColumnHeader), "provideColumn should be one of the cell or header containers.");

            UIElement element = (UIElement)cell;
            DataGridColumn column = cell.Column;
            bool isColumnHeader = (cell is DataGridColumnHeader);

            if (column != null)
            {
                // determine the desired value of width for auto kind columns
                DataGridLength width = column.Width;
                if (width.IsAuto || 
                    (!isColumnHeader && width.IsSizeToCells) ||
                    (isColumnHeader && width.IsSizeToHeader))
                {
                    DataGridLength oldWidth = (DataGridLength)e.OldValue;
                    double desiredWidth = 0.0;
                    if (oldWidth.UnitType != width.UnitType)
                    {
                        double constraintWidth = column.GetConstraintWidth(isColumnHeader);
                        if (!DoubleUtil.AreClose(element.DesiredSize.Width, constraintWidth))
                        {
                            element.InvalidateMeasure();
                            element.Measure(new Size(constraintWidth, double.PositiveInfinity));
                        }

                        desiredWidth = element.DesiredSize.Width;
                    }
                    else
                    {
                        desiredWidth = oldWidth.DesiredValue;
                    }

                    if (DoubleUtil.IsNaN(width.DesiredValue) ||
                        DoubleUtil.LessThan(width.DesiredValue, desiredWidth))
                    {
                        column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, desiredWidth, width.DisplayValue));
                    }
                }
            }
        }

        /// <summary>
        ///     Helper method which returns the clip for the cell based on whether it overlaps with frozen columns or not
        /// </summary>
        /// <param name="cell">The cell or header.</param>
        /// <returns></returns>
        public static Geometry GetFrozenClipForCell(IProvideDataGridColumn cell)
        {
            DataGridCellsPanel panel = GetParentPanelForCell(cell);
            if (panel != null)
            {
                return panel.GetFrozenClipForChild((UIElement)cell);
            }

            return null;
        }

        /// <summary>
        ///     Helper method which returns the parent DataGridCellsPanel for a cell
        /// </summary>
        /// <param name="cell">The cell or header.</param>
        /// <returns>Parent panel of the given cell or header</returns>
        public static DataGridCellsPanel GetParentPanelForCell(IProvideDataGridColumn cell)
        {
            Debug.Assert((cell is DataGridCell) || (cell is DataGridColumnHeader), "provideColumn should be one of the cell or header containers.");

            UIElement element = (UIElement)cell;
            return VisualTreeHelper.GetParent(element) as DataGridCellsPanel;
        }

        /// <summary>
        ///     Helper method which returns the parent DataGridCellPanel's offset from the scroll viewer
        ///     for a cell or Header
        /// </summary>
        /// <param name="cell">The cell or header.</param>
        /// <returns>Parent Panel's offset with respect to scroll viewer</returns>
        public static double GetParentCellsPanelHorizontalOffset(IProvideDataGridColumn cell)
        {
            DataGridCellsPanel panel = GetParentPanelForCell(cell);
            if (panel != null)
            {
                return panel.ComputeCellsPanelHorizontalOffset();
            }

            return 0.0;
        }

        #endregion

        #region Property Helpers

        public static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
        {
            return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
        }

        public static object GetCoercedTransferPropertyValue(
            DependencyObject baseObject, 
            object baseValue, 
            DependencyProperty baseProperty,
            DependencyObject parentObject, 
            DependencyProperty parentProperty)
        {
            return GetCoercedTransferPropertyValue(
                baseObject, 
                baseValue, 
                baseProperty,
                parentObject, 
                parentProperty,
                null, 
                null);
        }

        /// <summary>
        ///     Computes the value of a given property based on the DataGrid property transfer rules.
        /// </summary>
        /// <remarks>
        ///     This is intended to be called from within the coercion of the baseProperty.
        /// </remarks>
        /// <param name="baseObject">The target object which recieves the transferred property</param>
        /// <param name="baseValue">The baseValue that was passed into the coercion delegate</param>
        /// <param name="baseProperty">The property that is being coerced</param>
        /// <param name="parentObject">The object that contains the parentProperty</param>
        /// <param name="parentProperty">A property who's value should be transfered (via coercion) to the baseObject if it has a higher precedence.</param>
        /// <param name="grandParentObject">Same as parentObject but evaluated at a lower presedece for a given BaseValueSource</param>
        /// <param name="grandParentProperty">Same as parentProperty but evaluated at a lower presedece for a given BaseValueSource</param>
        /// <returns></returns>
        public static object GetCoercedTransferPropertyValue(
            DependencyObject baseObject, 
            object baseValue, 
            DependencyProperty baseProperty,            
            DependencyObject parentObject,           
            DependencyProperty parentProperty,
            DependencyObject grandParentObject,      
            DependencyProperty grandParentProperty)
        {
            // Transfer Property Coercion rules:
            //
            // Determine if this is a 'Transfer Property Coercion'.  If so:
            //   We can safely get the BaseValueSource because the property change originated from another
            //   property, and thus this BaseValueSource wont be stale.
            //   Pick a value to use based on who has the greatest BaseValueSource
            // If not a 'Transfer Property Coercion', simply return baseValue.  This will cause a property change if the value changes, which
            // will trigger a 'Transfer Property Coercion', and we will no longer have a stale BaseValueSource
            var coercedValue = baseValue;

            if (IsPropertyTransferEnabled(baseObject, baseProperty))
            {
                var propertySource = DependencyPropertyHelper.GetValueSource(baseObject, baseProperty);
                var maxBaseValueSource = propertySource.BaseValueSource;

                if (parentObject != null)
                {
                    var parentPropertySource = DependencyPropertyHelper.GetValueSource(parentObject, parentProperty);

                    if (parentPropertySource.BaseValueSource > maxBaseValueSource)
                    {
                        coercedValue = parentObject.GetValue(parentProperty);
                        maxBaseValueSource = parentPropertySource.BaseValueSource;
                    }
                }

                if (grandParentObject != null)
                {
                    var grandParentPropertySource = DependencyPropertyHelper.GetValueSource(grandParentObject, grandParentProperty);

                    if (grandParentPropertySource.BaseValueSource > maxBaseValueSource)
                    {
                        coercedValue = grandParentObject.GetValue(grandParentProperty);
                        maxBaseValueSource = grandParentPropertySource.BaseValueSource;
                    }
                }
            }

            return coercedValue;
        }

        /// <summary>
        ///     Causes the given DependencyProperty to be coerced in transfer mode.
        /// </summary>
        /// <remarks>
        ///     This should be called from within the target object's NotifyPropertyChanged.  It MUST be called in
        ///     response to a change in the target property.
        /// </remarks>
        /// <param name="d">The DependencyObject which contains the property that needs to be transfered.</param>
        /// <param name="p">The DependencyProperty that is the target of the property transfer.</param>
        public static void TransferProperty(DependencyObject d, DependencyProperty p)
        {
            var transferEnabledMap = GetPropertyTransferEnabledMapForObject(d);
            transferEnabledMap[p] = true;
            d.CoerceValue(p);
            transferEnabledMap[p] = false;
        }

        private static Dictionary<DependencyProperty, bool> GetPropertyTransferEnabledMapForObject(DependencyObject d)
        {
            var propertyTransferEnabledForObject = _propertyTransferEnabledMap[d] as Dictionary<DependencyProperty, bool>;

            if (propertyTransferEnabledForObject == null)
            {
                propertyTransferEnabledForObject = new Dictionary<DependencyProperty, bool>();
                _propertyTransferEnabledMap.SetWeak(d, propertyTransferEnabledForObject);
            }

            return propertyTransferEnabledForObject;
        }

        internal static bool IsPropertyTransferEnabled(DependencyObject d, DependencyProperty p)
        {
            var propertyTransferEnabledForObject = _propertyTransferEnabledMap[d] as Dictionary<DependencyProperty, bool>;

            if (propertyTransferEnabledForObject != null)
            {
                bool isPropertyTransferEnabled;
                if (propertyTransferEnabledForObject.TryGetValue(p, out isPropertyTransferEnabled))
                {
                    return isPropertyTransferEnabled;
                }
            }

            return false;
        }
        
        /// <summary>
        ///     Tracks which properties are currently being transfered.  This information is needed when GetPropertyTransferEnabledMapForObject
        ///     is called inside of Coersion.
        /// </summary>
        private static WeakHashtable _propertyTransferEnabledMap = new WeakHashtable();

        #endregion

        #region Theme

        /// <summary>
        ///     Will return the string version of the current theme name.
        ///     Will apply a resource reference to the element passed in.
        /// </summary>
        public static string GetTheme(FrameworkElement element)
        {
            object o = element.ReadLocalValue(ThemeProperty);
            if (o == DependencyProperty.UnsetValue)
            {
                element.SetResourceReference(ThemeProperty, _themeKey);
            }

            return (string)element.GetValue(ThemeProperty);
        }

        /// <summary>
        ///     Private property used to determine the theme name.
        /// </summary>
        private static readonly DependencyProperty ThemeProperty =
            DependencyProperty.RegisterAttached("Theme", typeof(string), typeof(DataGridHelper), new FrameworkPropertyMetadata(String.Empty));

        /// <summary>
        ///     The resource key used to fetch the theme name.
        /// </summary>
        private static ComponentResourceKey _themeKey = new ComponentResourceKey(typeof(DataGrid), "Theme");

        /// <summary>
        ///     Sets up a property change handler for the private theme property.
        ///     Use this to receive a theme change notification.
        ///     Requires calling GetTheme on an element of the given type at some point.
        /// </summary>
        public static void HookThemeChange(Type type, PropertyChangedCallback propertyChangedCallback)
        {
            ThemeProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(String.Empty, propertyChangedCallback));
        }

        #endregion

        #region Binding

        /// <summary>
        ///     Returns true if the binding (or any part of it) is OneWay.
        /// </summary>
        internal static bool IsOneWay(BindingBase bindingBase)
        {
            if (bindingBase == null)
            {
                return false;
            }

            // If it is a standard Binding, then check if it's Mode is OneWay
            Binding binding = bindingBase as Binding;
            if (binding != null)
            {
                return binding.Mode == BindingMode.OneWay;
            }

            // A multi-binding can be OneWay as well
            MultiBinding multiBinding = bindingBase as MultiBinding;
            if (multiBinding != null)
            {
                return multiBinding.Mode == BindingMode.OneWay;
            }

            // A priority binding is a list of bindings, if any are OneWay, we'll call it OneWay
            PriorityBinding priBinding = bindingBase as PriorityBinding;
            if (priBinding != null)
            {
                Collection<BindingBase> subBindings = priBinding.Bindings;
                int count = subBindings.Count;
                for (int i = 0; i < count; i++)
                {
                    if (IsOneWay(subBindings[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Ensures that the given binding is two way if it's not already OneWay.
        ///     The default Mode is BindingMode.Default which will become effectively OneWay
        ///     or TwoWay based on the property being bound to.  We ensure it's TwoWay & Explicit
        ///     to make sure it shows up in the bindingGroup, however this causes problems for
        ///     read-only properties, so we dont touch them.
        /// </summary>
        internal static void EnsureTwoWayIfNotOneWay(BindingBase bindingBase)
        {
            if (bindingBase == null)
            {
                return;
            }

            // If it is a standard Binding, then set the mode to TwoWay
            Binding binding = bindingBase as Binding;
            if (binding != null)
            {
                if (binding.Mode != BindingMode.OneWay)
                {
                    if (binding.Mode != BindingMode.TwoWay)
                    {
                        binding.Mode = BindingMode.TwoWay;
                    }

                    // Be careful not to modify bindings that we've already used.  We have no way to know this exactly,
                    // because that information is private, but we can avoid changing something that we've already changed.
                    if (binding.UpdateSourceTrigger != UpdateSourceTrigger.Explicit)
                    {
                        binding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                    }
                }

                return;
            }

            // A multi-binding can be set to TwoWay as well
            MultiBinding multiBinding = bindingBase as MultiBinding;
            if (multiBinding != null)
            {
                if (multiBinding.Mode != BindingMode.OneWay)
                {
                    if (multiBinding.Mode != BindingMode.TwoWay)
                    {
                        multiBinding.Mode = BindingMode.TwoWay;
                    }

                    // Be careful not to modify bindings that we've already used.  We have no way to know this exactly,
                    // because that information is private, but we can avoid changing something that we've already changed.
                    if (multiBinding.UpdateSourceTrigger != UpdateSourceTrigger.Explicit)
                    {
                        multiBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                    }
                }

                return;
            }

            // A priority binding is a list of bindings, each should be set to TwoWay
            PriorityBinding priBinding = bindingBase as PriorityBinding;
            if (priBinding != null)
            {
                Collection<BindingBase> subBindings = priBinding.Bindings;
                int count = subBindings.Count;
                for (int i = 0; i < count; i++)
                {
                    EnsureTwoWayIfNotOneWay(subBindings[i]);
                }
            }
        }

        internal static BindingExpression GetBindingExpression(FrameworkElement element, DependencyProperty dp)
        {
            if (element != null)
            {
                return element.GetBindingExpression(dp);
            }

            return null;
        }

        internal static void UpdateSource(FrameworkElement element, DependencyProperty dp)
        {
            BindingExpression binding = DataGridHelper.GetBindingExpression(element, dp);
            if (binding != null)
            {
                binding.UpdateSource();
            }
        }

        internal static void UpdateTarget(FrameworkElement element, DependencyProperty dp)
        {
            BindingExpression binding = DataGridHelper.GetBindingExpression(element, dp);
            if (binding != null)
            {
                binding.UpdateTarget();
            }
        }

        internal static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
        {
            if (IsDefaultValue(column, columnProperty))
            {
                content.ClearValue(contentProperty);
            }
            else
            {
                content.SetValue(contentProperty, column.GetValue(columnProperty));
            }
        }

        internal static string GetPathFromBinding(Binding binding)
        {
            if (binding != null)
            {
                if (!string.IsNullOrEmpty(binding.XPath))
                {
                    return binding.XPath;
                }
                else if (binding.Path != null)
                {
                    return binding.Path.Path;
                }
            }

            return null;
        }

        #endregion

        #region Other Helpers

        /// <summary>
        ///     Method which takes in DataGridHeadersVisibility parameter
        ///     and determines if row headers are visible.
        /// </summary>
        public static bool AreRowHeadersVisible(DataGridHeadersVisibility headersVisibility)
        {
            return (headersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row;
        }

        /// <summary>
        ///     Helper method which coerces a value such that it satisfies min and max restrictions
        /// </summary>
        public static double CoerceToMinMax(double value, double minValue, double maxValue)
        {
            value = Math.Max(value, minValue);
            value = Math.Min(value, maxValue);
            return value;
        }

        /// <summary>
        ///     Helper to check if TextCompositionEventArgs.Text has any non
        ///     escape characters.
        /// </summary>
        public static bool HasNonEscapeCharacters(TextCompositionEventArgs textArgs)
        {
            if (textArgs != null)
            {
                string text = textArgs.Text;
                for (int i = 0, count = text.Length; i < count; i++)
                {
                    if (text[i] != _escapeChar)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private const char _escapeChar = '\u001b';

        #endregion
    }
}
