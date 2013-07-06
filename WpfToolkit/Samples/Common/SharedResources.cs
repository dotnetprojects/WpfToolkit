// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Utility to to load shared resources into another ResourceDictionary.
    /// </summary>
    public static class SharedResources
    {
        /// <summary>
        /// Prefix of images loaded as resources.
        /// </summary>
        private const string ResourceImagePrefix = "System.Windows.Controls.Samples.Images.";

        /// <summary>
        /// Prefix of icons loaded as resources.
        /// </summary>
        private const string ResourceIconPrefix = "System.Windows.Controls.Samples.Icons.";

        /// <summary>
        /// Get an embedded resource image from the assembly.
        /// </summary>
        /// <param name="name">Name of the image resource.</param>
        /// <returns>
        /// Desired embedded resource image from the assembly.
        /// </returns>
        public static Image GetImage(string name)
        {
            return CreateImage(ResourceImagePrefix, name);
        }

        /// <summary>
        /// Get an embedded resource icon from the assembly.
        /// </summary>
        /// <param name="name">Name of the icon resource.</param>
        /// <returns>
        /// Desired embedded resource icon from the assembly.
        /// </returns>
        public static Image GetIcon(string name)
        {
            return CreateImage(ResourceIconPrefix, name);
        }

        /// <summary>
        /// A cached dictionary of the bitmap images.
        /// </summary>
        private static IDictionary<string, BitmapImage> cachedBitmapImages = new Dictionary<string, BitmapImage>();

        /// <summary>
        /// Get an embedded resource image from the assembly.
        /// </summary>
        /// <param name="prefix">The prefix of the full name of the resource.</param>
        /// <param name="name">Name of the image resource.</param>
        /// <returns>
        /// Desired embedded resource image from the assembly.
        /// </returns>
        public static Image CreateImage(string prefix, string name)
        {
            Image image = new Image { Tag = name };

            BitmapImage source = null;
            string resourceName = prefix + name;
            if (!cachedBitmapImages.TryGetValue(resourceName, out source))
            {
                Assembly assembly = typeof(SharedResources).Assembly;

                using (Stream resource = assembly.GetManifestResourceStream(resourceName))
                {
                    if (resource != null)
                    {
                        source = new BitmapImage();
#if SILVERLIGHT
                        source.SetSource(resource);
#else
                        source.StreamSource = resource;
#endif
                    }
                }
                cachedBitmapImages[resourceName] = source;
            }
            image.Source = source;
            return image;
        }

        /// <summary>
        /// Get all of the names of embedded resources images in the assembly.
        /// </summary>
        /// <returns>
        /// All of the names of embedded resources images in the assembly.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does more work than a property should.")]
        public static IEnumerable<string> GetImageNames()
        {
            return GetResourceNames(ResourceImagePrefix);
        }

        /// <summary>
        /// Get all of the names of embedded resources icons in the assembly.
        /// </summary>
        /// <returns>
        /// All of the names of embedded resources icons in the assembly.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does more work than a property should.")]
        public static IEnumerable<string> GetIconNames()
        {
            return GetResourceNames(ResourceIconPrefix);
        }

        /// <summary>
        /// Get all of the images in the assembly.
        /// </summary>
        /// <returns>All of the images in the assembly.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does more work than a property should")]
        public static IEnumerable<Image> GetImages()
        {
            foreach (string name in GetImageNames())
            {
                yield return GetImage(name);
            }
        }

        /// <summary>
        /// Get all of the icons in the assembly.
        /// </summary>
        /// <returns>All of the icons in the assembly.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does more work than a property should")]
        public static IEnumerable<Image> GetIcons()
        {
            foreach (string name in GetIconNames())
            {
                yield return GetImage(name);
            }
        }

        /// <summary>
        /// Get all the names of embedded resources in the assembly with the 
        /// provided prefix value.
        /// </summary>
        /// <param name="prefix">The prefix for the full resource name.</param>
        /// <returns>Returns an enumerable of all the resource names that match.</returns>
        private static IEnumerable<string> GetResourceNames(string prefix)
        {
            Assembly assembly = typeof(SharedResources).Assembly;
            foreach (string name in assembly.GetManifestResourceNames())
            {
                // Ignore resources that don't share the images prefix
                if (!name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Trim the prefix off of the name
                yield return name.Substring(prefix.Length, name.Length - prefix.Length);
            }
        }
    }
}