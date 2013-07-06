// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.Globalization;

#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// A set of simple helpers to enable the creation of more robust web 
    /// service samples. Includes a centralized place to get generic addresses 
    /// for service requests and web pages.
    /// </summary>
    public static class WebServiceHelper
    {
        /// <summary>
        /// The format of the URI for JSON requests to Bing Suggestions.
        /// </summary>
        private const string SearchSuggestionsJsonUriFormat = "http://api.bing.net/osjson.aspx?query={0}";

        /// <summary>
        /// Standard Bing Search address.
        /// </summary>
        private const string WebSearchUriFormat = "http://www.bing.com/search?q={0}";

        /// <summary>
        /// A format string for creating a link to look at airline fares online.
        /// </summary>
        private const string AirfareSearchUriFormat = "http://www.bing.com/travel/flight/flightSearch?t=r&o={0}&e={1}&d1={2}&r1={3}&p={4}&b=COACH"; 

        /// <summary>
        /// Gets a value indicating whether the document scheme allows for web 
        /// service access.
        /// </summary>
        /// <returns>Returns true when the scheme should permit web requests.</returns>
        public static bool CanMakeHttpRequests
        {
            get
            {
#if SILVERLIGHT
                if (!HtmlPage.IsEnabled)
                {
                    return false;
                }

                string scheme = HtmlPage.Document.DocumentUri.Scheme ?? string.Empty;
                return string.Compare(scheme, "http", StringComparison.OrdinalIgnoreCase) == 0;
#else
                return true;
#endif
            }
        }

        /// <summary>
        /// Creates a Uri to navigate to a web search service.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        /// <returns>Returns a new Uri instance.</returns>
        public static Uri CreateWebSearchUri(string searchText)
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, WebSearchUriFormat, HttpUtility.UrlEncode(searchText)));
        }

        /// <summary>
        /// Creates a Uri for retrieving search suggestion phrases.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        /// <returns>Returns a new Uri instance.</returns>
        public static Uri CreateWebSearchSuggestionsUri(string searchText)
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, SearchSuggestionsJsonUriFormat, HttpUtility.UrlEncode(searchText)));
        }

        /// <summary>
        /// Creates a Uri to look up flight pricing trends online using Bing
        /// Travel.
        /// </summary>
        /// <param name="departureAirport">The departure airport object.</param>
        /// <param name="arrivalAirport">The arrival airport object.</param>
        /// <param name="departure">The departure date.</param>
        /// <param name="arrival">The arrival date.</param>
        /// <param name="persons">The number of people that will be traveling.</param>
        /// <returns>Returns a new Uri object.</returns>
        public static Uri CreateAirfareSearchUri(Airport departureAirport, Airport arrivalAirport, DateTime departure, DateTime arrival, int persons)
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, AirfareSearchUriFormat, departureAirport.CodeFaa, arrivalAirport.CodeFaa, HttpUtility.UrlEncode(departure.ToShortDateString()), HttpUtility.UrlEncode(arrival.ToShortDateString()), persons.ToString(CultureInfo.InvariantCulture)));
        }
    }
}