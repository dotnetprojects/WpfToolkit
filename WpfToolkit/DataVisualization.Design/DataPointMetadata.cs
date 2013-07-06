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
    /// To register design time metadata for SSWCDC.DataPoint.
    /// </summary>
    internal class DataPointMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for SSWCDC.DataPoint.
        /// </summary>
        public DataPointMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDC.DataPoint),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.ActualDependentValue),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.ActualIndependentValue),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.DependentValue),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.DependentValueStringFormat),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.FormattedDependentValue),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.FormattedIndependentValue),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.IndependentValue),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.IndependentValueStringFormat),
                        new CategoryAttribute(Properties.Resources.DataVisualization));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.IsSelectionEnabled),
                        new BrowsableAttribute(false));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.ActualIndependentValue),
                        PropertyValueEditor.CreateEditorAttribute(typeof(TextBoxEditor)));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.DependentValue),
                        PropertyValueEditor.CreateEditorAttribute(typeof(TextBoxEditor)));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDC.DataPoint>(x => x.IndependentValue),
                        PropertyValueEditor.CreateEditorAttribute(typeof(TextBoxEditor)));

#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.DataVisualizationControlParts, false));
#endif
                });
        }
    }
}