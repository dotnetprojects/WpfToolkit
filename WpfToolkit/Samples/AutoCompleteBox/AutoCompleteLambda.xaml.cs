// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Globalization;
using System.Windows;
#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif
using System.Windows.Controls;
using System.ComponentModel;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// The AutoCompleteLambda sample page demonstrates using a custom data 
    /// template, binding to elements in a rich CLR type, and using a lambda 
    /// expression to provide custom search filtering capabilities to the 
    /// AutoCompleteBox control.
    /// </summary>
#if SILVERLIGHT
    [Sample("(1)ItemFilter lambda", DifficultyLevel.Basic)]
#endif
    [Category("AutoCompleteBox")]
    public partial class AutoCompleteLambda : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the AutoCompleteLambda class.
        /// </summary>
        public AutoCompleteLambda()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Airports_Loaded);
        }

        /// <summary>
        /// Handle the Loaded event of the page.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event arguments.</param>
        private void Airports_Loaded(object sender, RoutedEventArgs e)
        {
            // Provide airport data and a custom filter
            ObjectCollection airports = Airport.SampleAirports;
            DepartureAirport.ItemsSource = airports;
            ArrivalAirport.ItemsSource = airports;
            DepartureAirport.ItemFilter = (search, item) =>
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
            ArrivalAirport.ItemFilter = DepartureAirport.ItemFilter;

            // Look for changes
            DepartureAirport.SelectionChanged += SelectedItemsChanged;
            ArrivalAirport.SelectionChanged += SelectedItemsChanged;
            DepartureDate.SelectedDateChanged += SelectedItemsChanged;
            ArrivalDate.SelectedDateChanged += SelectedItemsChanged;

            // Set the date defaults in code
            DepartureDate.SelectedDate = DateTime.UtcNow + TimeSpan.FromDays(7);
            ArrivalDate.SelectedDate = DateTime.UtcNow + TimeSpan.FromDays(14);

            // Navigate to Bing Travel
            BookFlight.Click += BookFlight_Click;
        }

        /// <summary>
        /// The button to book the flight has been clicked.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void BookFlight_Click(object sender, RoutedEventArgs e)
        {
            Airport d = (Airport)DepartureAirport.SelectedItem;
            Airport a = (Airport)ArrivalAirport.SelectedItem;
            DateTime dd = (DateTime)DepartureDate.SelectedDate;
            DateTime ad = (DateTime)ArrivalDate.SelectedDate;
            int p = (int)Passengers.Value;

            Uri travel = WebServiceHelper.CreateAirfareSearchUri(d, a, dd, ad, p);
#if SILVERLIGHT
            HtmlPage.Window.Navigate(travel, "_new");
#else
            System.Diagnostics.Process.Start(travel.ToString());
#endif
        }

        /// <summary>
        /// Update the form when valid values are present in the controls.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void SelectedItemsChanged(object sender, SelectionChangedEventArgs e)
        {
            BookFlight.IsEnabled = DepartureAirport.SelectedItem != null
                && ArrivalAirport.SelectedItem != null
                && DepartureDate.SelectedDate != null
                && ArrivalDate.SelectedDate != null
                && Passengers.Value > 0;
        }
    }
}
