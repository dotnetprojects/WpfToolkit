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
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Interaction;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.Services;

namespace Microsoft.Windows.Controls.Design.VisualStudio 
{
internal class DataGridInitializer : DefaultInitializer 
    {
        public DataGridInitializer()
            : base() 
        {
        }

        // Set any property defaults here
        public override void InitializeDefaults(ModelItem item) 
        {
            if (item != null) 
            {
                DataGridHelper.SparseSetValue(item.Properties["Width"], 200.0);
                DataGridHelper.SparseSetValue(item.Properties["Height"], 200.0);

                DataGridHelper.SparseSetValue(item.Properties["AutoGenerateColumns"], false);
            }
        }
    }
    
    internal class DatePickerInitializer : DefaultInitializer 
    {
        public DatePickerInitializer()
            : base() 
        {
        }

        // Set any property defaults here
        public override void InitializeDefaults(ModelItem item) 
        {
            if (item != null) 
            {
                DataGridHelper.SparseSetValue(item.Properties["Width"], 115.0);
                DataGridHelper.SparseSetValue(item.Properties["Height"], 25.0);
            }
        }
    }
    
    internal class CalendarInitializer : DefaultInitializer 
    {
        public CalendarInitializer()
            : base() 
        {
        }

        // Set any property defaults here
        public override void InitializeDefaults(ModelItem item) 
        {
            if (item != null) 
            {
                DataGridHelper.SparseSetValue(item.Properties["Width"], 180.0);
                DataGridHelper.SparseSetValue(item.Properties["Height"], 170.0);
            }
        }
    }
}