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
    /// To register design time metadata for SSWCD.LinearAxis.
    /// </summary>
    internal class LinearAxisMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCD.LinearAxis.
        /// </summary>
        public LinearAxisMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDC.LinearAxis),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.LinearAxis>(x => x.Interval),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                });
        }
    }
}