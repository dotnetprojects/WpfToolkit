// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using SSWC = Silverlight::System.Windows.Controls;
using SSWCP = Silverlight::System.Windows.Controls.Primitives;

namespace System.Windows.Controls.Layout.VisualStudio.Design
{
    /// <summary>
    /// MetadataRegistration class.
    /// </summary>
    public partial class MetadataRegistration : MetadataRegistrationBase, IRegisterMetadata
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
        protected override void AddAttributes(AttributeTableBuilder builder)
        {
            // duplicated from .Design

            builder.AddCallback(
                typeof(SSWCP.AccordionButton), 
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWCP.ExpandableContentControl), 
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));

            // .VisualStudio.Design's own stuff

            builder.AddCallback(
                typeof(SSWC.AccordionItem), 
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
                typeof(SSWC.TransitioningContentControl),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
        }
    }
}