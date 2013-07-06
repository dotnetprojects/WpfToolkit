// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Windows;
#if SILVERLIGHT
using System.Json;
using System.Windows.Browser;
#else
using System.Web;
#endif
using System.Windows.Controls;
using System.ComponentModel;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// A simple auto complete search suggestions sample that connects to a 
    /// real web service.
    /// </summary>
#if SILVERLIGHT
    [Sample("Search Suggestions", DifficultyLevel.Scenario)]
#endif
    [Category("AutoCompleteBox")]
    public partial class SearchSuggestionSample : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the SearchSuggestionSample class.
        /// </summary>
        public SearchSuggestionSample()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Handles the Loaded event by initializing the control for live web 
        /// service use if the stack is available.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (WebServiceHelper.CanMakeHttpRequests)
            {
                HostingWarning.Visibility = Visibility.Collapsed;
                Go.IsEnabled = true;
                Search.IsEnabled = true;

                Search.Populating += Search_Populating;
#if SILVERLIGHT
                Action go = () => HtmlPage.Window.Navigate(WebServiceHelper.CreateWebSearchUri(Search.Text), "_blank");
#else
                Action go = () => System.Diagnostics.Process.Start(WebServiceHelper.CreateWebSearchUri(Search.Text).ToString());
#endif
                Search.KeyUp += (s, args) =>
                    {
                        if (args.Key == System.Windows.Input.Key.Enter)
                        {
                            go();
                        }
                    };
                Go.Click += (s, args) => go();
            }
        }

        /// <summary>
        /// Handle and cancel the Populating event, and kick off the web service
        /// request.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        private void Search_Populating(object sender, PopulatingEventArgs e)
        {
            AutoCompleteBox autoComplete = (AutoCompleteBox)sender;

            // Allow us to wait for the response
            e.Cancel = true;

            // Create a request for suggestion
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += OnDownloadStringCompleted;
            wc.DownloadStringAsync(WebServiceHelper.CreateWebSearchSuggestionsUri(autoComplete.SearchText), autoComplete);
        }

        /// <summary>
        /// Handle the string download completed event of WebClient.
        /// </summary>
        /// <param name="sender">The source object.</param>
        /// <param name="e">The event data.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Any failure in the Json or request parsing should not be surfaced.")]
        private void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            AutoCompleteBox autoComplete = e.UserState as AutoCompleteBox;
            if (autoComplete != null && e.Error == null && !e.Cancelled && !string.IsNullOrEmpty(e.Result))
            {
                List<string> data = new List<string>();
                try
                {
#if SILVERLIGHT
                    JsonArray result = (JsonArray)JsonArray.Parse(e.Result);
                    if (result.Count > 1)
                    {
                        string originalSearchString = result[0];
                        if (originalSearchString == autoComplete.SearchText)
                        {
                            JsonArray suggestions = (JsonArray)result[1];
                            foreach (JsonPrimitive suggestion in suggestions)
                            {
                                data.Add(suggestion);
                            }
                        }
                    }
#else
                    System.Text.RegularExpressions.Regex jsonRegEx = new System.Text.RegularExpressions.Regex(
                              "^\\[(?<SearchItem>.*),\\[(?:(?<Items>[^,\\n]+),)*(?<LastItem>" +
                              ".*)?\\]\\]$", System.Text.RegularExpressions.RegexOptions.Multiline
                                    | System.Text.RegularExpressions.RegexOptions.Compiled);
                    System.Text.RegularExpressions.Match match = jsonRegEx.Match(e.Result);
                    if (match.Groups["Items"] != null)
                    {
                        foreach (System.Text.RegularExpressions.Capture capture in match.Groups["Items"].Captures)
                        {
                            if (!String.IsNullOrEmpty(capture.Value))
                            {
                                data.Add(capture.Value.TrimStart('"').TrimEnd('"'));
                            }
                        }
                    }
                    if (match.Groups["LastItem"] != null && match.Groups["LastItem"].Captures.Count == 1 && !String.IsNullOrEmpty(match.Groups["LastItem"].Captures[0].Value))
                    {
                        data.Add(match.Groups["LastItem"].Captures[0].Value.TrimStart('"').TrimEnd('"'));
                    }
#endif

                    // Diplay the AutoCompleteBox drop down with any suggestions
                    if (data.Count > 0)
                    {
                        autoComplete.ItemsSource = data;
                        autoComplete.PopulateComplete();
                    }
                }
                catch
                {
                }
            }
        }
    }
}