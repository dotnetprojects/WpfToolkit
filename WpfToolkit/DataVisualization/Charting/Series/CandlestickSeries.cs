// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace System.Windows.Controls.DataVisualization.Charting
{
    [StyleTypedProperty(Property = "DataPointStyle", StyleTargetType = typeof(CandlestickDataPoint))]
    [StyleTypedProperty(Property = "LegendItemStyle", StyleTargetType = typeof(LegendItem))]
    public sealed partial class CandlestickSeries : DataPointSingleSeriesWithAxes
    {
        public Binding OpenValueBinding { get; set; }
        public Binding CloseValueBinding { get; set; }
        public Binding HighValueBinding { get; set; }
        public Binding LowValueBinding { get; set; }
        
        public CandlestickSeries()
        { }

        /// <summary>
        /// Gets the dependent axis as a range axis.
        /// </summary>
        public IRangeAxis ActualDependentRangeAxis { get { return this.InternalActualDependentAxis as IRangeAxis; } }

        #region public IRangeAxis DependentRangeAxis
        /// <summary>
        /// Gets or sets the dependent range axis.
        /// </summary>
        public IRangeAxis DependentRangeAxis
        {
            get { return GetValue(DependentRangeAxisProperty) as IRangeAxis; }
            set { SetValue(DependentRangeAxisProperty, value); }
        }

        /// <summary>
        /// Identifies the DependentRangeAxis dependency property.
        /// </summary>
        public static readonly DependencyProperty DependentRangeAxisProperty =
            DependencyProperty.Register(
                "DependentRangeAxis",
                typeof(IRangeAxis),
                typeof(CandlestickSeries),
                new PropertyMetadata(null, OnDependentRangeAxisPropertyChanged));

        /// <summary>
        /// DependentRangeAxisProperty property changed handler.
        /// </summary>
        /// <param name="d">CandlestickSeries that changed its DependentRangeAxis.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnDependentRangeAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CandlestickSeries source = (CandlestickSeries)d;
            IRangeAxis newValue = (IRangeAxis)e.NewValue;
            source.OnDependentRangeAxisPropertyChanged(newValue);
        }

        /// <summary>
        /// DependentRangeAxisProperty property changed handler.
        /// </summary>
        /// <param name="newValue">New value.</param>        
        private void OnDependentRangeAxisPropertyChanged(IRangeAxis newValue)
        {
            this.InternalDependentAxis = (IAxis)newValue;
        }
        #endregion public IRangeAxis DependentRangeAxis

        /// <summary>
        /// Gets the independent axis as a range axis.
        /// </summary>
        public IRangeAxis ActualIndependentRangeAxis { get { return this.InternalActualIndependentAxis as IRangeAxis; } }

        #region public IRangeAxis IndependentRangeAxis
        /// <summary>
        /// Gets or sets the independent range axis.
        /// </summary>
        public IRangeAxis IndependentRangeAxis
        {
            get { return GetValue(IndependentRangeAxisProperty) as IRangeAxis; }
            set { SetValue(IndependentRangeAxisProperty, value); }
        }

        /// <summary>
        /// Identifies the IndependentRangeAxis dependency property.
        /// </summary>
        public static readonly DependencyProperty IndependentRangeAxisProperty =
            DependencyProperty.Register(
                "IndependentRangeAxis",
                typeof(IRangeAxis),
                typeof(CandlestickSeries),
                new PropertyMetadata(null, OnIndependentRangeAxisPropertyChanged));

        /// <summary>
        /// IndependentRangeAxisProperty property changed handler.
        /// </summary>
        /// <param name="d">CandlestickSeries that changed its IndependentRangeAxis.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnIndependentRangeAxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CandlestickSeries source = (CandlestickSeries)d;
            IRangeAxis newValue = (IRangeAxis)e.NewValue;
            source.OnIndependentRangeAxisPropertyChanged(newValue);
        }

        /// <summary>
        /// IndependentRangeAxisProperty property changed handler.
        /// </summary>
        /// <param name="newValue">New value.</param>        
        private void OnIndependentRangeAxisPropertyChanged(IRangeAxis newValue)
        {
            this.InternalIndependentAxis = (IAxis)newValue;
        }
        #endregion public IRangeAxis IndependentRangeAxis

        /// <summary>
        /// Acquire a horizontal linear axis and a vertical linear axis.
        /// </summary>
        /// <param name="firstDataPoint">The first data point.</param>
        //protected override void GetAxes(DataPoint firstDataPoint)
        //{           
        //    GetRangeAxis(
        //        InternalIndependentAxis,
        //        firstDataPoint,
        //        AxisOrientation.Horizontal,
        //        () => CreateRangeAxisFromData(firstDataPoint.IndependentValue),
        //        () => InternalActualIndependentAxis as IRangeAxis,
        //        (value) => { InternalActualIndependentAxis = (IAxis)value; },
        //        (dataPoint) => dataPoint.IndependentValue);

        //    GetRangeAxis(
        //        InternalDependentAxis,
        //        firstDataPoint,
        //        AxisOrientation.Vertical,
        //        () =>
        //        {
        //            HybridAxis axis = (HybridAxis)CreateRangeAxisFromData(firstDataPoint.DependentValue);
        //            axis.ShowGridLines = true;
        //            return (IRangeAxis)axis;
        //        },
        //        () => InternalActualDependentAxis as IRangeAxis,
        //        (value) => { InternalActualDependentAxis = (IAxis)value; },
        //        (dataPoint) => dataPoint.DependentValue);
        //}
        protected override void GetAxes(DataPoint firstDataPoint)
        {
            GetAxes(
                firstDataPoint,
                (axis) => axis.Orientation == AxisOrientation.X,
                () =>
                {
                    IAxis axis = CreateRangeAxisFromData(firstDataPoint.IndependentValue);
                    if (axis == null)
                    {
                        axis = new CategoryAxis();
                    }
                    axis.Orientation = AxisOrientation.X;
                    return axis;
                },
                (axis) => axis.Orientation == AxisOrientation.Y && axis is IRangeAxis,
                () =>
                {
                    DisplayAxis axis = (DisplayAxis)CreateRangeAxisFromData(firstDataPoint.DependentValue);
                    if (axis == null)
                    {
                        throw new InvalidOperationException(Properties.Resources.DataPointSeriesWithAxes_NoSuitableAxisAvailableForPlottingDependentValue);
                    }
                    axis.ShowGridLines = true;
                    axis.Orientation = AxisOrientation.Y;
                    return axis;
                });
        }

        /// <summary>
        /// Returns the custom ResourceDictionary to use for necessary resources.
        /// </summary>
        /// <returns>
        /// ResourceDictionary to use for necessary resources.
        /// </returns>
        protected override IEnumerator<ResourceDictionary> GetResourceDictionaryEnumeratorFromHost()
        {
            return GetResourceDictionaryWithTargetType(SeriesHost, typeof(CandlestickDataPoint), true);
        }

        protected override void PrepareDataPoint(DataPoint dataPoint, object dataContext)
        {
            base.PrepareDataPoint(dataPoint, dataContext);

            CandlestickDataPoint candlestickDataPoint = (CandlestickDataPoint)dataPoint;
            candlestickDataPoint.SetBinding(CandlestickDataPoint.OpenProperty, OpenValueBinding);
            candlestickDataPoint.SetBinding(CandlestickDataPoint.CloseProperty, CloseValueBinding);
            candlestickDataPoint.SetBinding(CandlestickDataPoint.HighProperty, HighValueBinding);
            candlestickDataPoint.SetBinding(CandlestickDataPoint.LowProperty, LowValueBinding);
        }

        /// <summary>
        /// Creates a new Candlestick data point.
        /// </summary>
        /// <returns>A Candlestick data point.</returns>
        protected override DataPoint CreateDataPoint()
        {
            return new CandlestickDataPoint();
        }

        /// <summary>
        /// Returns the style to use for all data points.
        /// </summary>
        /// <returns>The style to use for all data points.</returns>
        //protected override Style GetDataPointStyleFromHost()
        //{
        //    return SeriesHost.NextStyle(typeof(CandlestickDataPoint), true);
        //}

        /// <summary>
        /// This method updates a single data point.
        /// </summary>
        /// <param name="dataPoint">The data point to update.</param>
        protected override void UpdateDataPoint(DataPoint dataPoint)
        {
            CandlestickDataPoint candlestickDataPoint = (CandlestickDataPoint)dataPoint;

            double PlotAreaHeight = ActualDependentRangeAxis.GetPlotAreaCoordinate(ActualDependentRangeAxis.Range.Maximum).Value;
            double dataPointX = ActualIndependentRangeAxis.GetPlotAreaCoordinate(dataPoint.ActualIndependentValue).Value;
            double highPointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(candlestickDataPoint.High)).Value;
            double lowPointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(candlestickDataPoint.Low)).Value;
            double openPointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(candlestickDataPoint.Open)).Value;
            double closePointY = ActualDependentRangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(candlestickDataPoint.Close)).Value;

            candlestickDataPoint.UpdateBody(ActualDependentRangeAxis);

            if (ValueHelper.CanGraph(dataPointX))
            {
                dataPoint.Height = Math.Abs(highPointY - lowPointY);
                dataPoint.Width = 5.0;

                if (dataPoint.ActualWidth == 0.0 || dataPoint.ActualHeight == 0.0)
                    dataPoint.UpdateLayout();

                Canvas.SetLeft(dataPoint,
                    Math.Round(dataPointX - (dataPoint.ActualWidth / 2)));
                Canvas.SetTop(dataPoint,
                    Math.Round(PlotAreaHeight - highPointY));
            }
        }
    }
}
