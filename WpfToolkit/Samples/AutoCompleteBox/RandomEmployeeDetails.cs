// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// RandomEmployeeDetails is a sample type that contains mostly random data 
    /// for use in DataGrid sample scenarios.
    /// </summary>
    public class RandomEmployeeDetails
    {
        /// <summary>
        /// A random number generator.
        /// </summary>
        private static Random RandomGenerator = new Random();

        /// <summary>
        /// Initializes a new instance of the RandomEmployeeDetails type. A 
        /// random number and bool value will be generated in the constructor.
        /// </summary>
        public RandomEmployeeDetails()
        {
            RandomNumber = RandomGenerator.Next();
            RandomTrueFalse = RandomGenerator.Next(0, 2) == 1;
        }

        /// <summary>
        /// Initializes a new instance of the RandomEmployeeDetails type.
        /// </summary>
        /// <param name="employee">An Employee object to read the Name from.</param>
        public RandomEmployeeDetails(Employee employee) : this()
        {
            Name = employee.DisplayName;
        }

        /// <summary>
        /// Gets or sets a name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a random number.
        /// </summary>
        public int RandomNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is true or false.
        /// </summary>
        public bool RandomTrueFalse { get; set; }
    }
}