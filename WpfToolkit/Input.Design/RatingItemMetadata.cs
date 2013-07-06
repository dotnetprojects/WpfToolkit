// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using SSWC = Silverlight::System.Windows.Controls;

namespace System.Windows.Controls.Layout.Design
{
    /// <summary>
    /// To register design time metadata for RatingItem.
    /// </summary>
    internal class RatingItemMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for RatingItem.
        /// </summary>
        public RatingItemMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWC.RatingItem),
                b =>
                {
#if MWD40
                    b.AddCustomAttributes(new FeatureAttribute(typeof(EmptyDefaultInitializer)));
#endif
                });
        }
    }
}
