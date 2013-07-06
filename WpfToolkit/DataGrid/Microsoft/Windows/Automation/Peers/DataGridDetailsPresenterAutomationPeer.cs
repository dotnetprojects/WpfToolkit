using System;
using System.Collections.Generic;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;

namespace Microsoft.Windows.Automation.Peers
{
    /// <summary>
    /// AutomationPeer for DataGridDetailsPresenter
    /// </summary>
    public sealed class DataGridDetailsPresenterAutomationPeer : FrameworkElementAutomationPeer
    {
        #region Constructors

        /// <summary>
        /// AutomationPeer for DataGridDetailsPresenter
        /// </summary>
        /// <param name="owner">DataGridDetailsPresenter</param>
        public DataGridDetailsPresenterAutomationPeer(DataGridDetailsPresenter owner)
            : base(owner)
        {
        }

        #endregion

        #region AutomationPeer Overrides

        ///
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        ///
        protected override bool IsContentElementCore()
        {
            return false;
        }

        #endregion
    }
}