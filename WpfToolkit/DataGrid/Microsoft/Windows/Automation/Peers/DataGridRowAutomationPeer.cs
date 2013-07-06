using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using MS.Internal;

namespace Microsoft.Windows.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for DataGridRow
    /// </summary>
    public sealed class DataGridRowAutomationPeer : FrameworkElementAutomationPeer
    {
        #region Constructors

        /// <summary>
        /// AutomationPeer for DataGridRow
        /// </summary>
        /// <param name="owner">DataGridRow</param>
        public DataGridRowAutomationPeer(DataGridRow owner)
            : base(owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            UpdateEventSource();
        }

        #endregion

        #region AutomationPeer Overrides

        /// <summary>
        /// Gets the control type for the element that is associated with the UI Automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.DataItem;
        }

        /// <summary>
        /// Called by GetClassName that gets a human readable name that, in addition to AutomationControlType, 
        /// differentiates the control represented by this AutomationPeer.
        /// </summary>
        /// <returns>The string that contains the name.</returns>
        protected override string GetClassNameCore()
        {
            return Owner.GetType().Name;
        }

        /// 
        protected override List<AutomationPeer> GetChildrenCore()
        {
            List<AutomationPeer> children = new List<AutomationPeer>(3);

            // Step 1: Add row header if exists
            AutomationPeer dataGridRowHeaderAutomationPeer = RowHeaderAutomationPeer;
            if (dataGridRowHeaderAutomationPeer != null)
            {
                children.Add(dataGridRowHeaderAutomationPeer);
            }

            // Step 2: Add all cells
            DataGridItemAutomationPeer itemPeer = this.EventsSource as DataGridItemAutomationPeer;
            if (itemPeer != null)
            {
                children.AddRange(itemPeer.GetCellItemPeers());
            }
            
            // Step 3: Add DetailsPresenter last if exists
            AutomationPeer dataGridDetailsPresenterAutomationPeer = DetailsPresenterAutomationPeer;
            if (dataGridDetailsPresenterAutomationPeer != null)
            {
                children.Add(dataGridDetailsPresenterAutomationPeer);
            }

            return children;
        }

        override protected bool IsOffscreenCore()
        {
            if (!Owner.IsVisible)
                return true;

            Rect boundingRect = DataGridAutomationPeer.CalculateVisibleBoundingRect(this.Owner);
            return DoubleUtil.AreClose(boundingRect, Rect.Empty) || DoubleUtil.AreClose(boundingRect.Height, 0.0) || DoubleUtil.AreClose(boundingRect.Width, 0.0);
        }

        #endregion

        #region Private helpers
        internal AutomationPeer RowHeaderAutomationPeer
        {
            get
            {
                DataGridRowHeader dataGridRowHeader = OwningDataGridRow.RowHeader;
                if (dataGridRowHeader != null)
                {
                    return CreatePeerForElement(dataGridRowHeader);
                }

                return null;
            }
        }

        private AutomationPeer DetailsPresenterAutomationPeer
        {
            get
            {
                DataGridDetailsPresenter dataGridDetailsPresenter = OwningDataGridRow.DetailsPresenter;
                if (dataGridDetailsPresenter != null)
                {
                    return CreatePeerForElement(dataGridDetailsPresenter);
                }

                return null;
            }
        }

        internal void UpdateEventSource()
        {
            DataGrid dataGrid = OwningDataGridRow.DataGridOwner;
            if (dataGrid != null)
            {
                DataGridAutomationPeer dataGridAutomationPeer = CreatePeerForElement(dataGrid) as DataGridAutomationPeer;
                if (dataGridAutomationPeer != null)
                {
                    AutomationPeer itemAutomationPeer = dataGridAutomationPeer.GetOrCreateItemPeer(OwningDataGridRow.Item);
                    if (itemAutomationPeer != null)
                    {
                        this.EventsSource = itemAutomationPeer;
                    }
                }
            }
        }

        private DataGridRow OwningDataGridRow
        {
            get
            {
                return (DataGridRow)Owner;
            }
        }

        #endregion
    }
}
