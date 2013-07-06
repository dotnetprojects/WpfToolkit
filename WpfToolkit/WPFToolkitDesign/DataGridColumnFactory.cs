//---------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
//---------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;

namespace Microsoft.Windows.Controls.Design 
{
    /// <summary>
    ///     A Factory for creating Column types. Allows customization of the column type 
    ///     on creation in the collection editor. You can customization creation, display name and the image to 
    ///     show in the collection editor
    /// </summary>
    internal class DataGridColumnFactory : NewItemFactory 
    {
        public override object CreateInstance(Type type) 
        {
            DataGridColumn gridColumn = null;

            if (type.IsAssignableFrom(typeof(DataGridTemplateColumn)))
            { 
                gridColumn = CreateTemplateColumn();
            } 
            else 
            {
                gridColumn = Activator.CreateInstance(type) as DataGridColumn;
            }

            if (gridColumn != null) 
            {
                gridColumn.Header = "Header";
            }

            return gridColumn;
        }

        /// <summary>
        ///     Create a Template column with a default cell and editing template 
        /// </summary>
        private static DataGridTemplateColumn CreateTemplateColumn() 
        {
            DataGridTemplateColumn gridColumn = new DataGridTemplateColumn();
            gridColumn.CellTemplate = new DataTemplate();
            gridColumn.CellEditingTemplate = new DataTemplate();

            return gridColumn;
        }

        public override object GetImage(Type type, Size desiredSize) 
        {
            object image = base.GetImage(type, desiredSize);
            if (typeof(DataGridTextColumn).IsAssignableFrom(type)) 
            { 
                image = Util.GetImage("DataGridTextColumn.png", desiredSize);
            } 
            else if (typeof(DataGridHyperlinkColumn).IsAssignableFrom(type)) 
            {
                image = Util.GetImage("DataGridHyperlinkColumn.png", desiredSize);
            } 
            else if (typeof(DataGridComboBoxColumn).IsAssignableFrom(type)) 
            {
                image = Util.GetImage("DataGridComboBoxColumn.png", desiredSize);
            } 
            else if (typeof(DataGridCheckBoxColumn).IsAssignableFrom(type)) 
            {
                image = Util.GetImage("DataGridCheckBoxColumn.png", desiredSize);
            } 
            else if (typeof(DataGridTemplateColumn).IsAssignableFrom(type)) 
            {
                image = Util.GetImage("DataGridTemplateColumn.png", desiredSize);
            }

            return image;
        }
    }
}