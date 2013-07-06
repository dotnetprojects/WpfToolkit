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
using Microsoft.Windows.Controls;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Interaction;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.Services;

namespace Microsoft.Windows.Controls.Design.VisualStudio 
{
    // The DataGridMenuProvider class provides two context menu items
    // at design time. These are implemented with the MenuAction class.
    public class DataGridMenuProvider : PrimarySelectionContextMenuProvider 
    {
        private MenuAction isDatasourceSetMenuAction;
        private MenuAction generateStockColumnsMenuAction;
        private MenuAction removeColumnsMenuAction;
        private MenuAction addColumnsMenuAction;

        /// <summary>
        ///  The provider's constructor sets up the MenuAction objects and the MenuGroup which holds them.
        /// </summary>
        public DataGridMenuProvider()
        {
            // Set up the MenuGroup which holds the MenuAction items.
            MenuGroup dataOperationsGroup = new MenuGroup("DataGroup", "DataGrid");

            isDatasourceSetMenuAction = new MenuAction("You need to set ItemsSource to enable some column operations.");

            generateStockColumnsMenuAction = new MenuAction("Generate Columns");
            generateStockColumnsMenuAction.Execute += new EventHandler<MenuActionEventArgs>(GenerateStockColumnsMenuAction_Execute);

            addColumnsMenuAction = new MenuAction("Add/Edit Columns...");
            addColumnsMenuAction.Execute += new EventHandler<MenuActionEventArgs>(AddColumnsMenuAction_Execute);

            removeColumnsMenuAction = new MenuAction("Remove Columns");
            removeColumnsMenuAction.Execute += new EventHandler<MenuActionEventArgs>(RemoveColumnsMenuAction_Execute);

            dataOperationsGroup.HasDropDown = true;
            dataOperationsGroup.Items.Add(isDatasourceSetMenuAction);
            dataOperationsGroup.Items.Add(generateStockColumnsMenuAction);
            dataOperationsGroup.Items.Add(addColumnsMenuAction);
            dataOperationsGroup.Items.Add(removeColumnsMenuAction);

            this.Items.Add(dataOperationsGroup);        // Can have groups - show up as sub menus
            
            // The UpdateItemStatus event is raised immediately before 
            // the menu show, which provides the opportunity to set states.
            UpdateItemStatus += new EventHandler<MenuActionEventArgs>(DataGridMenuProvider_UpdateItemStatus);
        }

        /// <summary>
        ///     Add and configure Columns
        /// </summary>
        private void AddColumnsMenuAction_Execute(object sender, MenuActionEventArgs e) 
        {
            using (ModelEditingScope scope = e.Selection.PrimarySelection.BeginEdit("Columns Changed")) 
            {
                AddDataGridColumnsUserInterface ui = new AddDataGridColumnsUserInterface(e.Context, e.Selection.PrimarySelection);

                // Use Windows Forms to show the design time because Windows Forms knows about the VS message pump
                System.Windows.Forms.DialogResult result = DesignerDialog.ShowDesignerDialog("Add/Edit Columns", ui); 
                if (result == System.Windows.Forms.DialogResult.OK) 
                {
                    scope.Complete();
                } 
                else 
                {
                    scope.Revert();
                }
            }
        }

        /// <summary>
        ///     Add Columns using DisplayMemberBinding
        /// </summary>
        private void GenerateStockColumnsMenuAction_Execute(object sender, MenuActionEventArgs e) 
        {
            AddColumns(e.Selection.PrimarySelection, e.Context);
        }

        /// <summary>
        ///     Add Columns based on the datasource
        /// </summary>
        private void AddColumns(ModelItem selectedDataGrid, EditingContext context) 
        {
            using (ModelEditingScope scope = selectedDataGrid.BeginEdit("Generate Columns")) 
            {
                // Set databinding related properties
                DataGridHelper.SparseSetValue(selectedDataGrid.Properties[DataGrid.AutoGenerateColumnsProperty], false);

                // Get the datasource 
                object dataSource = selectedDataGrid.Properties[ItemsControl.ItemsSourceProperty].ComputedValue;
                if (dataSource != null) 
                {
                    // Does WPF expose something like ListBindingHelper?
                    PropertyDescriptorCollection dataSourceProperties = System.Windows.Forms.ListBindingHelper.GetListItemProperties(dataSource);

                    foreach (PropertyDescriptor pd in dataSourceProperties) 
                    {
                        ModelItem dataGridColumn = null;
                        
                        dataGridColumn = DataGridHelper.CreateDefaultDataGridColumn(context, pd);

                        if (dataGridColumn != null) 
                        {
                            selectedDataGrid.Properties["Columns"].Collection.Add(dataGridColumn);
                        }
                    }
                }

                scope.Complete();
            }
        }

        /// <summary>
        ///     Update the state of the menu items based on the state of the model
        /// </summary>
        private void DataGridMenuProvider_UpdateItemStatus(object sender, MenuActionEventArgs e) 
        {
            ModelItem selectedDataGrid = e.Selection.PrimarySelection;
            object dataSource = selectedDataGrid.Properties[ItemsControl.ItemsSourceProperty].ComputedValue;

            if (dataSource == null) 
            {
                isDatasourceSetMenuAction.DisplayName = "You need to set ItemsSource to enable some column operations.";
                isDatasourceSetMenuAction.Visible = true;
                generateStockColumnsMenuAction.Visible = false;
            } 
            else 
            {
                isDatasourceSetMenuAction.DisplayName = string.Empty;
                isDatasourceSetMenuAction.Visible = false;
                generateStockColumnsMenuAction.Visible = true;
            }

            if (selectedDataGrid.Properties["Columns"].Collection.Count < 1) 
            {
                removeColumnsMenuAction.Visible = false;
            } 
            else 
            {
                removeColumnsMenuAction.Visible = true;
            }
        }

        /// <summary>
        ///     Remove columns
        /// </summary>
        private void RemoveColumnsMenuAction_Execute(object sender, MenuActionEventArgs e) 
        {
            ModelItem selectedDataGrid = e.Selection.PrimarySelection;
            using (ModelEditingScope scope = selectedDataGrid.BeginEdit("Delete Grid Columns")) 
            {
                selectedDataGrid.Properties["Columns"].Collection.Clear();
                scope.Complete();
            }
        }
    }
}
