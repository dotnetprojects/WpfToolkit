// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

extern alias Silverlight;
using System.Reflection;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using SSWCD = Silverlight::System.Windows.Controls.DataVisualization;
using SSWCDC = Silverlight::System.Windows.Controls.DataVisualization.Charting;
using SSWCDCP = Silverlight::System.Windows.Controls.DataVisualization.Charting.Primitives;

[assembly: ProvideMetadata(typeof(System.Windows.Controls.DataVisualization.Design.MetadataRegistration))]

namespace System.Windows.Controls.DataVisualization.Design
{
    /// <summary>
    /// MetadataRegistration class.
    /// </summary>
    public class MetadataRegistration : MetadataRegistrationBase, IProvideAttributeTable
    {
        /// <summary>
        /// Design time metadata registration class.
        /// </summary>
        public MetadataRegistration()
            : base()
        {
            // Note:
            // The default constructor sets value of AssemblyFullName and 
            // XmlResourceName used by MetadataRegistrationBase.AddDescriptions().
            // The convention here is that the <RootNamespace> in .design.csproj
            // (or Default namespace in Project -> Properties -> Application tab)
            // must be the same as runtime assembly's main namespace (t.Namespace)
            // plus .Design. This is to ease the move of controls from toolkit to sdk.

            Type t = typeof(SSWCD.Legend);
            AssemblyName an = t.Assembly.GetName();
            AssemblyFullName = ", " + an.FullName;
            XmlResourceName = t.Namespace + ".Design." + an.Name + ".XML";
        }

        /// <summary>
        /// Gets the AttributeTable for design time metadata.
        /// </summary>
        public AttributeTable AttributeTable
        {
            get
            {
                return BuildAttributeTable();
            }
        }

        /// <summary>
        /// Provide a place to add custom attributes without creating a AttributeTableBuilder subclass.
        /// </summary>
        /// <param name="builder">The assembly attribute table builder.</param>
        protected override void AddAttributes(AttributeTableBuilder builder)
        {
            // Note: everything added here must be duplicated in VisualStudio.Design as well!

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
                typeof(SSWCDC.NumericAxisLabel),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
        }
    }
}