// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using SSWCP = Silverlight::System.Windows.Controls.Primitives;

namespace System.Windows.Controls.Input.Design
{
    /// <summary>
    /// To register design time metadata for Accordion.
    /// </summary>
    internal class LinearClipperMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for Accordion.
        /// </summary>
        public LinearClipperMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCP.LinearClipper),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCP.LinearClipper>(x => x.RatioVisible),
                        new EditorBrowsableAttribute(EditorBrowsableState.Always),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(
                        "ExpandDirection",
                        new EditorBrowsableAttribute(EditorBrowsableState.Always),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWCP.LinearClipper>(x => x.RatioVisible)));

#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.BasicControls, true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCP.LinearClipper>(x => x.RatioVisible),
                        new Attribute[] { new NumberRangesAttribute(null, 0.0, 1.0, null, null) });
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCP.LinearClipper>(x => x.RatioVisible),
                        new Attribute[] { new NumberIncrementsAttribute(null, 0.0005, null) });
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCP.LinearClipper>(x => x.RatioVisible),
                        new Attribute[] { new NumberFormatAttribute("0'%", 3, 100.0) });
#endif
                });
        }
    }
}