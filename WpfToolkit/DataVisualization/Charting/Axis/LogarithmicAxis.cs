using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace System.Windows.Controls.DataVisualization.Charting
{
    /// <summary>
    /// An axis that displays numeric values along a logarithmic range.
    /// </summary>
    [StyleTypedProperty(Property = "GridLineStyle", StyleTargetType = typeof(Line))]
    [StyleTypedProperty(Property = "MajorTickMarkStyle", StyleTargetType = typeof(Line))]
    [StyleTypedProperty(Property = "MinorTickMarkStyle", StyleTargetType = typeof(Line))]
    [StyleTypedProperty(Property = "AxisLabelStyle", StyleTargetType = typeof(NumericAxisLabel))]
    [StyleTypedProperty(Property = "TitleStyle", StyleTargetType = typeof(Title))]
    [TemplatePart(Name = AxisGridName, Type = typeof(Grid))]
    [TemplatePart(Name = AxisTitleName, Type = typeof(Title))]
    public class LogarithmicAxis : NumericAxis
    {
        /// <summary>
        /// Instantiates a new instance of the LogarithmicAxis
        /// </summary>
        public LogarithmicAxis()
        {
            ActualRange = new Range<IComparable>(1.0, 2.0);
        }

        /// <summary>
        /// Gets the actual range of double values.
        /// </summary>
        protected Range<double> ActualDoubleRange { get; private set; }

        /// <summary>
        /// Updates ActualDoubleRange when ActualRange changes.
        /// </summary>
        /// <param name="range">New ActualRange value.</param>
        protected override void OnActualRangeChanged(Range<IComparable> range)
        {
            ActualDoubleRange = range.ToDoubleRange();
            base.OnActualRangeChanged(range);
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
            return GetPlotAreaCoordinate(value, currentRange.ToDoubleRange(), length);
        }
        protected override UnitValue GetPlotAreaCoordinate(object value, double length)
        {
            return GetPlotAreaCoordinate(value, ActualDoubleRange, length);
        }

        /// <summary>
        /// Returns the plot area coordinate of a value.
        /// </summary>
        /// <param name="value">The value to plot.</param>
        /// <param name="range">The range of values.</param>
        /// <param name="length">The length of the axis.</param>
        /// <returns>The plot area coordinate of the value.</returns>
        protected static UnitValue GetPlotAreaCoordinate(object value, Range<double> range, double length)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (range.HasData)
            {
                double doubleValue = ValueHelper.ToDouble(value);
                Range<double> actualDoubleRange = range;

                return
                    new UnitValue
                    (
                        length /
                        Math.Log10(actualDoubleRange.Maximum / actualDoubleRange.Minimum) *
                        Math.Log10(doubleValue / actualDoubleRange.Minimum),
                        Unit.Pixels
                    );
            }

            return new UnitValue();
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

                    double output =
                        Math.Pow
                        (
                            10,
                            coordinate *
                            Math.Log10(actualDoubleRange.Maximum / actualDoubleRange.Minimum) /
                            ActualLength
                        )
                        *
                        actualDoubleRange.Minimum;

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
        /// Returns a sequence of values to plot on the axis.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>A sequence of values to plot on the axis.</returns>
        protected override IEnumerable<IComparable> GetLabelValues(Size availableSize)
        {
            return GetMajorValues(availableSize).Cast<IComparable>();
        }

        private string _majorValues = "125;250;500;1000;2000;4000;8000";
        public string MajorValues
        {
            get { return _majorValues; }
            set { _majorValues = value; }
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

            foreach (var c in MajorValues.Split(new []{';'}))
            {
                yield return double.Parse(c);
            }            
        }
    }
}