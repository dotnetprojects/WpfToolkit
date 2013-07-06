// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using System.Reflection;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using SSWC = Silverlight::System.Windows.Controls;
using SSWCP = Silverlight::System.Windows.Controls.Primitives;

#if SILVERLIGHT
[assembly: ProvideMetadata(typeof(System.Windows.Controls.Layout.Design.MetadataRegistration))]
#endif
namespace System.Windows.Controls.Layout.Design
{
    /// <summary>
    /// MetadataRegistration class.
    /// </summary>
    public partial class MetadataRegistration : MetadataRegistrationBase,
#if SILVERLIGHT
        IProvideAttributeTable
        {
#else
        IRegisterMetadata
    {
        private bool _initialized;
#endif

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
            // plus .Layout.Design. This is to ease the move of controls from toolkit to sdk.

            Type t = typeof(SSWC.Accordion);
            AssemblyName an = t.Assembly.GetName();
            AssemblyFullName = ", " + an.FullName;
            XmlResourceName = t.Namespace + ".Layout.Design." + an.Name + ".XML";
        }

#if SILVERLIGHT
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
#else
        /// <summary>
        /// Gets the AttributeTable for design time metadata.
        /// </summary>
        public void Register()
        {
            if (!_initialized)
            {
                MetadataStore.AddAttributeTable(BuildAttributeTable());
                _initialized = true;
            }
        }
#endif

        /// <summary>
        /// Provide a place to add custom attributes without creating a AttributeTableBuilder subclass.
        /// </summary>
        /// <param name="builder">The assembly attribute table builder.</param>
        protected override void AddAttributes(AttributeTableBuilder builder)
        {
            // Note: everything added here must be duplicated in VisualStudio.Design as well!

            builder.AddCallback(
                typeof(SSWCP.AccordionButton),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCP.ExpandableContentControl),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
        }
    }
}
