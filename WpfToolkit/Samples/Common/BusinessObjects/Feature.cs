// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Represents a feature that can be installed.
    /// </summary>
    [ContentProperty("Subcomponents")]
    public class Feature : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the Feature class.
        /// </summary>
        public Feature()
        {
            Subcomponents = new Collection<Feature>();
            ShouldInstall = true;
        }

        /// <summary>
        /// Gets or sets the name of the feature.
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets the description of the feature.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a collection of sub-components that make up the feature.
        /// </summary>
        public Collection<Feature> Subcomponents { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the feature has subcomponents.
        /// </summary>
        public bool HasSubcomponents
        {
            get
            {
                return Subcomponents.Count > 0;
            }
        }

        /// <summary>
        /// Backing variable for the ShouldInstall property.
        /// </summary>
        private bool? _shouldInstall;

        /// <summary>
        /// Gets or sets whether the feature should be installed.
        /// </summary>
        public bool? ShouldInstall
        {
            get
            {
                return _shouldInstall;
            }
            set
            {
                if (value != _shouldInstall)
                {
                    _shouldInstall = value;
                    OnPropertyChanged("ShouldInstall");
                }
            }
        }

        /// <summary>
        /// Implements the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}