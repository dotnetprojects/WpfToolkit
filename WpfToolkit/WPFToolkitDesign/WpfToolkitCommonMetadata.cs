//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;

namespace Microsoft.Windows.Controls.Design 
{
    /// <summary>
    ///     Register common metadata for for the WPF Toolkit controls
    ///     This metadata applies to Cider and Blend
    /// </summary>
    internal class WpfToolkitCommonMetadata : IRegisterMetadata 
    {
        public void Register() 
        {
            AttributeTableBuilder builder = new WpfToolkitControlsAttributeTableBuilder();
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }

    /// <summary>
    ///     Table of metadata for the WPF Toolkit controls
    /// </summary>
    internal class WpfToolkitControlsAttributeTableBuilder : AttributeTableBuilder 
    {
        internal WpfToolkitControlsAttributeTableBuilder() 
        {
            // Add Category, Description, Browsable attributes here that apply to both products
            // Add NewItemTypes attributes for the collection editor
            // Add TypeConverters as required
            AddCalendarAttributes();
            AddDataGridAttributes();
            AddDataGridColumnAttributes();
            AddDataGridBoundColumnAttributes();
            AddDataGridComboBoxColumnAttributes();
            AddDataGridTemplateColumnAttributes();
            AddDataGridHyperlinkColumnAttributes();
            AddDataGridTextColumnAttributes();
            AddDatePickerAttributes();
        }

        private void AddCalendarAttributes() 
        {
            AddCallback(
                typeof(Calendar), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    // Set the default property and event
                    builder.AddCustomAttributes(new DefaultPropertyAttribute("SelectedDate"));
                    builder.AddCustomAttributes(new DefaultEventAttribute("SelectedDatesChanged"));

                    // Add the Calendar properties to a Calendar category
                    CategoryAttribute calendarCategory = new CategoryAttribute(SR.Get(SRID.CalendarCategoryTitle));
                    builder.AddCustomAttributes("BlackoutDates", calendarCategory);
                    builder.AddCustomAttributes(Calendar.CalendarButtonStyleProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.CalendarDayButtonStyleProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.CalendarItemStyleProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.DisplayDateEndProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.DisplayDateProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.DisplayDateStartProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.DisplayModeProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.FirstDayOfWeekProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.IsTodayHighlightedProperty, calendarCategory);
                    builder.AddCustomAttributes(Calendar.SelectedDateProperty, calendarCategory);
                    builder.AddCustomAttributes("SelectedDates", calendarCategory);
                    builder.AddCustomAttributes(Calendar.SelectionModeProperty, calendarCategory);

                    // Put the Style properties in the "Advanced" part of the category
                    EditorBrowsableAttribute advanced = new EditorBrowsableAttribute(EditorBrowsableState.Advanced);
                    builder.AddCustomAttributes(Calendar.CalendarButtonStyleProperty, advanced);
                    builder.AddCustomAttributes(Calendar.CalendarDayButtonStyleProperty, advanced);
                    builder.AddCustomAttributes(Calendar.CalendarItemStyleProperty, advanced);

                    // SelectedDates and BlackoutDates conflict with each other for now hide them both
                    // to avoid the result of setting a property being an error in the designer 
                    builder.AddCustomAttributes("BlackoutDates", BrowsableAttribute.No);
                    builder.AddCustomAttributes("SelectedDates", BrowsableAttribute.No);
                });
        }

