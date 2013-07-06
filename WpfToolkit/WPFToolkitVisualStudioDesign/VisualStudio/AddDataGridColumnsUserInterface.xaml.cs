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
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Model;

namespace Microsoft.Windows.Controls.Design.VisualStudio 
{
    /// <summary>
    ///     Interaction logic for AddDataGridColumnsUserInterface.xaml
    /// </summary>
    public partial class AddDataGridColumnsUserInterface : UserControl 
    {
        private ModelItem _dataGrid;
        private EditingContext _context;
        private DataGridColumnTypeModelCollection _columnTypes;
        private DataSourcePropertyModelCollection _dataSourceProperties;
        private CollectionViewSource _dataSourcePropertiesCVS;
        private DataGridColumnModelCollection _columns;

        public AddDataGridColumnsUserInterface() 
        {
            InitializeComponent();
        }

        public AddDataGridColumnsUserInterface(EditingContext context, ModelItem dataGrid) 
            : this() 
        {
            _dataGrid = dataGrid;
            _context = context;

            _columns = DataGridColumnModelCollection.Create(_dataGrid);
            _dataSourceProperties = DataSourcePropertyModelCollection.Create(_dataGrid);
            _columnTypes = DataGridColumnTypeModelCollection.Create();
            
            _dataSourcePropertiesCVS = new CollectionViewSource();
            _dataSourcePropertiesCVS.Source = _dataSourceProperties;
            _dataSourcePropertiesCVS.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _dataSourcePropertiesCVS.Filter += new FilterEventHandler(_dataSourcePropertiesCVS_Filter);
            dataSourceColumnsListBox.ItemsSource = _dataSourcePropertiesCVS.View;
            dataSourceColumnsListBox.SelectedIndex = 0;
            dataSourceColumnsListBox.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(HandleListBoxItemDoubleClick), true);

            datagridColumnsListBox.ItemsSource = _columns;
            datagridColumnsListBox.SelectedIndex = 0;
            datagridColumnsListBox.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(HandleListBoxItemDoubleClick), true);

