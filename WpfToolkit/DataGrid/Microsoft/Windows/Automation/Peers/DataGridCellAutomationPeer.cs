using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using MS.Internal;

namespace Microsoft.Windows.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for DataGridCell
    /// </summary>
    public sealed class DataGridCellAutomationPeer : FrameworkElementAutomationPeer
    {
        #region Constructors

        /// <summary>
        /// AutomationPeer for DataGridCell.
        /// This automation peer should not be part of the automation tree.
        /// It should act as a wrapper peer for DataGridCellItemAutomationPeer
        /// </summary>
        /// <param name="owner">DataGridCell</param>
        public DataGridCellAutomationPeer(DataGridCell owner)
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
            return AutomationControlType.Custom;
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
        override protected bool IsOffscreenCore()
        {
            if (!Owner.IsVisible)
                return true;

            Rect boundingRect = DataGridAutomationPeer.CalculateVisibleBoundingRect(this.Owner);
            return DoubleUtil.AreClose(boundingRect, Rect.Empty) || DoubleUtil.AreClose(boundingRect.Height, 0.0) || DoubleUtil.AreClose(boundingRect.Width, 0.0);
        }

        #endregion

        #region Private Helpers
        private void UpdateEventSource()
        {
            DataGridCell cell = (DataGridCell)Owner;
            DataGrid dataGrid = cell.DataGridOwner;
            if (dataGrid != null)
            {
                DataGridAutomationPeer dataGridAutomationPeer = CreatePeerForElement(dataGrid) as DataGridAutomationPeer;
                if (dataGridAutomationPeer != null)
                {
                    DataGridItemAutomationPeer itemAutomationPeer = dataGridAutomationPeer.GetOrCreateItemPeer(cell.DataContext);
                    if (itemAutomationPeer != null)
                    {
                        DataGridCellItemAutomationPeer cellItemAutomationPeer = itemAutomationPeer.GetOrCreateCellItemPeer(cell.Column);
                        this.EventsSource = cellItemAutomationPeer;
                    }
                }
            }
        }
        #endregion
    }
}