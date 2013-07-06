// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

extern alias Silverlight;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using SSWCD = Silverlight::System.Windows.Controls.DataVisualization;

namespace System.Windows.Controls.DataVisualization.Design
{
    /// <summary>
    /// To register design time metadata for SSWCD.Legend.
    /// </summary>
    internal class TitleMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCD.Legend.
        /// </summary>
        public TitleMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCD.Title),
                b =>
                {
                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWCD.Title>(x => x.Content)));

#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.DataVisualizationControlParts, false));
#endif
                });
        }
    }
}