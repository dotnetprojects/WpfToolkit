//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Model;

namespace Microsoft.Windows.Controls.Design.VisualStudio 
{
    /// <summary>
    ///     Register Cider specific metadata for the WPF Toolkit controls - augments base metadata
    /// </summary>
    internal class WpfToolkitVisualStudioMetadata : IRegisterMetadata 
    {
        public void Register() 
        {
            AttributeTableBuilder builder = new WpfToolkitControlsVisualStudioAttributeTableBuilder();
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }

    /// <summary>
    ///     Table of metadata for Cider for the WPF Toolkit controls 
    /// </summary>
    internal class WpfToolkitControlsVisualStudioAttributeTableBuilder : AttributeTableBuilder 
    {
        internal WpfToolkitControlsVisualStudioAttributeTableBuilder() 
        {
            AddToolboxBrowsableAttributes();
            AddCalendarAttributes();
            AddDataGridAttributes();
            AddDataGridColumnAttributes();
            AddDataGridBoundColumnAttributes();
            AddDataGridTemplateColumnAttributes();
            AddDataGridComboBoxColumnAttributes();
            AddDatePickerAttributes();
        }

        private void AddToolboxBrowsableAttributes()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();

            builder.AddCustomAttributes(typeof(DataGridCell), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridCellsPanel), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridCellsPresenter), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridColumnHeader), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridColumnHeadersPresenter), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridDetailsPresenter), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridHeaderBorder), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridRow), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridRowHeader), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DataGridRowsPresenter), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(SelectiveScrollingGrid), ToolboxBrowsableAttribute.No);
            
            builder.AddCustomAttributes(typeof(CalendarButton), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(CalendarDayButton), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(CalendarItem), ToolboxBrowsableAttribute.No);
            builder.AddCustomAttributes(typeof(DatePickerTextBox), ToolboxBrowsableAttribute.No);

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }

        private void AddCalendarAttributes() 
        {
            AddCallback(
                typeof(Calendar), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    // Hide the Style properties as VS doesn't support setting styles in the property browser
                    builder.AddCustomAttributes(Calendar.CalendarButtonStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(Calendar.CalendarDayButtonStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(Calendar.CalendarItemStyleProperty, BrowsableAttribute.No);

                    // When it is created from the toolbox
                    builder.AddCustomAttributes(new FeatureAttribute(typeof(CalendarInitializer)));
                });
            }
        
        private void AddDataGridAttributes() 
        {
            AddCallback(
                typeof(DataGrid), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    // Browsable attributes
                    builder.AddCustomAttributes(DataGrid.BindingGroupProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.CellStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.DragIndicatorStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.DropLocationIndicatorStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.ItemBindingGroupProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.ColumnHeaderStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.RowDetailsTemplateProperty, BrowsableAttribute.No); 
                    builder.AddCustomAttributes(DataGrid.RowHeaderStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.RowHeaderTemplateProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.RowStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.RowValidationErrorTemplateProperty, BrowsableAttribute.No);
                 
                    // In Cider we don't want these properties to show up in Alpha view
                    builder.AddCustomAttributes(DataGrid.AutoGenerateColumnsProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.ItemsSourceProperty, BrowsableAttribute.No);

                    // ItemsSource uses the internal WPF CustomCategoryAttribute. Because this attribute is not implemented 
                    // correctly it does not get replaced by the DataGrid CategoryAttribute added in DataGridMetadata.cs
                    // In Cider this means the ItemsSource property on DataGrid stays in the Content category rather than being 
                    // in the DataGrid category and this causes the CategoryEditor to fail to work correctly.
                    // To avoid these issues we replace it with our own custom CategoryAttribute so that both Category attributes are 
                    // for the same category. IF you derive from CategoryAttribute you need to override TypeId so that is has the same value 
                    // as Category attribute 
                    CategoryAttribute wpfContentCategoryAttribute = null;

                    // Use raw reflection here rather than TypeDescriptor so that we don't get muddled in half loaded metadata
                    // We are looking for System.Windows.CustomCategoryAttribute with a value of "Content"
                    foreach (Attribute itemSourceAttribute in typeof(DataGrid).GetProperty("ItemsSource").GetCustomAttributes(true)) 
                    {
                        CategoryAttribute categoryAttribute = itemSourceAttribute as CategoryAttribute;
                        if (categoryAttribute != null && categoryAttribute.Category.Equals("Content")) 
                        {
                            wpfContentCategoryAttribute = categoryAttribute;
                            break;
                        }
                    }

                    if (wpfContentCategoryAttribute != null) 
                    {
                        builder.AddCustomAttributes(DataGrid.ItemsSourceProperty, new WorkaroundCategoryAttribute(wpfContentCategoryAttribute, "Rows"));
                        
                        // Do the same thing for AlternationCount as it has the same problem:
                        builder.AddCustomAttributes(DataGrid.AlternationCountProperty, new WorkaroundCategoryAttribute(wpfContentCategoryAttribute, "Rows"));
                    }

                    // MenuActions
                    builder.AddCustomAttributes(new FeatureAttribute(typeof(DataGridMenuProvider)));

                    // When it is created from the toolbox
                    builder.AddCustomAttributes(new FeatureAttribute(typeof(DataGridInitializer)));
                });
        }

        private void AddDataGridColumnAttributes() 
        {
            AddCallback(
                typeof(DataGridColumn), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    // Only add the Sort Category in VS because it causes problems in Blend               
                    CategoryAttribute sortCategory = new CategoryAttribute(SR.Get(SRID.SortCategoryTitle));
                    builder.AddCustomAttributes(DataGridColumn.CanUserSortProperty, sortCategory);
                    builder.AddCustomAttributes(DataGridColumn.SortDirectionProperty, sortCategory);
                    builder.AddCustomAttributes(DataGridColumn.SortMemberPathProperty, sortCategory);

                    builder.AddCustomAttributes(DataGridColumn.CellStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.DragIndicatorStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.HeaderStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.HeaderTemplateProperty, BrowsableAttribute.No);
                    
                    builder.AddCustomAttributes(DataGridColumn.HeaderProperty, new TypeConverterAttribute(typeof(StringConverter)));
                });
        }

        private void AddDataGridBoundColumnAttributes() 
        {
            AddCallback(
                typeof(DataGridBoundColumn), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    builder.AddCustomAttributes(DataGridBoundColumn.EditingElementStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridBoundColumn.ElementStyleProperty, BrowsableAttribute.No);
                });
        }

        private void AddDataGridComboBoxColumnAttributes()
        {
            AddCallback(
                typeof(DataGridComboBoxColumn), 
                delegate(AttributeCallbackBuilder builder)
                {
                    // Only add the Selection Category in VS because it causes problems in Blend               
                    CategoryAttribute comboBoxCategory = new CategoryAttribute(SR.Get(SRID.SelectionCategoryTitle));
                    builder.AddCustomAttributes(DataGridComboBoxColumn.DisplayMemberPathProperty, comboBoxCategory);
                    builder.AddCustomAttributes(DataGridComboBoxColumn.SelectedValuePathProperty, comboBoxCategory);
                    
                    builder.AddCustomAttributes(DataGridComboBoxColumn.EditingElementStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridComboBoxColumn.ElementStyleProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridComboBoxColumn.ItemsSourceProperty, BrowsableAttribute.No);
                });
        }

        private void AddDataGridTemplateColumnAttributes() 
        {
            AddCallback(
                typeof(DataGridTemplateColumn), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    builder.AddCustomAttributes(DataGridTemplateColumn.CellEditingTemplateProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridTemplateColumn.CellTemplateProperty, BrowsableAttribute.No);
                });
        }

        private void AddDatePickerAttributes() 
        {
            AddCallback(
                typeof(DatePicker), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    // Hide the Style properties as VS doesn't support setting styles in the property browser
                    builder.AddCustomAttributes(DatePicker.CalendarStyleProperty, BrowsableAttribute.No);

                    // When it is created from the toolbox
                    builder.AddCustomAttributes(new FeatureAttribute(typeof(DatePickerInitializer)));
                });
        }
    }

    /// <summary>
    ///     See comments above. This category attribute is designed to replace the System.Windows.CustomCategoryAttribute 
    ///     with our own. It maintains the same TypeId as so that the metadata engine replaces it correctly. 
    /// </summary>
    internal class WorkaroundCategoryAttribute : CategoryAttribute 
    {
        private CategoryAttribute _baseCategoryAttribute;

        public WorkaroundCategoryAttribute(CategoryAttribute baseCategoryAttribute, string category)
            : base(category) 
        {
            _baseCategoryAttribute = baseCategoryAttribute;
        }

        public override object TypeId 
        {
            get 
            {
                return _baseCategoryAttribute.TypeId;
            }
        }
    }
}