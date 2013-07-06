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

namespace System.Windows.Controls.Layout.Design
{
    /// <summary>
    /// To register design time metadata for SSWC.ExpandableContentControl.
    /// </summary>
    internal class ExpandableContentControlMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWC.ExpandableContentControl.
        /// </summary>
        public ExpandableContentControlMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCP.ExpandableContentControl),
                b =>
                {
                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWCP.ExpandableContentControl>(x => x.Content)));

#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.BasicControls, false));
#endif
                });
        }
    }
}
