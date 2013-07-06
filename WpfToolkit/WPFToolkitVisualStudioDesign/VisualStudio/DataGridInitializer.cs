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
                // This will really cause a margin to be added.
                Size defaultsize = new Size(200, 200);
                DataGridHelper.SparseSetValue(item.Properties[FrameworkElement.WidthProperty], defaultsize.Width);
                DataGridHelper.SparseSetValue(item.Properties[FrameworkElement.HeightProperty], defaultsize.Height);
            }
        }
    }
}