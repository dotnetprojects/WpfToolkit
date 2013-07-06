// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Photograph business object used in examples.
    /// </summary>
    public sealed partial class Photograph
    {
        /// <summary>
        /// Gets the name of the Photograph.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets an Image control containing the Photograph.
        /// </summary>
        public Image Image { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Photograph class.
        /// </summary>
        /// <param name="resourceName">
        /// Name of the resource defining the photograph.
        /// </param>
        internal Photograph(string resourceName)
        {
            Name = resourceName;
            Image = SharedResources.GetImage(resourceName);
        }

        /// <summary>
        /// Overrides the string to return the name.
        /// </summary>
        /// <returns>Returns the photograph name.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Get all of the photographs defined in the assembly as embedded
        /// resources.
        /// </summary>
        /// <returns>
        /// All of the photographs defined in the assembly as embedded
        /// resources.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Doing more work than a property should")]
        public static IEnumerable<Photograph> GetPhotographs()
        {
            foreach (string resourceName in SharedResources.GetImageNames())
            {
                yield return new Photograph(resourceName);
            }
        }
    }
}