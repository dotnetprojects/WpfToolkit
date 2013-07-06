// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using System.Windows.Controls.Design.Common;
using SSWC = Silverlight::System.Windows.Controls;
using SSWCP = Silverlight::System.Windows.Controls.Primitives;

namespace System.Windows.Controls.Input.VisualStudio.Design
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
                typeof(SSWCP.LinearClipper),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));

            // .VisualStudio.Design's own stuff
            builder.AddCallback(
                typeof(SSWC.RatingItem),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
#if SILVERLIGHT
            builder.AddCallback(
                typeof(SSWC.ButtonSpinner),
                b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
            builder.AddCallback(
              typeof(SSWC.TimePickerPopup),
              b => b.AddCustomAttributes(new ToolboxBrowsableAttribute(false)));
#endif
        }
    }
}