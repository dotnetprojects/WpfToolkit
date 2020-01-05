// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

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
                new PropertyMetadata(0.0));

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
                new PropertyMetadata(0.0));

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
                new PropertyMetadata(0.0));

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
                new PropertyMetadata(0.0));

        #endregion public double Low

        private const string BodyName = "PART_Body";
        private Grid Body { get; set; }
        private const string ShadowName = "PART_Shadow";
        private Grid Shadow { get; set; }

        /// <summary>
        /// Initializes the static members of the BarDataPoint class.
        /// </summary>
        static CandlestickDataPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CandlestickDataPoint), new FrameworkPropertyMetadata(typeof(CandlestickDataPoint)));
        }

        /// <summary>
        /// Initializes a new instance of the ScatterDataPoint class.
        /// </summary>
        public CandlestickDataPoint()
        {
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
