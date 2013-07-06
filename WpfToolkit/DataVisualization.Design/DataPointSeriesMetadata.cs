// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

extern alias Silverlight;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using SSWCDC = Silverlight::System.Windows.Controls.DataVisualization.Charting;

namespace System.Windows.Controls.DataVisualization.Design
{
    /// <summary>
    /// To register design time metadata for SSWCDC.DataPointSeries.
    /// </summary>
    internal class DataPointSeriesMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCDC.DataPointSeries.
        /// </summary>
        public DataPointSeriesMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDC.DataPointSeries),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.DependentValueBinding),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.IndependentValueBinding),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.SelectedItem),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.TransitionDuration),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
#if !NO_EASING_FUNCTIONS
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.TransitionEasingFunction),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
#endif

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.LegendItemStyle),
                        new CategoryAttribute(Properties.Resources.DataVisualizationStyling));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.AnimationSequence),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.DependentValuePath),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.IndependentValuePath),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.IsSelectionEnabled),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.ItemsSource),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

#if MWD40
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.IndependentValuePath),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.ItemsSource),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.IndependentValueBinding),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.ItemsSource),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.DependentValuePath),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.ItemsSource),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.DependentValueBinding),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWCDC.DataPointSeries>(x => x.ItemsSource),
                            true));
#endif
                });
        }
    }
}