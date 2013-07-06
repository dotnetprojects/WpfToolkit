// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using System.ComponentModel;
using System.Windows.Controls.Design.Common;
using SSWC = Silverlight::System.Windows.Controls;

namespace System.Windows.Controls.Input.Design
{
    /// <summary>
    /// To register design time metadata for AutoCompleteBox.
    /// </summary>
    internal class AutoCompleteBoxMetadata : AttributeTableBuilder
    {
        /// <summary>
        /// To register design time metadata for AutoCompleteBox.
        /// </summary>
        public AutoCompleteBoxMetadata()
            : base()
        {
            AddCallback(
                typeof(SSWC.AutoCompleteBox),
                b =>
                {
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.TextFilter),
                        new BrowsableAttribute(false));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ItemFilter),
                        new BrowsableAttribute(false));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ValueMemberBinding),
                        new CategoryAttribute(Properties.Resources.AutoComplete));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ValueMemberPath),
                        new CategoryAttribute(Properties.Resources.AutoComplete));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.MaxDropDownHeight),
                        new CategoryAttribute(Properties.Resources.AutoComplete));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.MinimumPopulateDelay),
                        new CategoryAttribute(Properties.Resources.AutoComplete));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.MinimumPrefixLength),
                        new CategoryAttribute(Properties.Resources.AutoComplete));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.IsDropDownOpen),
                        new CategoryAttribute(Properties.Resources.AutoComplete));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.FilterMode),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.Text),
                        new CategoryAttribute(Properties.Resources.CommonProperties));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.IsTextCompletionEnabled),
                        new CategoryAttribute(Properties.Resources.CommonProperties));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ItemsSource),
                        new NewItemTypesAttribute(typeof(string)));
                    
#if MWD40
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Controls, true));

                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ItemTemplate),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ItemsSource),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ValueMemberPath),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.SelectedItem),
                            false));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.SelectedItem),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ItemsSource),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ValueMemberBinding),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.SelectedItem),
                            true));
                    b.AddCustomAttributes(
                        Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ItemContainerStyle),
                        new DataContextValueSourceAttribute(
                            Extensions.GetMemberName<SSWC.AutoCompleteBox>(x => x.ItemsSource),
                            true));
#endif
                });
        }
    }
}
