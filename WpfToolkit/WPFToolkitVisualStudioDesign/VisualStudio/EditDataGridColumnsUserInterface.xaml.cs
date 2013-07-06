//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design;

namespace Microsoft.Windows.Controls.Design.VisualStudio 
{
    /// <summary>
    /// Interaction logic for AddDataGridColumnsUserInterface.xaml
    /// </summary>
    public partial class EditDataGridColumnsUserInterface : UserControl 
    {
        internal EditDataGridColumnsUserInterface() 
        {
            InitializeComponent();
        }

        internal EditDataGridColumnsUserInterface(EditingContext context, DataGridColumnModel dataGridColumnModel, DataSourcePropertyModelCollection dataSourceProperties)
            : this() 
        {
            _dataSourceProperties = dataSourceProperties;
            _dataGridColumnModel = dataGridColumnModel;
            _context = context;

            this.DataContext = dataGridColumnModel;

            bindingComboBox.ItemsSource = _dataSourceProperties;
            clipboardContentBindingComboBox.ItemsSource = _dataSourceProperties;
        }

        private void ResetPropertyValueHandler(object sender, ExecutedRoutedEventArgs e) 
        {
            string propertyName = e.Parameter as string;
            if (propertyName != null) 
            {
                if (propertyName.Equals("Binding", StringComparison.Ordinal)) 
                {
                    // special case here for ComboBoxColumn
                    if (_dataGridColumnModel.HasBindingField)
                    {
                        _dataGridColumnModel.Column.Properties["Binding"].ClearValue();
                    }
                    
                    if (_dataGridColumnModel.HasSelectedItemBindingField)
                    {
                        _dataGridColumnModel.Column.Properties["SelectedItemBinding"].ClearValue();
                    }
                }
                else
                {
                    _dataGridColumnModel.Column.Properties[propertyName].ClearValue();
                }
            }
        }

        public static RoutedCommand ResetPropertyValue = new RoutedCommand();
        private DataGridColumnModel _dataGridColumnModel;
        private EditingContext _context;
        private DataSourcePropertyModelCollection _dataSourceProperties;
    }

    /// <summary>
    ///     Convert from Visibility to a CheckBox value and back
    /// </summary>
    public class VisibilityToBooleanConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
             return (value is Visibility) && (((Visibility) value) == Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }

            return flag ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
