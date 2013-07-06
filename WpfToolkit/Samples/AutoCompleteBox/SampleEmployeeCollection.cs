// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Windows.Controls;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// A collection type that makes it easy to place sample employee data into 
    /// XAML.
    /// </summary>
    public class SampleEmployeeCollection : ObjectCollection
    {
        /// <summary>
        /// Initializes a new instance of the SampleEmployeeCollection class.
        /// </summary>
        public SampleEmployeeCollection()
            : base(Employee.AllExecutives)
        {
        }
    }
}