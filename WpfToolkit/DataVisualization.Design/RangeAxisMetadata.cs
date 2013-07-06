// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

extern alias Silverlight;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design.Metadata;
using SSWCDC = Silverlight::System.Windows.Controls.DataVisualization.Charting;

namespace System.Windows.Controls.DataVisualization.Design
{
    /// <summary>
    /// To register design time metadata for SSWCDC.RangeAxis.
    /// </summary>
    internal class RangeAxisMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCDC.RangeAxis.
        /// </summary>
        public RangeAxisMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDC.RangeAxis),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.RangeAxis>(x => x.MinorTickMarkStyle),
                        new CategoryAttribute(Properties.Resources.DataVisualizationStyling));
                });
        }
    }
}