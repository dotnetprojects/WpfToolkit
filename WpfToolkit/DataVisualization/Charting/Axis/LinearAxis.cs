// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace System.Windows.Controls.DataVisualization.Charting
{
    /// <summary>
    /// An axis that displays numeric values.
    /// </summary>
    [StyleTypedProperty(Property = "GridLineStyle", StyleTargetType = typeof(Line))]
    [StyleTypedProperty(Property = "MajorTickMarkStyle", StyleTargetType = typeof(Line))]
    [StyleTypedProperty(Property = "MinorTickMarkStyle", StyleTargetType = typeof(Line))]
    [StyleTypedProperty(Property = "AxisLabelStyle", StyleTargetType = typeof(NumericAxisLabel))]
    [StyleTypedProperty(Property = "TitleStyle", StyleTargetType = typeof(Title))]
    [TemplatePart(Name = AxisGridName, Type = typeof(Grid))]
    [TemplatePart(Name = AxisTitleName, Type = typeof(Title))]
    public class LinearAxis : NumericAxis
    {
        #region public double? Interval
        /// <summary>
        /// Gets or sets the axis interval.
        /// </summary>
        [TypeConverter(typeof(NullableConverter<double>))]
        public double? Interval
        {
            get { return (double?)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// Identifies the Interval dependency property.
        /// </summary>
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register(
                "Interval",
                typeof(double?),
                typeof(LinearAxis),
                new PropertyMetadata(null, OnIntervalPropertyChanged));

        /// <summary>
        /// IntervalProperty property changed handler.
        /// </summary>
        /// <param name="d">LinearAxis that changed its Interval.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LinearAxis source = (LinearAxis)d;
            source.OnIntervalPropertyChanged();
        }

        /// <summary>
        /// IntervalProperty property changed handler.
        /// </summary>
        private void OnIntervalPropertyChanged()
        {
            OnInvalidated(new RoutedEventArgs());
        }
        #endregion public double? Interval

        #region public double ActualInterval
        /// <summary>
        /// Gets the actual interval of the axis.
        /// </summary>
        public double ActualInterval
        {
            get { return (double)GetValue(ActualIntervalProperty); }
            private set { SetValue(ActualIntervalProperty, value); }
        }

        /// <summary>
        /// Identifies the ActualInterval dependency property.
        /// </summary>
        public static readonly DependencyProperty ActualIntervalProperty =
            DependencyProperty.Register(
                "ActualInterval",
                typeof(double),
                typeof(LinearAxis),
                new PropertyMetadata(double.NaN));
        #endregion public double ActualInterval

        /// <summary>
        /// Instantiates a new instance of the LinearAxis class.
        /// </summary>
        public LinearAxis()
        {
            this.ActualRange = new Range<IComparable>(0.0, 1.0);
        }

        /// <summary>
        /// Returns the plot area coordinate of a value.
        /// </summary>
        /// <param name="value">The value to plot.</param>
        /// <param name="currentRange">The range of values.</param>
        /// <param name="length">The length of axis.</param>
        /// <returns>The plot area coordinate of a value.</returns>
        protected override UnitValue GetPlotAreaCoordinate(object value, Range<IComparable> currentRange, double length)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (currentRange.HasData)
            {
                double doubleValue = ValueHelper.ToDouble(value);
                Range<double> actualDoubleRange = currentRange.ToDoubleRange();

                double pixelLength = Math.Max(length - 1, 0);
                double rangelength = actualDoubleRange.Maximum - actualDoubleRange.Minimum;

                return new UnitValue((doubleValue - actualDoubleRange.Minimum) * (pixelLength / rangelength), Unit.Pixels);
            }

            return UnitValue.NaN();
        }

        /// <summary>
        /// Returns the actual interval to use to determine which values are 
        /// displayed in the axis.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>Actual interval to use to determine which values are 
        /// displayed in the axis.
        /// </returns>
        protected virtual double CalculateActualInterval(Size availableSize)
        {
            if (Interval != null)
            {
                return Interval.Value;
            }

            Range<double> doubleActualRange = ActualRange.ToDoubleRange();

            // helper functions
            Func<double, double> Exponent = x => Math.Ceiling(Math.Log(x, 10));
            Func<double, double> Mantissa = x => x / Math.Pow(10, Exponent(x) - 1);

            // reduce intervals for horizontal axis.
            double maxIntervals = Orientation == AxisOrientation.X ? MaximumAxisIntervalsPer200Pixels * 0.8 : MaximumAxisIntervalsPer200Pixels;
            // real maximum interval count
            double maxIntervalCount = GetLength(availableSize) / 200 * maxIntervals;

            double range = Math.Abs(doubleActualRange.Minimum - doubleActualRange.Maximum);
            double interval = Math.Pow(10, Exponent(range));
            double tempInterval = interval;

            // decrease interval until interval count becomes less than maxIntervalCount
            while (true)
            {
                int mantissa = (int)Mantissa(tempInterval);
                if (mantissa == 5)
                {
                    // reduce 5 to 2
                    tempInterval = ValueHelper.RemoveNoiseFromDoubleMath(tempInterval / 2.5);
                }
                else if (mantissa == 2 || mantissa == 1 || mantissa == 10)
                {
                    // reduce 2 to 1,10 to 5,1 to 0.5
                    tempInterval = ValueHelper.RemoveNoiseFromDoubleMath(tempInterval / 2.0);
                }

                if (range / tempInterval > maxIntervalCount)
                {
                    break;
                }

                interval = tempInterval;
            }
            return interval;
        }

        /// <summary>
        /// Returns a sequence of values to create major tick marks for.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>A sequence of values to create major tick marks for.
        /// </returns>
        protected override IEnumerable<IComparable> GetMajorTickMarkValues(Size availableSize)
        {
            return GetMajorValues(availableSize).Cast<IComparable>();
        }

        /// <summary>
        /// Returns a sequence of major axis values.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>A sequence of major axis values.
        /// </returns>
        private IEnumerable<double> GetMajorValues(Size availableSize)
        {
            if (!ActualRange.HasData || ValueHelper.Compare(ActualRange.Minimum, ActualRange.Maximum) == 0 || GetLength(availableSize) == 0.0)
            {
                yield break;
            }
            Range<double> typedActualRange = ActualRange.ToDoubleRange();
            this.ActualInterval = CalculateActualInterval(availableSize);
            double startValue = AlignToInterval(typedActualRange.Minimum, this.ActualInterval);
            if (startValue < typedActualRange.Minimum)
            {
                startValue = AlignToInterval(typedActualRange.Minimum + this.ActualInterval, this.ActualInterval);
            }
            double nextValue = startValue;
            for (int counter = 1; nextValue <= typedActualRange.Maximum; counter++)
            {
                yield return nextValue;
                nextValue = startValue + (counter * this.ActualInterval);
            }
        }

        /// <summary>
        /// Returns a sequence of values to plot on the axis.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>A sequence of values to plot on the axis.</returns>
        protected override IEnumerable<IComparable> GetLabelValues(Size availableSize)
        {
            return GetMajorValues(availableSize).Cast<IComparable>();
        }

        /// <summary>
        /// Aligns a value to the provided interval value.  The aligned value
        /// should always be smaller than or equal to than the provided value.
        /// </summary>
        /// <param name="value">The value to align to the interval.</param>
        /// <param name="interval">The interval to align to.</param>
        /// <returns>The aligned value.</returns>
        private static double AlignToInterval(double value, double interval)
        {
            double typedInterval = (double)interval;
            double typedValue = (double)value;
            return ValueHelper.RemoveNoiseFromDoubleMath(ValueHelper.RemoveNoiseFromDoubleMath(Math.Floor(typedValue / typedInterval)) * typedInterval);
        }

        /// <summary>
        /// Returns the value range given a plot area coordinate.
        /// </summary>
        /// <param name="value">The plot area position.</param>
        /// <returns>The value at that plot area coordinate.</returns>
        protected override IComparable GetValueAtPosition(UnitValue value)
        {
            if (ActualRange.HasData && ActualLength != 0.0)
            {
                if (value.Unit == Unit.Pixels)
                {
                    double coordinate = value.Value;
                    Range<double> actualDoubleRange = ActualRange.ToDoubleRange();

                    double rangelength = actualDoubleRange.Maximum - actualDoubleRange.Minimum;
                    double output = ((coordinate * (rangelength / ActualLength)) + actualDoubleRange.Minimum);

                    return output;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return null;
        }

        /// <summary>
        /// Function that uses the mid point of all the data values
        /// in the value margins to convert a length into a range
        /// of data with the mid point as the center of that range.
        /// </summary>
        /// <param name="midPoint">The mid point of the range.</param>
        /// <param name="length">The length of the range.</param>
        /// <returns>The range object.</returns>
        private static Range<double> LengthToRange(double midPoint, double length)
        {
            double halfLength = length / 2.0;
            return new Range<double>(midPoint - halfLength, midPoint + halfLength);
        }

        /// <summary>
        /// Overrides the actual range to ensure that it is never set to an
        /// empty range.
        /// </summary>
        /// <param name="range">The range to override.</param>
        /// <returns>Returns the overridden range.</returns>
        protected override Range<IComparable> OverrideDataRange(Range<IComparable> range)
        {
            range = base.OverrideDataRange(range);
            if (!range.HasData)
            {
                return new Range<IComparable>(0.0, 1.0);
            }
            else if (ValueHelper.Compare(range.Minimum, range.Maximum) == 0)
            {
                Range<IComparable> outputRange = new Range<IComparable>((ValueHelper.ToDouble(range.Minimum)) - 1, (ValueHelper.ToDouble(range.Maximum)) + 1);
                return outputRange;
            }

            // ActualLength of 1.0 or less maps all points to the same coordinate
            if (range.HasData && this.ActualLength > 1.0)
            {
                bool isDataAnchoredToOrigin = false;
                IList<ValueMarginCoordinateAndOverlap> valueMargins = new List<ValueMarginCoordinateAndOverlap>();
                foreach (IValueMarginProvider valueMarginProvider in this.RegisteredListeners.OfType<IValueMarginProvider>())
                {
                    foreach (ValueMargin valueMargin in valueMarginProvider.GetValueMargins(this))
                    {
                        IAnchoredToOrigin dataAnchoredToOrigin = valueMarginProvider as IAnchoredToOrigin;
                        isDataAnchoredToOrigin = (dataAnchoredToOrigin != null && dataAnchoredToOrigin.AnchoredAxis == this);
                        
                        valueMargins.Add(
                        new ValueMarginCoordinateAndOverlap
                        {
                            ValueMargin = valueMargin,
                        });
                    }
                }

                if (valueMargins.Count > 0)
                {
                    double maximumPixelMarginLength =
                        valueMargins
                        .Select(valueMargin => valueMargin.ValueMargin.LowMargin + valueMargin.ValueMargin.HighMargin)
                        .MaxOrNullable().Value;

                    // Requested margin is larger than the axis so give up
                    // trying to find a range that will fit it.
                    if (maximumPixelMarginLength > this.ActualLength)
                    {
                        return range;
                    }

                    Range<double> originalRange = range.ToDoubleRange();
                    Range<double> currentRange = range.ToDoubleRange();

                    // Ensure range is not empty.
                    if (currentRange.Minimum == currentRange.Maximum)
                    {
                        currentRange = new Range<double>(currentRange.Maximum - 1, currentRange.Maximum + 1);
                    }

                    // priming the loop
                    double actualLength = this.ActualLength;
                    ValueMarginCoordinateAndOverlap maxLeftOverlapValueMargin;
                    ValueMarginCoordinateAndOverlap maxRightOverlapValueMargin;
                    UpdateValueMargins(valueMargins, currentRange.ToComparableRange());
                    GetMaxLeftAndRightOverlap(valueMargins, out maxLeftOverlapValueMargin, out maxRightOverlapValueMargin);

                    while (maxLeftOverlapValueMargin.LeftOverlap > 0 || maxRightOverlapValueMargin.RightOverlap > 0)
                    {
                        double unitOverPixels = currentRange.GetLength().Value / actualLength;
                        double newMinimum = currentRange.Minimum - ((maxLeftOverlapValueMargin.LeftOverlap + 0.5) * unitOverPixels);
                        double newMaximum = currentRange.Maximum + ((maxRightOverlapValueMargin.RightOverlap + 0.5) * unitOverPixels);

                        currentRange = new Range<double>(newMinimum, newMaximum);
                        UpdateValueMargins(valueMargins, currentRange.ToComparableRange());
                        GetMaxLeftAndRightOverlap(valueMargins, out maxLeftOverlapValueMargin, out maxRightOverlapValueMargin);
                    }

                    if (isDataAnchoredToOrigin)
                    {
                        if (originalRange.Minimum >= 0 && currentRange.Minimum < 0)
                        {
                            currentRange = new Range<double>(0, currentRange.Maximum);
                        }
                        else if (originalRange.Maximum <= 0 && currentRange.Maximum > 0)
                        {
                            currentRange = new Range<double>(currentRange.Minimum, 0);
                        }
                    }

                    return currentRange.ToComparableRange();
                }
            }
            return range;
        }
    }
}