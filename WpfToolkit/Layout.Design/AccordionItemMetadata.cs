// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using SSWC = Silverlight::System.Windows.Controls;

namespace System.Windows.Controls.Layout.Design
{
    /// <summary>
    /// To register design time metadata for AccordionItem.
    /// </summary>
    internal class AccordionItemMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for AccordionItem.
        /// </summary>
        public AccordionItemMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWC.AccordionItem),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AccordionItem>(x => x.Header),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AccordionItem>(x => x.ExpandDirection),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AccordionItem>(x => x.IsSelected),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWC.AccordionItem>(x => x.Header)));

#if MWD40
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AccordionItem>(x => x.Content),
                        new AlternateContentPropertyAttribute());

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.BasicControls, false));

                    b.AddCustomAttributes(new FeatureAttribute(typeof(AccordionItemDefaultInitializer)));

                    b.AddCustomAttributes(new FeatureAttribute(typeof(AccordionItemIsSelectedDesignModeValueProvider)));
                    b.AddCustomAttributes(new FeatureAttribute(typeof(AccordionItemIsSelectedDesignModeValueProvider.AdornerProxy)));
#endif
                });
        }
    }
}
