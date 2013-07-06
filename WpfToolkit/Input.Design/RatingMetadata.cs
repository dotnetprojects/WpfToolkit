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
using SSWC = Silverlight::System.Windows.Controls;

namespace System.Windows.Controls.Input.Design
{
    /// <summary>
    /// To register design time metadata for Accordion.
    /// </summary>
    internal class RatingMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for Accordion.
        /// </summary>
        public RatingMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWC.Rating),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Rating>(x => x.SelectionMode),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Rating>(x => x.Value),
                        new EditorBrowsableAttribute(EditorBrowsableState.Always),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.Rating>(x => x.IsReadOnly),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWC.Rating>(x => x.Value)));

#if MWD40
                    b.AddCustomAttributes(new FeatureAttribute(typeof(RatingDefaultInitializer)));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Controls, true));
#endif
                });
        }
    }
}