        private void AddDataGridAttributes() 
        {
            AddCallback(
                typeof(DataGrid), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    // Set the default property. The default event is inherited - SelectionChanged
                    builder.AddCustomAttributes(new DefaultPropertyAttribute("Columns"));

                    // In Blend these properties need to be browsable to be accessible from the CategoryEditor
                    builder.AddCustomAttributes(DataGrid.AutoGenerateColumnsProperty, BrowsableAttribute.Yes);
                    builder.AddCustomAttributes(DataGrid.ItemsSourceProperty, BrowsableAttribute.Yes);

                    builder.AddCustomAttributes(DataGrid.CurrentCellProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.CurrentColumnProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.CurrentItemProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.RowStyleSelectorProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes("SelectedItems", BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.RowDetailsTemplateSelectorProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGrid.RowHeaderTemplateSelectorProperty, BrowsableAttribute.No);

                    // Add Column types that can be added
                    NewItemTypesAttribute attr = new NewItemTypesAttribute(
                                                        typeof(DataGridTextColumn),
                                                        typeof(DataGridCheckBoxColumn),
                                                        typeof(DataGridHyperlinkColumn),
                                                        typeof(DataGridComboBoxColumn),
                                                        typeof(DataGridTemplateColumn));
                    attr.FactoryType = typeof(DataGridColumnFactory);
                    builder.AddCustomAttributes("Columns", attr);

                    // Enable addition of RowValidationRules
                    builder.AddCustomAttributes("RowValidationRules", new NewItemTypesAttribute(typeof(ExceptionValidationRule), typeof(DataErrorValidationRule)));

                    CategoryAttribute columnsCategory = new CategoryAttribute(SR.Get(SRID.ColumnsCategoryTitle));
                    builder.AddCustomAttributes(DataGrid.AutoGenerateColumnsProperty, columnsCategory);
                    builder.AddCustomAttributes(DataGrid.CanUserReorderColumnsProperty, columnsCategory);
                    builder.AddCustomAttributes(DataGrid.CanUserResizeColumnsProperty, columnsCategory);
                    builder.AddCustomAttributes(DataGrid.CanUserSortColumnsProperty, columnsCategory);
                    builder.AddCustomAttributes(DataGrid.CellStyleProperty, columnsCategory);
                    builder.AddCustomAttributes("Columns", columnsCategory);
                    builder.AddCustomAttributes(DataGrid.ColumnWidthProperty, columnsCategory);
                    builder.AddCustomAttributes(DataGrid.FrozenColumnCountProperty, columnsCategory);
                    builder.AddCustomAttributes(DataGrid.MaxColumnWidthProperty, columnsCategory);
                    builder.AddCustomAttributes(DataGrid.MinColumnWidthProperty, columnsCategory);

                    CategoryAttribute rowsCategory = new CategoryAttribute(SR.Get(SRID.RowsCategoryTitle));
                    builder.AddCustomAttributes(DataGrid.ItemsSourceProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.AlternationCountProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.AlternatingRowBackgroundProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.AreRowDetailsFrozenProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.CanUserAddRowsProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.CanUserDeleteRowsProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.CanUserResizeRowsProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.MinRowHeightProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.RowBackgroundProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.RowDetailsTemplateProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.RowDetailsVisibilityModeProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.RowHeightProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.RowStyleProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.RowValidationErrorTemplateProperty, rowsCategory);
                    builder.AddCustomAttributes("RowValidationRules", rowsCategory);
                    builder.AddCustomAttributes(DataGrid.SelectionModeProperty, rowsCategory);
                    builder.AddCustomAttributes(DataGrid.SelectionUnitProperty, rowsCategory);

                    CategoryAttribute headersCategory = new CategoryAttribute(SR.Get(SRID.HeadersCategoryTitle));
                    builder.AddCustomAttributes(DataGrid.ColumnHeaderHeightProperty, headersCategory);
                    builder.AddCustomAttributes(DataGrid.ColumnHeaderStyleProperty, headersCategory);
                    builder.AddCustomAttributes(DataGrid.HeadersVisibilityProperty, headersCategory);
                    builder.AddCustomAttributes(DataGrid.RowHeaderTemplateProperty, headersCategory);
                    builder.AddCustomAttributes(DataGrid.RowHeaderStyleProperty, headersCategory);
                    builder.AddCustomAttributes(DataGrid.RowHeaderWidthProperty, headersCategory);

                    CategoryAttribute gridLinesCategory = new CategoryAttribute(SR.Get(SRID.GridLinesCategoryTitle));
                    builder.AddCustomAttributes(DataGrid.GridLinesVisibilityProperty, gridLinesCategory);
                    builder.AddCustomAttributes(DataGrid.HorizontalGridLinesBrushProperty, gridLinesCategory);
                    builder.AddCustomAttributes(DataGrid.VerticalGridLinesBrushProperty, gridLinesCategory);
                });
        }

        private void AddDataGridColumnAttributes() 
        {
            AddCallback(
                typeof(DataGridColumn), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    builder.AddCustomAttributes("ActualWidth", BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.HeaderTemplateSelectorProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.IsAutoGeneratedProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.IsFrozenProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.IsFrozenProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.HeaderTemplateSelectorProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridColumn.DisplayIndexProperty, BrowsableAttribute.No);

                    // Fix the serialization of ClipboardContentBinding when it is null
                    builder.AddCustomAttributes("ClipboardContentBinding", new DefaultValueAttribute(null));
                    builder.AddCustomAttributes("ClipboardContentBinding", BrowsableAttribute.No);

                    // builder.AddCustomAttributes(DataGridColumn.HeaderProperty, new TypeConverterAttribute(typeof(StringConverter)));
                    builder.AddCustomAttributes(DataGridColumn.CanUserResizeProperty, CategoryAttribute.Layout);
                    builder.AddCustomAttributes(DataGridColumn.MaxWidthProperty, CategoryAttribute.Layout);
                    builder.AddCustomAttributes(DataGridColumn.MaxWidthProperty, new EditorBrowsableAttribute(EditorBrowsableState.Always));
                    builder.AddCustomAttributes(DataGridColumn.MinWidthProperty, CategoryAttribute.Layout);
                    builder.AddCustomAttributes(DataGridColumn.MinWidthProperty, new EditorBrowsableAttribute(EditorBrowsableState.Always));
                    builder.AddCustomAttributes(DataGridColumn.WidthProperty, CategoryAttribute.Layout);

                    // Note In Blend for some reason I don't understand these properties end up in the "Sort" category.
                    // This looks like a bug in Blend
                    // So for Blend leave the sort properties in "Misc" and then these in Header - Sort category moved to 
                    // VS metadata
                    CategoryAttribute headerCategory = new CategoryAttribute(SR.Get(SRID.HeaderCategoryTitle));
                    builder.AddCustomAttributes(DataGridColumn.HeaderProperty, headerCategory);
                    builder.AddCustomAttributes(DataGridColumn.HeaderStringFormatProperty, headerCategory);
                    builder.AddCustomAttributes(DataGridColumn.HeaderStyleProperty, headerCategory);
                    builder.AddCustomAttributes(DataGridColumn.HeaderTemplateProperty, headerCategory);

                    builder.AddCustomAttributes(DataGridColumn.VisibilityProperty, CategoryAttribute.Appearance);
                });
        }

