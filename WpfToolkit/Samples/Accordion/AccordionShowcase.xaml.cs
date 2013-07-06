// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System.ComponentModel;

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// Two samples that completely restyles accordion.
    /// </summary>
#if SILVERLIGHT
    [Sample("Accordion Showcase", DifficultyLevel.Basic)]
#endif
    [Category("Accordion")]
    public partial class AccordionShowcase : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="AccordionShowcase"/> class.
        /// </summary>
        public AccordionShowcase()
        {
            InitializeComponent();
        }
    }
}
