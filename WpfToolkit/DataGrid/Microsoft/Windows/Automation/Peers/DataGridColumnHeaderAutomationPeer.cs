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
    /// AutomationPeer for DataGridColumnHeader
    /// </summary>
    public sealed class DataGridColumnHeaderAutomationPeer : ButtonBaseAutomationPeer,
        IInvokeProvider, IScrollItemProvider, ITransformProvider
    {
        #region Constructors

        /// <summary>
        /// AutomationPeer for DataGridColumnHeader
        /// </summary>
        /// <param name="owner">DataGridColumnHeader</param>
        public DataGridColumnHeaderAutomationPeer(DataGridColumnHeader owner)
            : base(owner)
        {
        }

        #endregion

        #region AutomationPeer Overrides

        /// <summary>
        /// Gets the control type for the element that is associated with the UI Automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.HeaderItem;
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

        /// <summary>
        /// Gets the control pattern that is associated with the specified System.Windows.Automation.Peers.PatternInterface.
        /// </summary>
        /// <param name="patternInterface">A value from the System.Windows.Automation.Peers.PatternInterface enumeration.</param>
        /// <returns>The object that supports the specified pattern, or null if unsupported.</returns>
        public override object GetPattern(PatternInterface patternInterface)
        {
            switch (patternInterface)
            {
                case PatternInterface.Invoke:
                    {
                        if (this.OwningHeader.Column != null && this.OwningHeader.Column.CanUserSort)
                        {
                            return this;
                        }

                        break;
                    }

                case PatternInterface.ScrollItem:
                    {
                        return this;
                    }

                case PatternInterface.Transform:
                    {
                        if (this.OwningHeader.Column != null && this.OwningHeader.Column.DataGridOwner.CanUserResizeColumns)
                        {
                            return this;
                        }
                        
                        break;
                    }
            }

            return base.GetPattern(patternInterface);
        }

        /// <summary>
        /// Gets a value that specifies whether the element is a content element.
        /// </summary>
        /// <returns>true if the element is a content element; otherwise false</returns>
        protected override bool IsContentElementCore()
        {
            return false;
        }

        override protected bool IsOffscreenCore()
        {
            if (!Owner.IsVisible)
                return true;

            Rect boundingRect = DataGridAutomationPeer.CalculateVisibleBoundingRect(this.Owner);
            return DoubleUtil.AreClose(boundingRect, Rect.Empty) || DoubleUtil.AreClose(boundingRect.Height, 0.0) || DoubleUtil.AreClose(boundingRect.Width, 0.0);
        }

        #endregion

        #region IInvokeProvider

        void IInvokeProvider.Invoke()
        {
            this.OwningHeader.Invoke();
        }

        #endregion

        #region IScrollItemProvider

        void IScrollItemProvider.ScrollIntoView()
        {
            if (this.OwningHeader.Column != null)
            {
                DataGrid dataGrid = this.OwningHeader.Column.DataGridOwner;
                if (dataGrid != null)
                {
                    dataGrid.ScrollIntoView(null, this.OwningHeader.Column);
                }
            }
        }

        #endregion

        #region ITransformProvider

        bool ITransformProvider.CanMove 
        { 
            get 
            { 
                return false; 
            } 
        }

        bool ITransformProvider.CanResize 
        { 
            get 
            { 
                return this.OwningHeader.Column != null && this.OwningHeader.Column.DataGridOwner.CanUserResizeColumns; 
            } 
        }

        bool ITransformProvider.CanRotate 
        { 
            get 
            { 
                return false; 
            } 
        }

        void ITransformProvider.Move(double x, double y)
        {
            throw new InvalidOperationException();
        } 

        void ITransformProvider.Resize(double width, double height)
        {
            if (this.OwningHeader.Column != null && this.OwningHeader.Column.DataGridOwner.CanUserResizeColumns)
            {
                this.OwningHeader.Column.Width = new DataGridLength(width);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        void ITransformProvider.Rotate(double degrees)
        {
            throw new InvalidOperationException();
        }

        #endregion

        #region Properties

        private DataGridColumnHeader OwningHeader
        {
            get
            {
                return (DataGridColumnHeader)Owner;
            }
        }

        #endregion
    }
}
