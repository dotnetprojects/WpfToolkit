// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using SSWC = Silverlight::System.Windows.Controls;

namespace System.Windows.Controls.Layout.Design
{
    /// <summary>
    /// To register design time metadata for Accordion.
    /// </summary>
    internal class AccordionMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for Accordion.
        /// </summary>
        public AccordionMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWC.Accordion),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Accordion>(x => x.ExpandDirection),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Accordion>(x => x.SelectionMode),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Accordion>(x => x.SelectionSequence),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Accordion>(x => x.DisplayMemberPath),
                        new EditorBrowsableAttribute(EditorBrowsableState.Always));

#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Controls, true));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Accordion>(x => x.SelectedItem),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.Accordion>(x => x.ItemsSource),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Accordion>(x => x.ItemContainerStyle),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.Accordion>(x => x.ItemsSource),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Accordion>(x => x.ContentTemplate),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.Accordion>(x => x.ItemsSource),
                            true));
#endif
                });
        }
    }
}
