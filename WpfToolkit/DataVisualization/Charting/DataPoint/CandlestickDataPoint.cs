// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace System.Windows.Controls.DataVisualization.Charting
{
    /// <summary>
    /// Represents a data point used for scatter series.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplateVisualState(Name = DataPoint.StateCommonNormal, GroupName = DataPoint.GroupCommonStates)]
    [TemplateVisualState(Name = DataPoint.StateCommonMouseOver, GroupName = DataPoint.GroupCommonStates)]
    [TemplateVisualState(Name = DataPoint.StateSelectionUnselected, GroupName = DataPoint.GroupSelectionStates)]
    [TemplateVisualState(Name = DataPoint.StateSelectionSelected, GroupName = DataPoint.GroupSelectionStates)]
    [TemplateVisualState(Name = DataPoint.StateRevealShown, GroupName = DataPoint.GroupRevealStates)]
    [TemplateVisualState(Name = DataPoint.StateRevealHidden, GroupName = DataPoint.GroupRevealStates)]
    public sealed partial class CandlestickDataPoint : DataPoint
    {
        #region public double Open
        /// <summary>
        /// Gets or sets the size value of the bubble data point.
        /// </summary>
        public double Open
        {
            get { return (double)GetValue(OpenProperty); }
            set { SetValue(OpenProperty, value); }
        }

        /// <summary>
        /// Identifies the Size dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenProperty =
            DependencyProperty.Register(
                "Open",
                typeof(double),
                typeof(CandlestickDataPoint),
                new PropertyMetadata(0.0, OnOpenPropertyChanged));

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="d">BubbleDataPoint that changed its Size.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //BubbleDataPoint source = (BubbleDataPoint)d;
            //double oldValue = (double)e.OldValue;
            //double newValue = (double)e.NewValue;
            //source.OnSizePropertyChanged(oldValue, newValue);
        }

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>        
        private void OnOpenPropertyChanged(double oldValue, double newValue)
        {
            //RoutedPropertyChangedEventHandler<double> handler = SizePropertyChanged;
            //if (handler != null)
            //{
            //    handler(this, new RoutedPropertyChangedEventArgs<double>(oldValue, newValue));
            //}

            //if (this.State == DataPointState.Created)
            //{
            //    this.ActualSize = newValue;
            //}
        }

        /// <summary>
        /// This event is raised when the size property is changed.
        /// </summary>
        internal event RoutedPropertyChangedEventHandler<double> OpenPropertyChanged;

        #endregion public double Open

        #region public double Close
        /// <summary>
        /// Gets or sets the size value of the bubble data point.
        /// </summary>
        public double Close
        {
            get { return (double)GetValue(CloseProperty); }
            set { SetValue(CloseProperty, value); }
        }

        /// <summary>
        /// Identifies the Size dependency property.
        /// </summary>
        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.Register(
                "Close",
                typeof(double),
                typeof(CandlestickDataPoint),
                new PropertyMetadata(0.0, OnClosePropertyChanged));

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="d">BubbleDataPoint that changed its Size.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnClosePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //BubbleDataPoint source = (BubbleDataPoint)d;
            //double oldValue = (double)e.OldValue;
            //double newValue = (double)e.NewValue;
            //source.OnSizePropertyChanged(oldValue, newValue);
        }

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>        
        private void OnClosePropertyChanged(double oldValue, double newValue)
        {
            //RoutedPropertyChangedEventHandler<double> handler = SizePropertyChanged;
            //if (handler != null)
            //{
            //    handler(this, new RoutedPropertyChangedEventArgs<double>(oldValue, newValue));
            //}

            //if (this.State == DataPointState.Created)
            //{
            //    this.ActualSize = newValue;
            //}
        }

        /// <summary>
        /// This event is raised when the size property is changed.
        /// </summary>
        internal event RoutedPropertyChangedEventHandler<double> ClosePropertyChanged;

        #endregion public double Close

        #region public double High
        /// <summary>
        /// Gets or sets the size value of the bubble data point.
        /// </summary>
        public double High
        {
            get { return (double)GetValue(HighProperty); }
            set { SetValue(HighProperty, value); }
        }

        /// <summary>
        /// Identifies the Size dependency property.
        /// </summary>
        public static readonly DependencyProperty HighProperty =
            DependencyProperty.Register(
                "High",
                typeof(double),
                typeof(CandlestickDataPoint),
                new PropertyMetadata(0.0, OnHighPropertyChanged));

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="d">BubbleDataPoint that changed its Size.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnHighPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //BubbleDataPoint source = (BubbleDataPoint)d;
            //double oldValue = (double)e.OldValue;
            //double newValue = (double)e.NewValue;
            //source.OnSizePropertyChanged(oldValue, newValue);
        }

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>        
        private void OnHighPropertyChanged(double oldValue, double newValue)
        {
            //RoutedPropertyChangedEventHandler<double> handler = SizePropertyChanged;
            //if (handler != null)
            //{
            //    handler(this, new RoutedPropertyChangedEventArgs<double>(oldValue, newValue));
            //}

            //if (this.State == DataPointState.Created)
            //{
            //    this.ActualSize = newValue;
            //}
        }

        /// <summary>
        /// This event is raised when the size property is changed.
        /// </summary>
        internal event RoutedPropertyChangedEventHandler<double> HighPropertyChanged;

        #endregion public double High

        #region public double Low
        /// <summary>
        /// Gets or sets the size value of the bubble data point.
        /// </summary>
        public double Low
        {
            get { return (double)GetValue(LowProperty); }
            set { SetValue(LowProperty, value); }
        }

        /// <summary>
        /// Identifies the Size dependency property.
        /// </summary>
        public static readonly DependencyProperty LowProperty =
            DependencyProperty.Register(
                "Low",
                typeof(double),
                typeof(CandlestickDataPoint),
                new PropertyMetadata(0.0, OnLowPropertyChanged));

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="d">BubbleDataPoint that changed its Size.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnLowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //BubbleDataPoint source = (BubbleDataPoint)d;
            //double oldValue = (double)e.OldValue;
            //double newValue = (double)e.NewValue;
            //source.OnSizePropertyChanged(oldValue, newValue);
        }

        /// <summary>
        /// SizeProperty property changed handler.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>        
        private void OnLowPropertyChanged(double oldValue, double newValue)
        {
            //RoutedPropertyChangedEventHandler<double> handler = SizePropertyChanged;
            //if (handler != null)
            //{
            //    handler(this, new RoutedPropertyChangedEventArgs<double>(oldValue, newValue));
            //}

            //if (this.State == DataPointState.Created)
            //{
            //    this.ActualSize = newValue;
            //}
        }

        /// <summary>
        /// This event is raised when the size property is changed.
        /// </summary>
        internal event RoutedPropertyChangedEventHandler<double> LowPropertyChanged;

        #endregion public double Low

        private const string BodyName = "PART_Body";
        private Grid Body { get; set; }
        private const string ShadowName = "PART_Shadow";
        private Grid Shadow { get; set; }


