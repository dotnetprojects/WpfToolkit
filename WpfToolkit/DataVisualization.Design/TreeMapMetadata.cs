// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

extern alias Silverlight;

using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design.Metadata;
using SSWCDTM = Silverlight::System.Windows.Controls.DataVisualization.TreeMap;

namespace System.Windows.Controls.DataVisualization.Design.Controls.Design.Common
{
    /// <summary>
    /// To register design time data for TreeMap.
    /// </summary>
    internal class TreeMapMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time data for TreeMap.
        /// </summary>
        public TreeMapMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWCDTM),
                b =>
                {
                    // move TreeMap items to the DataVisualization tab.
                    // a workaround with direct string names for 
                    // INotifyPropertyChanged
                    b.AddCustomAttributes(
                        "ItemDefinition",
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDTM>(x => x.ItemDefinitionSelector),
                        new CategoryAttribute(Properties.Resources.DataVisualization));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDTM>(x => x.Interpolators),
                        new CategoryAttribute(Properties.Resources.DataVisualization));

                    // move the ItemsSource to the Common Properties tab.
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWCDTM>(x => x.ItemsSource),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                });
        }
    }
}