            columnTypeComboBox.ItemsSource = _columnTypes;
            columnTypeComboBox.SelectedIndex = 0;
        }

        private void HandleListBoxItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (object.ReferenceEquals(sender, dataSourceColumnsListBox))
                {
                    AddButton_Click(null, null);
                }
                else if (object.ReferenceEquals(sender, datagridColumnsListBox))
                {
                    RemoveButton_Click(null, null);
                }
            }
        }

        /// <summary>
        ///     Only show those datasource fields that don't have bound columns
        /// </summary>
        private void _dataSourcePropertiesCVS_Filter(object sender, FilterEventArgs e) 
        {
            if (_columns != null) 
            {
                DataSourcePropertyModel prop = e.Item as DataSourcePropertyModel;
                e.Accepted = !(_columns.Includes(dataGridColumnModel => prop.Name.Equals(dataGridColumnModel.BindingPropertyName)));
            }
        }

        private void EditColumn()
        {
            DataGridColumnModel columnModel = datagridColumnsListBox.SelectedItem as DataGridColumnModel;
            if (columnModel == null)
            {
                return;
            }

            using (ModelEditingScope scope = columnModel.Column.BeginEdit(columnModel.Column.Name + " Changed"))
            {
                EditDataGridColumnsUserInterface ui = new EditDataGridColumnsUserInterface(_context, columnModel, _dataSourceProperties);

                // Use Windows Forms to show the design time because Windows Forms knows about the VS message pump
                System.Windows.Forms.DialogResult result = DesignerDialog.ShowDesignerDialog("Edit Column", ui, 360, 470); 
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    scope.Complete();
                    _dataSourcePropertiesCVS.View.Refresh();
                }
                else
                {
                    scope.Revert();
                }
            }
        }

        /// <summary>
        ///     Edit the column associated with the button that was clicked.
        /// </summary>
        private void EditColumn_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = DataGridHelper.FindParent<ListBoxItem>((Button)sender);
            if (item != null)
            {
                ListBox listBox = DataGridHelper.FindParent<ListBox>(item);
                if (listBox != null)
                {
                    listBox.SelectedItems.Clear();
                }

                item.IsSelected = true;
                EditColumn();
            }
        }

        /// <summary>
        ///     Create a DataGridColumn for the selected data source property
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumnTypeModel columnTypeModel = columnTypeComboBox.SelectedItem as DataGridColumnTypeModel;
            if (columnTypeModel == null) 
            {
                throw new Exception("unexpected state");
            }

            if (dataSourceColumnsListBox.SelectedItems.Count > 0)
            {
                int oldSelectedIndex = dataSourceColumnsListBox.SelectedIndex;
                DataSourcePropertyModel[] itemsToAdd = new DataSourcePropertyModel[dataSourceColumnsListBox.SelectedItems.Count];
                dataSourceColumnsListBox.SelectedItems.CopyTo(itemsToAdd, 0);

                foreach (DataSourcePropertyModel pm in itemsToAdd)
                {
                    ModelItem dataGridColumn = columnTypeModel.CreateDataGridColumn(_context, pm.Property);
                    _columns.Add(dataGridColumn);
                }

                DataGridHelper.SparseSetValue(_dataGrid.Properties[DataGrid.AutoGenerateColumnsProperty], false);
                _dataSourcePropertiesCVS.View.Refresh();
                dataSourceColumnsListBox.SelectedIndex = Math.Min(oldSelectedIndex, dataSourceColumnsListBox.Items.Count - 1);
            }
        }

        /// <summary>
        ///     Create DataGridColumns for all the datasoure properties that don't already have columns
        /// </summary>
        private void CreateAllButton_Click(object sender, RoutedEventArgs e) 
        {
            bool hasColumnsAlready = _columns.Count != 0;
            DataGridColumnTypeModel columnTypeModel = columnTypeComboBox.SelectedItem as DataGridColumnTypeModel;
            if (columnTypeModel == null) 
            {
                throw new Exception("unexpected state");
            }

            // Set databinding related properties
            DataGridHelper.SparseSetValue(_dataGrid.Properties[DataGrid.AutoGenerateColumnsProperty], false);

            foreach (DataSourcePropertyModel pm in _dataSourceProperties) 
            {
                bool addThisColumn = true;

                if (!hasColumnsAlready) 
                {
                    addThisColumn = true;
                } 
                else 
                {
                    addThisColumn = !(_columns.Includes(dataGridColumnModel => pm.Name.Equals(dataGridColumnModel.BindingPropertyName)));
                }

                if (addThisColumn) 
                {
                    ModelItem dataGridColumn = columnTypeModel.CreateDataGridColumn(_context, pm.Property);
                    _columns.Add(dataGridColumn);
                }
            }

            _dataSourcePropertiesCVS.View.Refresh();
        }

        /// <summary>
        ///     Remove the selected DataGridColumn
        /// </summary>
        private void RemoveButton_Click(object sender, RoutedEventArgs e) 
        {
            if (datagridColumnsListBox.SelectedItems.Count > 0)
            {
                int oldSelectedIndex = datagridColumnsListBox.SelectedIndex;
                DataGridColumnModel[] itemsToRemove = new DataGridColumnModel[datagridColumnsListBox.SelectedItems.Count];
                datagridColumnsListBox.SelectedItems.CopyTo(itemsToRemove, 0);

                foreach (DataGridColumnModel dataGridColumnModel in itemsToRemove)
                {
                    _columns.Remove(dataGridColumnModel);
                }

                datagridColumnsListBox.SelectedIndex = Math.Min(oldSelectedIndex, datagridColumnsListBox.Items.Count - 1);
                _dataSourcePropertiesCVS.View.Refresh();
            }
        }

        /// <summary>
        ///     Remove all of the DataGrid Columns
        /// </summary>
        private void NoneButton_Click(object sender, RoutedEventArgs e) 
        {
            _columns.Clear();
            _dataSourcePropertiesCVS.View.Refresh();
        }

        private void CreateUnboundButton_Click(object sender, RoutedEventArgs e) 
        {
            DataGridColumnTypeModel columnTypeModel = columnTypeComboBox.SelectedItem as DataGridColumnTypeModel;
            if (columnTypeModel == null) 
            {
                throw new Exception("unexpected state");
            }

            // Set databinding related properties
            DataGridHelper.SparseSetValue(_dataGrid.Properties[DataGrid.AutoGenerateColumnsProperty], false);

            ModelItem dataGridColumn = columnTypeModel.CreateDataGridColumn(_context);
            _columns.Add(dataGridColumn);
        }
    }

    /// <summary>
    ///     Utility classes to help out with the UI
    /// </summary>
    internal abstract class ModelBase : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    internal class DataGridColumnModel : ModelBase
    {
        private ModelItem _column;

        public DataGridColumnModel(ModelItem column)
        {
            this._column = column;
            _column.PropertyChanged += new PropertyChangedEventHandler(_column_PropertyChanged);
        }

        private void _column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ClipboardContentBinding"))
            {
                OnPropertyChanged("ClipboardContentBindingPropertyName");
            }
            else if (e.PropertyName.Equals("Binding") || e.PropertyName.Equals("SelectedItemBinding"))
            {
                OnPropertyChanged("BindingPropertyName");
                OnPropertyChanged("Name");
                OnPropertyChanged("ClipboardContentBindingPropertyName");
                OnPropertyChanged("SortMemberPath");
            }

            OnPropertyChanged(e.PropertyName);
        }

        public string ClipboardContentBindingPropertyName
        {
            get
            {
                string propertyName = string.Empty;
                Binding b = Column.Properties["ClipboardContentBinding"].ComputedValue as Binding;
                if (b != null)
                {
                    propertyName = b.Path.Path;
                }

                return propertyName;
            }

            set
            {
                Column.Properties["ClipboardContentBinding"].SetValue(new Binding(value));
            }
        }

        public string SortMemberPath
        {
            get
            {
                return Column.Properties["SortMemberPath"].ComputedValue as string;
            }

            set
            {
                Column.Properties["SortMemberPath"].ComputedValue = value;
            }
        }

        public bool CanUserReorder
        {
            get
            {
                return (bool)Column.Properties["CanUserReorder"].ComputedValue;
            }

            set
            {
                Column.Properties["CanUserReorder"].SetValue(value);
            }
        }

        public bool CanUserResize
        {
            get
            {
                return (bool)Column.Properties["CanUserResize"].ComputedValue;
            }

            set
            {
                Column.Properties["CanUserResize"].SetValue(value);
            }
        }

        public bool CanUserSort
        {
            get
            {
                return (bool)Column.Properties["CanUserSort"].ComputedValue;
            }

            set
            {
                Column.Properties["CanUserSort"].SetValue(value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)Column.Properties["IsReadOnly"].ComputedValue;
            }

            set
            {
                Column.Properties["IsReadOnly"].SetValue(value);
            }
        }

        public int DisplayIndex
        {
            get
            {
                return (int)Column.Properties["DisplayIndex"].ComputedValue;
            }

            set
            {
                Column.Properties["DisplayIndex"].SetValue(value);
            }
        }

        public ModelItem Column
        {
            get
            {
                return _column;
            }
        }

        public bool HasBindingField
        {
            get
            {
                return typeof(DataGridBoundColumn).IsAssignableFrom(Column.ItemType);
            }
        }

        public bool HasSelectedItemBindingField
        {
            get
            {
                return typeof(DataGridComboBoxColumn).IsAssignableFrom(Column.ItemType);
            }
        }

        public string Header
        {
            get
            {
                return Column.Properties["Header"].ComputedValue as string;
            }

            set
            {
                Column.Properties["Header"].SetValue(value);
            }
        }

        public double MaxWidth
        {
            get
            {
                return (double)Column.Properties["MaxWidth"].ComputedValue;
            }

            set
            {
                Column.Properties["MaxWidth"].ComputedValue = value;
            }
        }

        public double MinWidth
        {
            get
            {
                return (double)Column.Properties["MinWidth"].ComputedValue;
            }

            set
            {
                Column.Properties["MinWidth"].ComputedValue = value;
            }
        }

        public string Name
        {
            get
            {
                string name = Column.ItemType.Name;
                string propertyName = BindingPropertyName;
                if (!propertyName.Equals(string.Empty))
                {
                    name = name + " (" + propertyName + ")";
                }

                return name;
            }
        }

        public string BindingPropertyName
        {
            get
            {
                string propertyName = string.Empty;
                Binding binding = null;

                if (HasBindingField)
                {
                    binding = Column.Properties["Binding"].ComputedValue as Binding;
                }
                else if (HasSelectedItemBindingField)
                {
                    binding = Column.Properties["SelectedItemBinding"].ComputedValue as Binding;
                }

                if (binding != null)
                {
                    propertyName = binding.Path.Path;
                }

                return propertyName;
            }

            set
            {
                if (HasBindingField)
                {
                    Column.Properties["Binding"].SetValue(new Binding(value));
                }
                else if (HasSelectedItemBindingField)
                {
                    Column.Properties["SelectedItemBinding"].SetValue(new Binding(value));
                }
            }
        }

        public Visibility ShowBindingField
        {
            get
            {
                if (HasBindingField || HasSelectedItemBindingField)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public DataGridLength Width
        {
            get
            {
                return (DataGridLength)Column.Properties["Width"].ComputedValue;
            }

            set
            {
                Column.Properties["Width"].ComputedValue = value;
            }
        }

        public Visibility Visibility
        {
            get
            {
                return (Visibility)Column.Properties["Visibility"].ComputedValue;
            }

            set
            {
                Column.Properties["Visibility"].ComputedValue = value;
            }
        }

        public override string ToString()
        {
            return "Column: " + this.Name;
        }
    }

    internal class DataGridColumnModelCollection : ObservableCollection<DataGridColumnModel> 
    {
        private ModelItem _dataGrid = null;        

        internal static DataGridColumnModelCollection Create(ModelItem dataGrid) 
        {
            DataGridColumnModelCollection columns = new DataGridColumnModelCollection(dataGrid);

            columns.Initializing = true;
            try 
            {
                foreach (ModelItem dataGridColumn in dataGrid.Properties["Columns"].Collection) 
                {
                    columns.Add(dataGridColumn);
                }
            } 
            finally 
            {
                columns.Initializing = false;
            }

            return columns;
        }

        public DataGridColumnModelCollection(ModelItem dataGrid) 
        {
            _dataGrid = dataGrid;
        }

        internal bool Initializing 
        { 
            get; set; 
        }

        public void Add(ModelItem dataGridColumn) 
        {
            this.Add(new DataGridColumnModel(dataGridColumn));
        }

        public bool Includes(Func<DataGridColumnModel, bool> predicate) 
        {
            foreach (DataGridColumnModel item in this) 
            {
                if (predicate(item)) 
                {
                    return true;
                }
            }

            return false;
        }

        // Override update methods to keep the underlying model object in sync
        protected override void ClearItems() 
        {
            if (!this.Initializing) 
            {
                _dataGrid.Properties["Columns"].Collection.Clear();
            }

            base.ClearItems();
        }

        protected override void RemoveItem(int index) 
        {
            if (!this.Initializing) 
            {
                _dataGrid.Properties["Columns"].Collection.RemoveAt(index);
            }

            base.RemoveItem(index);
        }

        protected override void InsertItem(int index, DataGridColumnModel item) 
        {
            if (!this.Initializing) 
            {
                _dataGrid.Properties["Columns"].Collection.Insert(index, item.Column);
            }

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, DataGridColumnModel item) 
        {
            if (!this.Initializing)
            {
                _dataGrid.Properties["Columns"].Collection[index] = item.Column;
            }

            base.SetItem(index, item);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (!this.Initializing) 
            {
                _dataGrid.Properties["Columns"].Collection.Move(oldIndex, newIndex);
            }

            base.MoveItem(oldIndex, newIndex);
        }
    }

    internal class DataSourcePropertyModel 
    {
        public PropertyDescriptor Property 
        { 
            get; set; 
        }

        public string Name 
        { 
            get; set; 
        }

        public override string ToString() 
        {
            return "Property: " + this.Name;
        }
    }

    internal class DataSourcePropertyModelCollection : ObservableCollection<DataSourcePropertyModel> 
    {
        internal static DataSourcePropertyModelCollection Create(ModelItem dataGrid) 
        {
            DataSourcePropertyModelCollection properties = new DataSourcePropertyModelCollection();

            object dataSource = dataGrid.Properties[ItemsControl.ItemsSourceProperty].ComputedValue;
            if (dataSource != null) 
            {
                foreach (PropertyDescriptor pd in System.Windows.Forms.ListBindingHelper.GetListItemProperties(dataSource)) 
                {
                    properties.Add(new DataSourcePropertyModel { Name = pd.Name, Property = pd });
                }
            }

            return properties;
        }
    }

    internal class DataGridColumnTypeModel 
    {
        protected DataGridColumnTypeModel() 
        {
        }

        public DataGridColumnTypeModel(Type columnType, string columnName)
        {
            this.ColumnType = columnType;
            this.Name = columnName;
        }

        public Type ColumnType 
        { 
            get; set; 
        }

        public string Name 
        { 
            get; set; 
        }

        public override string ToString() 
        {
            return this.Name;
        }

        public virtual ModelItem CreateDataGridColumn(EditingContext context) 
        {
            return DataGridHelper.CreateUnboundDataGridColumn(context, ColumnType);
        }

        public virtual ModelItem CreateDataGridColumn(EditingContext context, PropertyDescriptor pd) 
        {
            return DataGridHelper.CreateDataGridColumn(context, ColumnType, pd);
        }
    }

    internal class DefaultDataGridColumnTypeModel : DataGridColumnTypeModel 
    {
        public DefaultDataGridColumnTypeModel() 
        {
            this.Name = "Default";
        }

        public override ModelItem CreateDataGridColumn(EditingContext context) 
        {
            return DataGridHelper.CreateUnboundDataGridColumn(context, typeof(DataGridTextColumn));
        }

        public override ModelItem CreateDataGridColumn(EditingContext context, PropertyDescriptor pd) 
        {
            return DataGridHelper.CreateDefaultDataGridColumn(context, pd);
        }
    }
    
    internal class DataGridColumnTypeModelForDataGridTemplateColumn : DataGridColumnTypeModel 
    {
        public DataGridColumnTypeModelForDataGridTemplateColumn() 
            : base(typeof(DataGridTemplateColumn), "Template") 
        { 
        }

        public override ModelItem CreateDataGridColumn(EditingContext context) 
        {
            return DataGridHelper.CreateUnboundDataGridTemplateColumn(context);
        }

        public override ModelItem CreateDataGridColumn(EditingContext context, PropertyDescriptor pd) 
        {
            return DataGridHelper.CreateBoundDataGridTemplateColumn(context, pd);
        }
    }

    internal class DataGridColumnTypeModelCollection : ObservableCollection<DataGridColumnTypeModel> 
    {
        internal static DataGridColumnTypeModelCollection Create() 
        {
            DataGridColumnTypeModelCollection columnTypes = new DataGridColumnTypeModelCollection();
            columnTypes.Add(new DefaultDataGridColumnTypeModel());
            columnTypes.Add(new DataGridColumnTypeModel(typeof(DataGridTextColumn), "Text"));
            columnTypes.Add(new DataGridColumnTypeModel(typeof(DataGridCheckBoxColumn), "CheckBox"));
            columnTypes.Add(new DataGridColumnTypeModel(typeof(DataGridHyperlinkColumn), "Hyperlink"));
            columnTypes.Add(new DataGridColumnTypeModel(typeof(DataGridComboBoxColumn), "ComboBox"));
            columnTypes.Add(new DataGridColumnTypeModelForDataGridTemplateColumn());
            return columnTypes;
        }
    }
}
