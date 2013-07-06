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
    /// To register design time metadata for SSWC.TransitioningContentControl.
    /// </summary>
    internal class TransitioningContentControlMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWC.TransitioningContentControl.
        /// </summary>
        public TransitioningContentControlMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWC.TransitioningContentControl),
                b =>
                {
                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWC.TransitioningContentControl>(x => x.Content)));

#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.BasicControls, false));
#endif
                });
        }
    }
}