#if !SILVERLIGHT
        /// <summary>
        /// Initializes the static members of the BarDataPoint class.
        /// </summary>
        static CandlestickDataPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CandlestickDataPoint), new FrameworkPropertyMetadata(typeof(CandlestickDataPoint)));
        }
#endif

        /// <summary>
        /// Initializes a new instance of the ScatterDataPoint class.
        /// </summary>
        public CandlestickDataPoint()
        {
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(CandlestickDataPoint);
#endif
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Body = GetTemplateChild(BodyName) as Grid;
            Shadow = GetTemplateChild(ShadowName) as Grid;
        }

        public void UpdateBody(IRangeAxis rangeAxis)
        {
            if (Body == null)
                return;

            double highPointY = rangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(High)).Value;
            double lowPointY = rangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(Low)).Value;
            double openPointY = rangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(Open)).Value;
            double closePointY = rangeAxis.GetPlotAreaCoordinate(ValueHelper.ToDouble(Close)).Value;

            Thickness margin;
            if (openPointY > closePointY)
            {
                margin = new Thickness(0, highPointY - openPointY, 0, closePointY - lowPointY);
            }
            else
            {
                margin = new Thickness(0, highPointY - closePointY, 0, openPointY - lowPointY);
            }

            Body.Margin = margin;
        }
    }
}