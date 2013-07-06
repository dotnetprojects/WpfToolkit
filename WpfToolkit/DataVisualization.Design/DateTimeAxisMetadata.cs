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
    /// To register design time metadata for SSWCDC.DateTimeAxis.
    /// </summary>
    internal class DateTimeAxisMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCDC.DateTimeAxis.
        /// </summary>
        public DateTimeAxisMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDC.DateTimeAxis),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DateTimeAxis>(x => x.Interval),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DateTimeAxis>(x => x.IntervalType),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DateTimeAxis>(x => x.Maximum),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DateTimeAxis>(x => x.Minimum),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                });
        }
    }
}