        private void AddDataGridBoundColumnAttributes() 
        {
            // Fix the serialization of Binding when it is null
            AddCallback(
                typeof(DataGridBoundColumn), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    builder.AddCustomAttributes("Binding", new DefaultValueAttribute(null));
                    builder.AddCustomAttributes("Binding", BrowsableAttribute.No);
                });
        }

        private void AddDataGridHyperlinkColumnAttributes()
        {
            // Fix the serialization of Binding when it is null
            AddCallback(
                typeof(DataGridHyperlinkColumn), 
                delegate(AttributeCallbackBuilder builder)
                {
                    builder.AddCustomAttributes("ContentBinding", new DefaultValueAttribute(null));
                    builder.AddCustomAttributes("ContentBinding", BrowsableAttribute.No);
                });
        }

        private void AddDataGridComboBoxColumnAttributes() 
        {
            // Fix the serialization of Binding when it is null
            AddCallback(
                typeof(DataGridComboBoxColumn), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    builder.AddCustomAttributes("SelectedValueBinding", new DefaultValueAttribute(null));
                    builder.AddCustomAttributes("SelectedValueBinding", BrowsableAttribute.No);

                    builder.AddCustomAttributes("SelectedItemBinding", new DefaultValueAttribute(null));
                    builder.AddCustomAttributes("SelectedItemBinding", BrowsableAttribute.No);

                    builder.AddCustomAttributes("TextBinding", new DefaultValueAttribute(null));
                    builder.AddCustomAttributes("TextBinding", BrowsableAttribute.No);
                });
        }

        private void AddDataGridTextColumnAttributes()
        {
            AddCallback(
                typeof(DataGridTextColumn), 
                delegate(AttributeCallbackBuilder builder)
                {
                    CategoryAttribute textCategory = new CategoryAttribute(SR.Get(SRID.TextCategoryTitle));
                    builder.AddCustomAttributes(DataGridTextColumn.FontStyleProperty, textCategory);
                    builder.AddCustomAttributes(DataGridTextColumn.FontFamilyProperty, textCategory);
                    builder.AddCustomAttributes(DataGridTextColumn.FontSizeProperty, textCategory);
                    builder.AddCustomAttributes(DataGridTextColumn.FontWeightProperty, textCategory);
                    builder.AddCustomAttributes(DataGridTextColumn.ForegroundProperty, textCategory);
                });
        }

        private void AddDataGridTemplateColumnAttributes() 
        {
            AddCallback(
                typeof(DataGridTemplateColumn), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    builder.AddCustomAttributes(DataGridTemplateColumn.CellEditingTemplateSelectorProperty, BrowsableAttribute.No);
                    builder.AddCustomAttributes(DataGridTemplateColumn.CellTemplateSelectorProperty, BrowsableAttribute.No);
                });
        }

        private void AddDatePickerAttributes() 
        {
            AddCallback(
                typeof(DatePicker), 
                delegate(AttributeCallbackBuilder builder) 
                {
                    // Set the default property and event
                    builder.AddCustomAttributes(new DefaultPropertyAttribute("SelectedDate"));
                    builder.AddCustomAttributes(new DefaultEventAttribute("SelectedDateChanged"));

                    // Add the Calendar properties to a Calendar category
                    CategoryAttribute datePickerCategory = new CategoryAttribute(SR.Get(SRID.DatePickerCategoryTitle));
                    builder.AddCustomAttributes("BlackoutDates", datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.CalendarStyleProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.DisplayDateEndProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.DisplayDateProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.DisplayDateStartProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.FirstDayOfWeekProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.IsTodayHighlightedProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.SelectedDateProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.SelectedDateFormatProperty, datePickerCategory);
                    builder.AddCustomAttributes(DatePicker.TextProperty, datePickerCategory);

                    // Put the Style properties in the "Advanced" part of the category
                    EditorBrowsableAttribute advanced = new EditorBrowsableAttribute(EditorBrowsableState.Advanced);
                    builder.AddCustomAttributes(DatePicker.CalendarStyleProperty, advanced);

                    builder.AddCustomAttributes(DatePicker.IsDropDownOpenProperty, BrowsableAttribute.No);

                    // SelectedDate and BlackoutDates conflict with each other so hide BlackoutDates for now
                    // to avoid the result of setting a property being an error in the designer 
                    builder.AddCustomAttributes("BlackoutDates", BrowsableAttribute.No);
                });
        }
    }
}