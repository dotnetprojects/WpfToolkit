// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

extern alias Silverlight;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using SSWCD = Silverlight::System.Windows.Controls.DataVisualization;
using SSWCDC = Silverlight::System.Windows.Controls.DataVisualization.Charting;
using SSWCDCP = Silverlight::System.Windows.Controls.DataVisualization.Charting.Primitives;

namespace System.Windows.Controls.DataVisualization.VisualStudio.Design
{
    /// <summary>
    /// MetadataRegistration class.
    /// </summary>
    public class MetadataRegistration : MetadataRegistrationBase, IRegisterMetadata
    {
        /// <summary>
        /// Borrowed from System.Windows.Controls.Toolbox.Design.MetadataRegistration:
        /// use a static flag to ensure metadata is registered only one.
        /// </summary>
        private static bool _initialized;

        /// <summary>
        /// Called by tools to register design time metadata.
        /// </summary>
        public void Register()
        {
            if (!_initialized)
            {
                MetadataStore.AddAttributeTable(BuildAttributeTable());
                _initialized = true;
            }
        }

        /// <summary>
        /// Provide a place to add custom attributes without creating a AttributeTableBuilder subclass.
        /// </summary>
        /// <param name="builder">The assembly attribute table builder.</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Invalid warning")]
        protected override void AddAttributes(AttributeTableBuilder builder)
        {
            // duplicated from .Design

            builder.AddCallback(
                typeof(SSWCD.Interpolator),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.AxisLabel),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.DateTimeAxisLabel),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDCP.EdgePanel),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCD.Legend),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.NumericAxisLabel),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCD.Title),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));

            // VisualStudio.Design's own stuff

            builder.AddCallback(
                typeof(SSWCDC.AreaDataPoint),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.BarDataPoint),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.BubbleDataPoint),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.ColumnDataPoint),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.LineDataPoint),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.PieDataPoint),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCDC.ScatterDataPoint),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));

            builder.AddCallback(
                typeof(SSWCDC.LegendItem),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
        }
    }
}