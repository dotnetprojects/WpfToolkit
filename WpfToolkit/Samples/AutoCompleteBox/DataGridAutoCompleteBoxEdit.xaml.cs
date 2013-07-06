// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// The DataGridAutoCompleteBoxEdit class selects a small set of data to 
    /// display in the DataGrid. The XAML file contains the custom editing 
    /// template for AutoCompleteBox.
    /// </summary>
    [Sample("DataGrid Editing", DifficultyLevel.Advanced)]
    [Category("AutoCompleteBox")]
    public partial class DataGridAutoCompleteBoxEdit : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the DataGridAutoCompleteBoxEdit type.
        /// </summary>
        public DataGridAutoCompleteBoxEdit()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Handle the Loaded event of the page. This creates a small, random 
        /// set of data to display in the grid.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            List<RandomEmployeeDetails> data = new List<RandomEmployeeDetails>();
            Random random = new Random();

            // Select up to 8 random employees
            foreach (Employee employee in 
                new SampleEmployeeCollection()
                    .Where(item => random.Next(2) == 0)
                    .Distinct()
                    .Take(8))
            {
                data.Add(new RandomEmployeeDetails(employee));
            }
            MyDataGrid.ItemsSource = data;
        }
    }
}