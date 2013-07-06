//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Interaction;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.Services;

namespace Microsoft.Windows.Controls.Design.VisualStudio 
{
    internal static class DataGridHelper 
    {
        internal static ModelItem CreateDefaultDataGridColumn(EditingContext context, PropertyDescriptor pd) 
        {
            Type dataGridColumnType = typeof(DataGridTextColumn);

            if (pd.PropertyType == typeof(bool)) 
            {
                dataGridColumnType = typeof(DataGridCheckBoxColumn);
            } 
            else if (pd.PropertyType == typeof(Uri)) 
            {
                dataGridColumnType = typeof(DataGridHyperlinkColumn);
            } 
            else if (pd.PropertyType.IsEnum) 
            {
                dataGridColumnType = typeof(DataGridComboBoxColumn);
            }

            return CreateDataGridColumn(context, dataGridColumnType, pd);
        }

        internal static ModelItem CreateDataGridColumn(EditingContext context, Type dataGridColumnType, PropertyDescriptor pd) 
        {
            ModelItem dataGridColumn = ModelFactory.CreateItem(context, dataGridColumnType);
            dataGridColumn.Properties[DataGridColumn.HeaderProperty].SetValue(pd.Name);

            Binding binding = new Binding(pd.Name);
            if (pd.IsReadOnly)
            {
                binding.Mode = BindingMode.OneWay;
                dataGridColumn.Properties["IsReadOnly"].SetValue(true);
            }

            // special case ComboBox because it's not a Bound column.
            if (dataGridColumnType == typeof(DataGridComboBoxColumn))
            {
                dataGridColumn.Properties["SelectedItemBinding"].SetValue(binding);
            }
            else
            {
                dataGridColumn.Properties["Binding"].SetValue(binding);
            }

            if (!typeof(IComparable).IsAssignableFrom(pd.PropertyType))
            {
                dataGridColumn.Properties[DataGridColumn.CanUserSortProperty].SetValue(false);
            }

            return dataGridColumn;
        }
        
        internal static ModelItem CreateUnboundDataGridColumn(EditingContext context, Type dataGridColumnType) 
        {
            ModelItem dataGridColumn = ModelFactory.CreateItem(context, dataGridColumnType);
            dataGridColumn.Properties[DataGridColumn.HeaderProperty].SetValue(dataGridColumnType.Name);
            return dataGridColumn;
        }

        internal static ModelItem CreateBoundDataGridTemplateColumn(EditingContext context, PropertyDescriptor pd) 
        {
            ModelItem dataGridColumn = null;
            dataGridColumn = ModelFactory.CreateItem(context, typeof(DataGridTemplateColumn));
            dataGridColumn.Properties[DataGridColumn.HeaderProperty].SetValue(pd.Name);

            ModelItem textBlock = ModelFactory.CreateItem(context, typeof(TextBlock));
            textBlock.Properties[TextBlock.TextProperty].SetValue(new Binding(pd.Name));
            ModelItem displayDataTemplate = CreateDataTemplate(context, dataGridColumn, textBlock);
            dataGridColumn.Properties[DataGridTemplateColumn.CellTemplateProperty].SetValue(displayDataTemplate);

            ModelItem textBox = ModelFactory.CreateItem(context, typeof(TextBox));
            textBox.Properties[TextBox.TextProperty].SetValue(new Binding(pd.Name));
            ModelItem editingDataTemplate = CreateDataTemplate(context, dataGridColumn, textBox);
            dataGridColumn.Properties[DataGridTemplateColumn.CellEditingTemplateProperty].SetValue(editingDataTemplate);

            dataGridColumn.Properties["ClipboardContentBinding"].SetValue(new Binding(pd.Name));

            return dataGridColumn;
        }

        internal static ModelItem CreateUnboundDataGridTemplateColumn(EditingContext context) 
        {
            ModelItem dataGridColumn = null;
            dataGridColumn = ModelFactory.CreateItem(context, typeof(DataGridTemplateColumn));
            dataGridColumn.Properties[DataGridColumn.HeaderProperty].SetValue("Column");

            ModelItem textBlock = ModelFactory.CreateItem(context, typeof(TextBlock));
            textBlock.Properties[TextBlock.TextProperty].SetValue("PlaceHolder");
            ModelItem displayDataTemplate = CreateDataTemplate(context, dataGridColumn, textBlock);
            dataGridColumn.Properties[DataGridTemplateColumn.CellTemplateProperty].SetValue(displayDataTemplate);

            ModelItem textBox = ModelFactory.CreateItem(context, typeof(TextBox));
            textBox.Properties[TextBox.TextProperty].SetValue("PlaceHolder");
            ModelItem editingDataTemplate = CreateDataTemplate(context, dataGridColumn, textBox);
            dataGridColumn.Properties[DataGridTemplateColumn.CellEditingTemplateProperty].SetValue(editingDataTemplate);
            return dataGridColumn;
        }

        // To generate a Data Template, do the following:
        // 1) Serialize the ModelItem of the control to string
        // 2) Add DataTemplate tag around the string
        // 3) Deserialize the string to a DataTemplate
        // Note: This method is currently a workaround.
        internal static ModelItem CreateDataTemplate(EditingContext context, ModelItem parent, ModelItem controlToTemplate) 
        {
            ExternalMarkupService ems = context.Services.GetService<ExternalMarkupService>();

            string s = ems.Save(controlToTemplate);
            s = "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" + System.Environment.NewLine + 
                    s + System.Environment.NewLine + 
                    "</DataTemplate>";
            ModelItem dt = ems.Load(s, parent, null);

            return dt;
        }

        internal static void SparseSetValue(ModelProperty property, object value)
        {
            object defaultValue = property.DefaultValue;
            if (object.Equals(defaultValue, value))
            {
                property.ClearValue();
            }
            else
            {
                property.SetValue(value);
            }
        }

        // Walks up the visual tree to find a parent of the given type
        internal static T FindParent<T>(Visual child) where T : Visual
        {
            DependencyObject current = VisualTreeHelper.GetParent(child);
            while (current != null)
            {
                T parent = current as T;
                if (parent != null)
                {
                    return parent;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }
    }
}
