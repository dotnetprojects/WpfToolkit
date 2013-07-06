using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;

namespace Microsoft.Windows.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for a cell item in a DataGridRow.
    /// Cell may not have a visual container if it is scrolled out of view.
    /// </summary>
    public sealed class DataGridCellItemAutomationPeer : AutomationPeer,
        IGridItemProvider, ITableItemProvider, IInvokeProvider, IScrollItemProvider, ISelectionItemProvider
    {
        #region Constructors

        /// <summary>
        /// AutomationPeer for an item in a DataGrid
        /// </summary>
        public DataGridCellItemAutomationPeer(object item, DataGridColumn dataGridColumn) : base()
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (dataGridColumn == null)
            {
                throw new ArgumentNullException("dataGridColumn");
            }

            _item = item;
            _column = dataGridColumn;
        }

        #endregion

        #region AutomationPeer Overrides
        
        ///
        protected override string GetAcceleratorKeyCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetAcceleratorKey() : string.Empty;
        }

        ///
        protected override string GetAccessKeyCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetAccessKey() : string.Empty;
        }

        ///
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        ///
        protected override string GetAutomationIdCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetAutomationId() : string.Empty;
        }

        ///
        protected override Rect GetBoundingRectangleCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetBoundingRectangle() : new Rect();
        }

        ///
        protected override List<AutomationPeer> GetChildrenCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetChildren() : null;
        }
        
        ///
        protected override string GetClassNameCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetClassName() : string.Empty;
        }

        ///
        protected override Point GetClickablePointCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetClickablePoint() : new Point(double.NaN, double.NaN);
        }

        ///
        protected override string GetHelpTextCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetHelpText() : string.Empty;
        }

        ///
        protected override string GetItemStatusCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetItemStatus() : string.Empty;
        }

        ///
        protected override string GetItemTypeCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetItemType() : string.Empty;
        }

        ///
        protected override AutomationPeer GetLabeledByCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetLabeledBy() : null;
        }

        ///
        protected override string GetLocalizedControlTypeCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetLocalizedControlType() : base.GetLocalizedControlTypeCore();
        }

        ///
        protected override string GetNameCore()
        {
            return SR.Get(SRID.DataGridCellItemAutomationPeer_NameCoreFormat, _item, _column.DisplayIndex);
        }

        ///
        protected override AutomationOrientation GetOrientationCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.GetOrientation() : AutomationOrientation.None;
        }

        ///
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Invoke:
                    if (!this.OwningDataGrid.IsReadOnly && !_column.IsReadOnly)
                    {
                        return this;
                    }

                    break;
                case PatternInterface.SelectionItem:
                case PatternInterface.ScrollItem:
                case PatternInterface.GridItem:
                case PatternInterface.TableItem:
                    return this;
            }

            return null;
        }
        
        ///
        protected override bool HasKeyboardFocusCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.HasKeyboardFocus() : false;
        }

        ///
        protected override bool IsContentElementCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.IsContentElement() : true;
        }

        ///
        protected override bool IsControlElementCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.IsControlElement() : true;
        }

        ///
        protected override bool IsEnabledCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.IsEnabled() : true;
        }

        ///
        protected override bool IsKeyboardFocusableCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.IsKeyboardFocusable() : false;
        }

        ///
        protected override bool IsOffscreenCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.IsOffscreen() : true;
        }

        ///
        protected override bool IsPasswordCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.IsPassword() : false;
        }

        ///
        protected override bool IsRequiredForFormCore()
        {
            return (this.OwningCellPeer != null) ? this.OwningCellPeer.IsRequiredForForm() : false;
        }

        ///
        protected override void SetFocusCore()
        {
            if (this.OwningCellPeer != null && this.OwningCellPeer.Owner.Focusable)
            {
                this.OwningCellPeer.SetFocus();
            }
        }
        
        #endregion

        #region IGridItemProvider

        int IGridItemProvider.Column
        {
            get
            {
                return this.OwningDataGrid.Columns.IndexOf(this._column);
            }
        }

        int IGridItemProvider.ColumnSpan
        {
            get
            {
                return 1;
            }
        }

        IRawElementProviderSimple IGridItemProvider.ContainingGrid
        {
            get
            { 
                return this.ContainingGrid;
            }
        }

        int IGridItemProvider.Row
        {
            get
            {
                return this.OwningDataGrid.Items.IndexOf(this._item);
            }
        }

        int IGridItemProvider.RowSpan
        {
            get
            {
                return 1;
            }
        }

        #endregion

        #region ITableItemProvider

        IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
        {
            if (this.OwningDataGrid != null &&
                (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Column) == DataGridHeadersVisibility.Column &&
                this.OwningDataGrid.ColumnHeadersPresenter != null)
            {
                DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = this.OwningDataGrid.ColumnHeadersPresenter;
                DataGridColumnHeader dataGridColumnHeader = dataGridColumnHeadersPresenter.ItemContainerGenerator.ContainerFromIndex(this.OwningDataGrid.Columns.IndexOf(_column)) as DataGridColumnHeader;
                if (dataGridColumnHeader != null)
                {
                    AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(dataGridColumnHeader);
                    if (peer != null)
                    {
                        List<IRawElementProviderSimple> providers = new List<IRawElementProviderSimple>(1);
                        providers.Add(ProviderFromPeer(peer));
                        return providers.ToArray();
                    }
                }
            }

            return null;
        }

        IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
        {
            if (this.OwningDataGrid != null &&
                (this.OwningDataGrid.HeadersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row)
            {
                DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDataGrid) as DataGridAutomationPeer;
                DataGridItemAutomationPeer dataGridItemAutomationPeer = dataGridAutomationPeer.GetOrCreateItemPeer(_item);
                if (dataGridItemAutomationPeer != null)
                {
                    AutomationPeer rowHeaderAutomationPeer = dataGridItemAutomationPeer.RowHeaderAutomationPeer;
                    if (rowHeaderAutomationPeer != null)
                    {
                        List<IRawElementProviderSimple> providers = new List<IRawElementProviderSimple>(1);
                        providers.Add(ProviderFromPeer(rowHeaderAutomationPeer));
                        return providers.ToArray();
                    }
                }
            }

            return null;
        }

        #endregion

        #region IInvokeProvider

        void IInvokeProvider.Invoke()
        {
            if (this.OwningDataGrid.IsReadOnly || _column.IsReadOnly)
            {
                return;
            }

            EnsureEnabled();

            bool success = false;

            // If the current cell is virtualized - scroll into view
            if (this.OwningCell == null)
            {
                this.OwningDataGrid.ScrollIntoView(_item, _column);
            }

            if (this.OwningCell != null)
            {
                IEditableCollectionView iecv = (IEditableCollectionView)this.OwningDataGrid.Items;
                if (iecv.CurrentEditItem == _item)
                {
                    success = this.OwningDataGrid.CommitEdit();
                }
                else
                {
                    this.OwningDataGrid.UnselectAll();
                    this.OwningCell.Focus();
                    success = this.OwningDataGrid.BeginEdit();
                }
            }

            if (!success)
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region IScrollItemProvider
        
        void IScrollItemProvider.ScrollIntoView()
        {
            this.OwningDataGrid.ScrollIntoView(_item, _column);
        }
        
        #endregion
        
        #region ISelectionItemProvider
        
        bool ISelectionItemProvider.IsSelected
        {
            get
            {
                return this.OwningDataGrid.SelectedCellsInternal.Contains(new DataGridCellInfo(_item, _column));
            }
        }

        IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
        {
            get
            {
                return this.ContainingGrid;
            }
        }

        void ISelectionItemProvider.AddToSelection()
        {
            // If item is already selected - do nothing
            DataGridCellInfo currentCellInfo = new DataGridCellInfo(_item, _column);
            if (this.OwningDataGrid.SelectedCellsInternal.Contains(currentCellInfo))
            {
                return;
            }

            EnsureEnabled();
        
            if (this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Single &&
                this.OwningDataGrid.SelectedCells.Count > 0)
            {
                throw new InvalidOperationException();
            }

            this.OwningDataGrid.SelectedCellsInternal.Add(currentCellInfo);
        }

        void ISelectionItemProvider.RemoveFromSelection()
        {
            EnsureEnabled();

            DataGridCellInfo currentCellInfo = new DataGridCellInfo(_item, _column);
            if (this.OwningDataGrid.SelectedCellsInternal.Contains(currentCellInfo))
            {
                this.OwningDataGrid.SelectedCellsInternal.Remove(currentCellInfo);
            }
        }

        void ISelectionItemProvider.Select()
        {
            EnsureEnabled();

            DataGridCellInfo currentCellInfo = new DataGridCellInfo(_item, _column);
            this.OwningDataGrid.SelectOnlyThisCell(currentCellInfo);
        }

        #endregion

        #region Private Methods

        private void EnsureEnabled()
        {
            if (!OwningDataGrid.IsEnabled)
            {
                throw new ElementNotEnabledException();
            }
        }
        
        #endregion

        #region Private Properties

        private DataGrid OwningDataGrid
        {
            get
            {
                return _column.DataGridOwner;
            }
        }

        // This may be null if the cell is virtualized
        private DataGridCell OwningCell
        {
            get
            {
                return this.OwningDataGrid.TryFindCell(_item, _column);
            }
        }

        internal DataGridCellAutomationPeer OwningCellPeer
        {
            get
            {
                DataGridCellAutomationPeer cellPeer = null;
                DataGridCell cell = this.OwningCell;
                if (cell != null)
                {
                    cellPeer = FrameworkElementAutomationPeer.CreatePeerForElement(cell) as DataGridCellAutomationPeer;
                    cellPeer.EventsSource = this;
                }

                return cellPeer;
            }
        }

        private IRawElementProviderSimple ContainingGrid
        {
            get
            {
                AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(OwningDataGrid);
                if (peer != null)
                {
                    return ProviderFromPeer(peer);
                }

                return null;
            }
        }

        #endregion

        #region Data

        private object _item;
        private DataGridColumn _column;

        #endregion
    }
}
