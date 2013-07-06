// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.Collapse()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.Expand()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.ExpandCollapseState", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.AddToSelection()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.IsSelected", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.RemoveFromSelection()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.Select()", Justification = "Required for subset compat with WPF")]
[assembly: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.SelectionContainer", Justification = "Required for subset compat with WPF")]

namespace System.Windows.Automation.Peers
{
    /// <summary>
    /// Exposes AccordionItem types to UI Automation.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
#if SILVERLIGHT
    public class AccordionItemAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, ISelectionItemProvider
#else
    public class AccordionItemAutomationPeer : ItemAutomationPeer, IExpandCollapseProvider, ISelectionItemProvider
#endif
    {
        /// <summary>
        /// Gets the AccordionItem that owns this AccordionItemAutomationPeer.
        /// </summary>
        private AccordionItem OwnerAccordionItem
        {
#if SILVERLIGHT
            get { return (AccordionItem)Owner; }
#else
            get { return base.Item as AccordionItem; }
#endif
        }

#if SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the AccordionAutomationPeer class.
        /// </summary>
        /// <param name="owner">
        /// The Accordion that is associated with this
        /// AccordionAutomationPeer.
        /// </param>
        public AccordionItemAutomationPeer(AccordionItem owner)
            : base(owner)
        {
        }
#else
        /// <summary>
        /// Initializes a new instance of the AccordionAutomationPeer class.
        /// </summary>
        /// <param name="item">
        /// The item associated with this AutomationPeer 
        /// </param>
        /// <param name="itemsControlAutomationPeer">
        /// The Accordion that is associated with this item.
        /// </param>
        public AccordionItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer)
            : base(item, itemsControlAutomationPeer)
        {
        }
#endif

        /// <summary>
        /// Gets the control type for the AccordionItem that is associated
        /// with this AccordionItemAutomationPeer.  This method is called by
        /// GetAutomationControlType.
        /// </summary>
        /// <returns>Custom AutomationControlType.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ListItem;
        }

        /// <summary>
        /// Gets the name of the AccordionItem that is associated with this
        /// AccordionItemAutomationPeer.  This method is called by GetClassName.
        /// </summary>
        /// <returns>The name AccordionItem.</returns>
        protected override string GetClassNameCore()
        {
            return "AccordionItem";
        }

        /// <summary>
        /// Gets the control pattern for the AccordionItem that is associated
        /// with this AccordionItemAutomationPeer.
        /// </summary>
        /// <param name="patternInterface">The desired PatternInterface.</param>
        /// <returns>The desired AutomationPeer or null.</returns>
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.ExpandCollapse ||
                patternInterface == PatternInterface.SelectionItem)
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Gets the state (expanded or collapsed) of the Accordion.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
        {
            get
            {
                return OwnerAccordionItem.IsSelected ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed;
            }
        }

        /// <summary>
        /// Collapses the AccordionItem.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void IExpandCollapseProvider.Collapse()
        {
            if (!IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            AccordionItem owner = OwnerAccordionItem;
            if (owner.IsLocked)
            {
                throw new InvalidOperationException(Controls.Properties.Resources.Automation_OperationCannotBePerformed);
            }

            owner.IsSelected = false;
        }

        /// <summary>
        /// Expands the AccordionItem.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void IExpandCollapseProvider.Expand()
        {
            if (!IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            AccordionItem owner = OwnerAccordionItem;
            if (owner.IsLocked)
            {
                throw new InvalidOperationException(Controls.Properties.Resources.Automation_OperationCannotBePerformed);
            }

            owner.IsSelected = true;
        }

        /// <summary>
        /// Adds the AccordionItem to the collection of selected items.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.AddToSelection()
        {
            AccordionItem owner = OwnerAccordionItem;
            Accordion parent = owner.ParentAccordion;
            if (parent == null)
            {
                throw new InvalidOperationException(Controls.Properties.Resources.Automation_OperationCannotBePerformed);
            }
            parent.SelectedItems.Add(owner);
        }

        /// <summary>
        /// Gets a value indicating whether the Accordion is selected.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        bool ISelectionItemProvider.IsSelected
        {
            get { return OwnerAccordionItem.IsSelected; }
        }

        /// <summary>
        /// Removes the current Accordion from the collection of selected
        /// items.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.RemoveFromSelection()
        {
            AccordionItem owner = OwnerAccordionItem;
            Accordion parent = owner.ParentAccordion;
            if (parent == null)
            {
                throw new InvalidOperationException(Controls.Properties.Resources.Automation_OperationCannotBePerformed);
            }
            parent.SelectedItems.Remove(owner);
        }

        /// <summary>
        /// Clears selection from currently selected items and then proceeds to
        /// select the current Accordion.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.Select()
        {
            OwnerAccordionItem.IsSelected = true;
        }

        /// <summary>
        /// Gets the UI Automation provider that implements ISelectionProvider
        /// and acts as the container for the calling object.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
        {
            get
            {
                Accordion parent = OwnerAccordionItem.ParentAccordion;
                if (parent != null)
                {
#if SILVERLIGHT
                    AutomationPeer peer = FromElement(parent);
#else
                    AutomationPeer peer = UIElementAutomationPeer.FromElement(parent);
#endif
                    if (peer != null)
                    {
                        return ProviderFromPeer(peer);
                    }
                }
                return null;
            }
        }
    }
}
