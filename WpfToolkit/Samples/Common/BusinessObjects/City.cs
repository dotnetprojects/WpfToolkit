// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// City business object used for charting samples.
    /// </summary>
    public class City
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the population of the city.
        /// </summary>
        public int Population { get; set; }

        /// <summary>
        /// Initializes a new instance of the City class.
        /// </summary>
        public City()
        {
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets a collection of cities in the Puget Sound area.
        /// </summary>
        public static ObjectCollection PugetSound
        {
            get
            {
                ObjectCollection pugetSound = new ObjectCollection();
                pugetSound.Add(new City { Name = "Bellevue", Population = 112344 });
                pugetSound.Add(new City { Name = "Issaquah", Population = 11212 });
                pugetSound.Add(new City { Name = "Redmond", Population = 46391 });
                pugetSound.Add(new City { Name = "Seattle", Population = 592800 });
                return pugetSound;
            }
        }
    }
}