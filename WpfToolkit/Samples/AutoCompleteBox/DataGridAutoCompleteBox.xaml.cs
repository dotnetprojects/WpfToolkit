// (c) Copyright Microsoft Corporation.
// This source is subject to [###LICENSE_NAME###].
// Please see [###LICENSE_LINK###] for details.
// All other rights reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// A sample AutoCompleteBox with a DataGrid selection adapter.
    /// </summary>
    [Sample("DataGrid", DifficultyLevel.Advanced)]
    [Category("AutoCompleteBox")]
    public partial class DataGridAutoCompleteBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the type.
        /// </summary>
        public DataGridAutoCompleteBox()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Handle the loaded event.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Bind to the sample airport data
            MyAutoCompleteBox.ItemsSource = Airport.SampleAirports;
            
            // A custom search, the same that is used in the basic lambda file
            MyAutoCompleteBox.FilterMode = AutoCompleteFilterMode.Custom;
            MyAutoCompleteBox.ItemFilter = (search, item) =>
            {
                Airport airport = item as Airport;
                if (airport != null)
                {
                    // Interested in: Name, City, FAA code
                    string filter = search.ToUpper(CultureInfo.InvariantCulture);
                    return (airport.CodeFaa.ToUpper(CultureInfo.InvariantCulture).Contains(filter)
                        || airport.City.ToUpper(CultureInfo.InvariantCulture).Contains(filter)
                        || airport.Name.ToUpper(CultureInfo.InvariantCulture).Contains(filter));
                }

                return false;
            };
        }
    }
}