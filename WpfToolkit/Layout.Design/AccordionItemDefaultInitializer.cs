// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using Microsoft.Windows.Design.Model;
using System.Windows.Controls.Design.Common;

namespace System.Windows.Controls.Layout.Design
{
    /// <summary>
    /// Default initializer for AccordionItem. 
    /// </summary>
    internal class AccordionItemDefaultInitializer : DefaultInitializer
    {
        /// <summary>
        /// Sets the default property values for AccordionItem.
        /// </summary>
        /// <param name="item">The AccordionItem ModelItem.</param>
        public override void InitializeDefaults(ModelItem item)
        {
            // <AccordionItem Content="AccordionItem" Header="AccordionItem" />
            string headerName = Extensions.GetMemberName<Silverlight::System.Windows.Controls.AccordionItem>(x => x.Header);
            string contentName = Extensions.GetMemberName<Silverlight::System.Windows.Controls.AccordionItem>(x => x.Content);
            item.Properties[headerName].SetValue(Properties.Resources.AccordionItem_Header);
            item.Properties[contentName].SetValue(Properties.Resources.AccordionItem_Content);
        }
    }
}