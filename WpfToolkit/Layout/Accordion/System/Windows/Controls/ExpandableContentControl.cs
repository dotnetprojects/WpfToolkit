// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

namespace System.Windows.Controls.Primitives
{
    public class ExpandableContentControl : ContentControl
    {
        static ExpandableContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpandableContentControl),
                new FrameworkPropertyMetadata(typeof(ExpandableContentControl)));
            ClipToBoundsProperty.OverrideMetadata(typeof(ExpandableContentControl), new FrameworkPropertyMetadata(true));
            FocusableProperty.OverrideMetadata(typeof(ExpandableContentControl), new FrameworkPropertyMetadata(false));
        }

        #region public ExpandDirection RevealMode

        /// <summary>
        /// Gets or sets the direction in which the ExpandableContentControl 
        /// content window opens.
        /// </summary>
        public ExpandDirection RevealMode
        {
            get { return (ExpandDirection) GetValue(RevealModeProperty); }
            set { SetValue(RevealModeProperty, value); }
        }

        /// <summary>
        /// Identifies the RevealMode dependency property.
        /// </summary>
        public static readonly DependencyProperty RevealModeProperty = DependencyProperty.Register("RevealMode",
            typeof(ExpandDirection), typeof(ExpandableContentControl),
            new FrameworkPropertyMetadata(ExpandDirection.Down,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange),
            IsRevealModeValid);

        private static bool IsRevealModeValid(object value)
        {
            return value != null && Enum.IsDefined(typeof(ExpandDirection), value);
        }

        /// <summary>
        /// Gets a value indicating whether the content should be revealed horizontally.
        /// </summary>
        private bool IsHorizontalRevealMode
        {
            get { return RevealMode == ExpandDirection.Left || RevealMode == ExpandDirection.Right; }
        }

        /// <summary>
        /// Gets a value indicating whether the content should be revealed verticaly.
        /// </summary>
        private bool IsVerticalRevealMode
        {
            get { return RevealMode == ExpandDirection.Up || RevealMode == ExpandDirection.Down; }
        }

        #endregion public ExpandDirection RevealMode

        #region public double Percentage

        /// <summary>
        /// Gets or sets the relative percentage of the content that is 
        /// currently visible. A percentage of 1 corresponds to the complete
        /// TargetSize.
        /// </summary>
        public double Percentage
        {
            get { return (double) GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        /// <summary>
        /// Identifies the Percentage dependency property.
        /// </summary>
        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register("Percentage",
            typeof(double), typeof(ExpandableContentControl),
            new FrameworkPropertyMetadata(1.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        #endregion public double Percentage

        #region IsExpanded

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded",
            typeof(bool), typeof(ExpandableContentControl), new PropertyMetadata(false));

        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        #endregion

        private UIElement Child
        {
            get { return VisualChildrenCount == 0 ? null : GetVisualChild(0) as UIElement; }
        }

        /// <summary>
        /// Gets the content current visible size.
        /// </summary>
        internal Size RelevantContentSize
        {
            get
            {
                return new Size(
                    (IsHorizontalRevealMode ? Width : 0),
                    (IsVerticalRevealMode ? Height : 0));
            }
        }


        protected override Size MeasureOverride(Size availableSize)
        {
            var child = Child;
            var size = new Size();

            if (child != null)
            {
                child.Measure(availableSize);
                size = child.DesiredSize;
            }

            return Resize(size, Percentage);
        }

        private Size Resize(Size size, double factor)
        {
            return IsHorizontalRevealMode
                ? new Size(size.Width*factor, size.Height)
                : new Size(size.Width, size.Height*factor);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var child = Child;
            if (child == null || Math.Abs(Percentage) < 0.0001)
                return finalSize;

            var rect = new Rect(Resize(finalSize, 1/Percentage));

            if (RevealMode == ExpandDirection.Down)
                rect.Y = -rect.Height*(1 - Percentage);
            else if (RevealMode == ExpandDirection.Left)
                rect.X = -rect.Width*(1 - Percentage);

            child.Arrange(rect);

            return finalSize;
        }
    }
}