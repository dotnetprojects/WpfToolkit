using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Windows.Controls;

namespace Microsoft.Windows.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for an item in a DataGrid
    /// This automation peer correspond to a row data item which may not have a visual container
    /// </summary>
    public sealed class DataGridItemAutomationPeer : AutomationPeer,
        IInvokeProvider, IScrollItemProvider, ISelectionItemProvider, ISelectionProvider
    {
        #region Constructors

        /// <summary>
        /// AutomationPeer for an item in a DataGrid
        /// </summary>
        public DataGridItemAutomationPeer(object item, DataGrid dataGrid) : base()
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (dataGrid == null)
            {
                throw new ArgumentNullException("dataGrid");
            }

            _item = item;
            _dataGridAutomationPeer = FrameworkElementAutomationPeer.CreatePeerForElement(dataGrid);
        }

        #endregion

        #region AutomationPeer Overrides
        
        ///
        protected override string GetAcceleratorKeyCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetAcceleratorKey() : string.Empty;
        }

        ///
        protected override string GetAccessKeyCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetAccessKey() : string.Empty;
        }

        ///
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.DataItem;
        }

        ///
        protected override string GetAutomationIdCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetAutomationId() : string.Empty;
        }

        ///
        protected override Rect GetBoundingRectangleCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetBoundingRectangle() : new Rect();
        }

        ///
        protected override List<AutomationPeer> GetChildrenCore()
        {
            AutomationPeer wrapperPeer = this.OwningRowPeer;

            if (wrapperPeer != null)
            {
                // We need to update children manually since wrapperPeer is not in the Automation Tree
                // When containers are recycled the visual (DataGridRow) will point to a new item. 
                // WrapperPeer's children are the peers for DataGridRowHeader, DataGridCells and DataGridRowDetails.
                wrapperPeer.ResetChildrenCache();
                return wrapperPeer.GetChildren();
            }

            return GetCellItemPeers();
        }
        
        ///
        protected override string GetClassNameCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetClassName() : string.Empty;
        }

        ///
        protected override Point GetClickablePointCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetClickablePoint() : new Point(double.NaN, double.NaN);
        }

        ///
        protected override string GetHelpTextCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetHelpText() : string.Empty;
        }

        ///
        protected override string GetItemStatusCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetItemStatus() : string.Empty;
        }

        ///
        protected override string GetItemTypeCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetItemType() : string.Empty;
        }

        ///
        protected override AutomationPeer GetLabeledByCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetLabeledBy() : null;
        }

        ///
        protected override string GetLocalizedControlTypeCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetLocalizedControlType() : base.GetLocalizedControlTypeCore();
        }

        ///
        protected override string GetNameCore()
        {
            return _item.ToString();
        }

        ///
        protected override AutomationOrientation GetOrientationCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.GetOrientation() : AutomationOrientation.None;
        }

        ///
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Invoke:
                    {
                        if (!this.OwningDataGrid.IsReadOnly)
                        {
                            return this;
                        }

                        break;
                    }

                case PatternInterface.ScrollItem:
                case PatternInterface.Selection:
                    return this;
                case PatternInterface.SelectionItem:
                    if (IsRowSelectionUnit)
                    {
                        return this;
                    }
                    break;
            }

            return null;
        }

        ///
        protected override bool HasKeyboardFocusCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.HasKeyboardFocus() : false;
        }

        ///
        protected override bool IsContentElementCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.IsContentElement() : true;
        }

        ///
        protected override bool IsControlElementCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.IsControlElement() : true;
        }

        ///
        protected override bool IsEnabledCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.IsEnabled() : true;
        }

        ///
        protected override bool IsKeyboardFocusableCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.IsKeyboardFocusable() : false;
        }

        ///
        protected override bool IsOffscreenCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.IsOffscreen() : true;
        }

        ///
        protected override bool IsPasswordCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.IsPassword() : false;
        }

        ///
        protected override bool IsRequiredForFormCore()
        {
            return (this.OwningRowPeer != null) ? this.OwningRowPeer.IsRequiredForForm() : false;
        }

        ///
        protected override void SetFocusCore()
        {
            if (this.OwningRowPeer != null && this.OwningRowPeer.Owner.Focusable)
            {
                this.OwningRowPeer.SetFocus();
            }
        }
        
        #endregion

        #region IInvokeProvider

        // Invoking DataGrid item should commit the item if it is in edit mode
        // or BeginEdit if item is not in edit mode
        void IInvokeProvider.Invoke()
        {
            EnsureEnabled();

            if (this.OwningRowPeer == null)
            {
                this.OwningDataGrid.ScrollIntoView(_item);
            }
              
            bool success = false;
            if (this.OwningRow != null)
            {
                IEditableCollectionView iecv = (IEditableCollectionView)this.OwningDataGrid.Items;
                if (iecv.CurrentEditItem == _item)
                {
                    success = this.OwningDataGrid.CommitEdit();
                }
                else
                {
                    if (this.OwningDataGrid.Columns.Count > 0)
                    {
                        DataGridCell cell = this.OwningDataGrid.TryFindCell(_item, this.OwningDataGrid.Columns[0]);
                        if (cell != null)
                        {
                            this.OwningDataGrid.UnselectAll();
                            cell.Focus();
                            success = this.OwningDataGrid.BeginEdit();
                        }
                    }
                }
            }

            // Invoke on a NewItemPlaceholder row creates a new item.
            // BeginEdit on a NewItemPlaceholder row returns false.
            if (!success && !IsNewItemPlaceholder)
            {
                throw new InvalidOperationException(SR.Get(SRID.DataGrid_AutomationInvokeFailed));
            } 
        }

        #endregion

        #region IScrollItemProvider
        
        void IScrollItemProvider.ScrollIntoView()
        {
            this.OwningDataGrid.ScrollIntoView(_item);
        }
        
        #endregion

        #region ISelectionItemProvider
        
        bool ISelectionItemProvider.IsSelected
        {
            get
            {
                return this.OwningDataGrid.SelectedItems.Contains(_item);
            }
        }

        IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
        {
            get
            {
                return ProviderFromPeer(_dataGridAutomationPeer);
            }
        }

        void ISelectionItemProvider.AddToSelection()
        {
            if (!IsRowSelectionUnit)
            {
                throw new InvalidOperationException(SR.Get(SRID.DataGridRow_CannotSelectRowWhenCells));
            }

            // If item is already selected - do nothing
            if (this.OwningDataGrid.SelectedItems.Contains(_item))
            {
                return;
            }

            EnsureEnabled();
        
            if (this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Single &&
                this.OwningDataGrid.SelectedItems.Count > 0)
            {
                throw new InvalidOperationException();
            }

            if (this.OwningDataGrid.Items.Contains(_item))
            {
                this.OwningDataGrid.SelectedItems.Add(_item);
            }
        }

        void ISelectionItemProvider.RemoveFromSelection()
        {
            if (!IsRowSelectionUnit)
            {
                throw new InvalidOperationException(SR.Get(SRID.DataGridRow_CannotSelectRowWhenCells));
            }

            EnsureEnabled();

            if (this.OwningDataGrid.SelectedItems.Contains(_item))
            {
                this.OwningDataGrid.SelectedItems.Remove(_item);
            }
        }

        void ISelectionItemProvider.Select()
        {
            if (!IsRowSelectionUnit)
            {
                throw new InvalidOperationException(SR.Get(SRID.DataGridRow_CannotSelectRowWhenCells));
            }

            EnsureEnabled();

            this.OwningDataGrid.SelectedItem = _item;
        }
        
        #endregion

        #region ISelectionProvider
        
        bool ISelectionProvider.CanSelectMultiple
        {
            get
            {
                return this.OwningDataGrid.SelectionMode == DataGridSelectionMode.Extended;
            }
        }

        bool ISelectionProvider.IsSelectionRequired
        {
            get
            {
                return false;
            }
        }

        IRawElementProviderSimple[] ISelectionProvider.GetSelection()
        {
            DataGrid dataGrid = this.OwningDataGrid;
            if (dataGrid == null)
            {
                return null;
            }

            int rowIndex = dataGrid.Items.IndexOf(_item);

            // If row has selection
            if (rowIndex > -1 && dataGrid.SelectedCellsInternal.Intersects(rowIndex)) 
            {
                List<IRawElementProviderSimple> selectedProviders = new List<IRawElementProviderSimple>();

                for (int i = 0; i < this.OwningDataGrid.Columns.Count; i++)
                {
                    // cell is selected
                    if (dataGrid.SelectedCellsInternal.Contains(rowIndex, i)) 
                    {
                        DataGridColumn column = dataGrid.ColumnFromDisplayIndex(i);
                        DataGridCellItemAutomationPeer peer = GetOrCreateCellItemPeer(column);
                        if (peer != null)
                        {
                            selectedProviders.Add(ProviderFromPeer(peer));
                        }
                    }
                }

                if (selectedProviders.Count > 0)
                {
                    return selectedProviders.ToArray();
                }
            }

            return null;
        }

        #endregion

        #region Private Methods

        internal List<AutomationPeer> GetCellItemPeers()
        {
            List<AutomationPeer> peers = new List<AutomationPeer>();
            Dictionary<DataGridColumn, DataGridCellItemAutomationPeer> oldChildren = new Dictionary<DataGridColumn, DataGridCellItemAutomationPeer>(_itemPeers);
            _itemPeers.Clear();

            foreach (DataGridColumn column in this.OwningDataGrid.Columns)
            {
                DataGridCellItemAutomationPeer peer = null;
                bool peerExists = oldChildren.TryGetValue(column, out peer);
                if (!peerExists || peer == null)
                {
                    peer = new DataGridCellItemAutomationPeer(_item, column);
                }

                peers.Add(peer);
                _itemPeers.Add(column, peer);
            }

            return peers;
        }

        internal DataGridCellItemAutomationPeer GetOrCreateCellItemPeer(DataGridColumn column)
        {
            DataGridCellItemAutomationPeer peer = null;
            bool peerExists = _itemPeers.TryGetValue(column, out peer);
            if (!peerExists || peer == null)
            {
                peer = new DataGridCellItemAutomationPeer(_item, column);
                _itemPeers.Add(column, peer);
            }

            return peer;
        }

        internal AutomationPeer RowHeaderAutomationPeer
        {
            get
            {
                return (this.OwningRowPeer != null) ? this.OwningRowPeer.RowHeaderAutomationPeer : null;
            }
        }

        private void EnsureEnabled()
        {
            if (!_dataGridAutomationPeer.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }
        }
        
        #endregion

        #region Private Properties
        private bool IsRowSelectionUnit
        {
            get
            {
                return (this.OwningDataGrid != null &&
                    (this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.FullRow ||
                    this.OwningDataGrid.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader));
            }
        }

        private bool IsNewItemPlaceholder
        {
            get
            {
                return (_item == CollectionView.NewItemPlaceholder) || (_item == DataGrid.NewItemPlaceholder);
            }
        }

        private DataGrid OwningDataGrid
        {
            get
            {
                DataGridAutomationPeer gridPeer = _dataGridAutomationPeer as DataGridAutomationPeer;
                return (DataGrid)gridPeer.Owner;
            }
        }

        private DataGridRow OwningRow
        {
            get
            {
                return this.OwningDataGrid.ItemContainerGenerator.ContainerFromItem(_item) as DataGridRow;
            }
        }

        internal DataGridRowAutomationPeer OwningRowPeer
        {
            get
            {
                DataGridRowAutomationPeer rowPeer = null;
                DataGridRow row = this.OwningRow;
                if (row != null)
                {
                    rowPeer = FrameworkElementAutomationPeer.CreatePeerForElement(row) as DataGridRowAutomationPeer;
                }

                return rowPeer;
            }
        }

        #endregion

        #region Data

        private object _item;
        private AutomationPeer _dataGridAutomationPeer;
        private Dictionary<DataGridColumn, DataGridCellItemAutomationPeer> _itemPeers = new Dictionary<DataGridColumn, DataGridCellItemAutomationPeer>();

        #endregion
    }
}
