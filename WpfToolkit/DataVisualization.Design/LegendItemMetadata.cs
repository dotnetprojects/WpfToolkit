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
using SSWCDC = Silverlight::System.Windows.Controls.DataVisualization.Charting;

namespace System.Windows.Controls.DataVisualization.Design
{
    /// <summary>
    /// To register design time metadata for SSWCD.Legend.
    /// </summary>
    internal class LegendItemMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCD.Legend.
        /// </summary>
        public LegendItemMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDC.LegendItem),
                b =>
                {
                    b.AddCustomAttributes(new DefaultBindingPropertyAttribute(
                        Extensions.GetMemberName<SSWCDC.LegendItem>(x => x.Content)));

#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.DataVisualizationControlParts, false));
#endif
                });
        }
    }
}