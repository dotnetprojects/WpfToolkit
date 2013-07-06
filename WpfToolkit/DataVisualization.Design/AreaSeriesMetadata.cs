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
    /// To register design time metadata for SSWCDC.AreaSeries.
    /// </summary>
    internal class AreaSeriesMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCDC.AreaSeries.
        /// </summary>
        public AreaSeriesMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDC.AreaSeries),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.ActualDependentRangeAxis),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.ActualIndependentAxis),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.DependentRangeAxis),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.IndependentAxis),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.Geometry),
                        new CategoryAttribute(Properties.Resources.DataVisualization));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.DataPointStyle),
                        new CategoryAttribute(Properties.Resources.DataVisualizationStyling));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.PathStyle),
                        new CategoryAttribute(Properties.Resources.DataVisualizationStyling));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.DependentRangeAxis),
                        new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.AreaSeries>(x => x.IndependentAxis),
                        new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
                });
        }
    }
}