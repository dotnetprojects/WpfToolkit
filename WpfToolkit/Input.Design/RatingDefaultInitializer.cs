// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

extern alias Silverlight;
using System.Windows.Controls.Design.Common;
using Microsoft.Windows.Design.Model;
using SSW = Silverlight::System.Windows;
using SSWC = Silverlight::System.Windows.Controls;

namespace System.Windows.Controls.Input.Design
{
    /// <summary>
    /// Default initializer for Rating. 
    /// </summary>
    internal class RatingDefaultInitializer : DefaultInitializer
    {
        /// <summary>
        /// Sets the default property values for Rating. 
        /// </summary>
        /// <param name="item">SSWCDC.Rating ModelItem.</param>
        public override void InitializeDefaults(ModelItem item)
        {
            string propertyName;

            // <inputToolkit:Rating ItemCount="5">
            propertyName = Extensions.GetMemberName<SSWC.Rating>(x => x.ItemCount);
            item.Properties[propertyName].SetValue(5);
        }
    }
}