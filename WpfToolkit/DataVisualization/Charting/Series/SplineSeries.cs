// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

#if !DEFINITION_SERIES_COMPATIBILITY_MODE

namespace System.Windows.Controls.DataVisualization.Charting
{
    /// <summary>
    /// Represents a control that contains a data series to be rendered in X/Y 
    /// line format.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [StyleTypedProperty(Property = DataPointStyleName, StyleTargetType = typeof(LineDataPoint))]
    [StyleTypedProperty(Property = "LegendItemStyle", StyleTargetType = typeof(LegendItem))]
    [StyleTypedProperty(Property = "PathStyle", StyleTargetType = typeof(Path))]
    [TemplatePart(Name = DataPointSeries.PlotAreaName, Type = typeof(Canvas))]
    [SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance", Justification = "Depth of hierarchy is necessary to avoid code duplication.")]
    public partial class SplineSeries : LineAreaBaseSeries<LineDataPoint>
    {
        #region public PointCollection Points
        /// <summary>
        /// Gets the collection of points that make up the spline.
        /// </summary>
        public PointCollection Points
        {
            get { return GetValue(PointsProperty) as PointCollection; }
            private set { SetValue(PointsProperty, value); }
        }

        /// <summary>
        /// Identifies the Points dependency property.
        /// </summary>
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register(
                "Points",
                typeof(PointCollection),
                typeof(SplineSeries),
                null);
        #endregion public PointCollection Points

        #region public PathGeometry SplinePoints
        /// <summary>
        /// Gets the collection of points that make up the line.
        /// </summary>
        public PathGeometry SplinePoints
        {
            get { return GetValue(SplinePointsProperty) as PathGeometry; }
            private set { SetValue(SplinePointsProperty, value); }
        }

        /// <summary>
        /// Identifies the SplinePoints dependency property.
        /// </summary>
        public static readonly DependencyProperty SplinePointsProperty =
            DependencyProperty.Register(
                "SplinePoints",
                typeof(PathGeometry),
                typeof(SplineSeries),
                null);
        #endregion public PathGeometry SplinePoints

        #region public double SplineTension

        /// <summary>
        /// Gets or sets the tension in the beziers that make up the spline.
        /// </summary>
        /// <remarks>
        /// The greater the tension, the more straight/linear the spline will look.
        /// Less tension creates a more curvy spline.
        /// </remarks>
        public double SplineTension
        {
            get { return (double)GetValue(SplineTensionProperty); }
            set { SetValue(SplineTensionProperty, value); }
        }

        /// <summary>
        /// Identifies the SplineTension dependency property.
        /// </summary>
        public static readonly DependencyProperty SplineTensionProperty =
            DependencyProperty.Register(
                "SplineTension",
                typeof(double),
                typeof(SplineSeries),
                new PropertyMetadata(2.5));
        #endregion public double SplineTension

        #region public Style PathStyle
        /// <summary>
        /// Gets or sets the style of the Path object that follows the data 
        /// points.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Path", Justification = "Matches System.Windows.Shapes.Path.")]
        public Style PathStyle
        {
            get { return GetValue(PathStyleProperty) as Style; }
            set { SetValue(PathStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the PathStyle dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Path", Justification = "Matches System.Windows.Shapes.Path.")]
        public static readonly DependencyProperty PathStyleProperty =
            DependencyProperty.Register(
                "PathStyle",
                typeof(Style),
                typeof(SplineSeries),
                null);
        #endregion public Style PathStyle

#if !SILVERLIGHT
        /// <summary>
        /// Initializes the static members of the LineSeries class.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Dependency properties are initialized in-line.")]
        static SplineSeries()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplineSeries), new FrameworkPropertyMetadata(typeof(SplineSeries)));
        }

#endif
        /// <summary>
        /// Initializes a new instance of the LineSeries class.
        /// </summary>
        public SplineSeries()
        {
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(SplineSeries);
#endif
        }

        /// <summary>
        /// Acquire a horizontal linear axis and a vertical linear axis.
        /// </summary>
        /// <param name="firstDataPoint">The first data point.</param>
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
        /// Updates the Series shape object from a collection of Points.
        /// </summary>
        /// <param name="points">Collection of Points.</param>
        protected override void UpdateShapeFromPoints(IEnumerable<Point> points)
        {
            if (points.Any())
            {
                PointCollection pointCollection = new PointCollection();
                foreach (Point point in points)
                {
                    pointCollection.Add(point);
                }

                //At least two points are necessary to generate a proper spline
                if (pointCollection.Count >= 2)
                {
                    PathGeometry geometry = new PathGeometry();
                    PathFigure figure = new PathFigure();

                    PointCollection bezierPoints = GetBezierPoints(pointCollection);

                    figure.StartPoint = bezierPoints[0];
                    for (int i = 1; i < bezierPoints.Count; i += 3)
                    {
                        figure.Segments.Add(new BezierSegment()
                        {
                            Point1 = bezierPoints[i],
                            Point2 = bezierPoints[i + 1],
                            Point3 = bezierPoints[i + 2]
                        });
                    }

                    geometry.Figures.Add(figure);
                    SplinePoints = geometry;
                }
                else
                {
                    SplinePoints = null;
                }

                Points = pointCollection;
            }
            else
            {
                Points = null;
                SplinePoints = null;
            }
        }

        #region Bezier Curve Building

        /*
         * Formulas and code pulled from Kerem Kat's MapBezier example:
         * http://www.codeproject.com/KB/silverlight/MapBezier.aspx
         */

        private PointCollection GetBezierPoints(PointCollection pts)
        {
            PointCollection ret = new PointCollection();

            for (int i = 0; i < pts.Count; i++)
            {
                // for first point append as is.
                if (i == 0)
                {
                    ret.Add(pts[0]);
                    continue;
                }

                // for each point except first and last get B1, B2. next point. 
                // Last point do not have a next point.
                ret.Add(GetB1(pts, i - 1, SplineTension));
                ret.Add(GetB2(pts, i - 1, SplineTension));
                ret.Add(pts[i]);
            }

            return ret;
        }

        private Point GetB1(PointCollection pts, int i, double a)
        {
            Point derivedPoint = GetDerivative(pts, i, a);
            return new Point(pts[i].X + derivedPoint.X / 3, pts[i].Y + derivedPoint.Y / 3);
        }

        private Point GetB2(PointCollection pts, int i, double a)
        {
            Point derivedPoint = GetDerivative(pts, i + 1, a);
            return new Point(pts[i + 1].X - derivedPoint.X / 3, pts[i + 1].Y - derivedPoint.Y / 3);
        }

        private Point GetDerivative(PointCollection pts, int i, double a)
        {
            if (pts.Count < 2)
                throw new ArgumentOutOfRangeException("pts", "Data must contain at least two points.");

            if (i == 0)
            {
                // First point.
                return new Point((pts[1].X - pts[0].X) / a, (pts[1].Y - pts[0].Y) / a);
            }
            if (i == pts.Count - 1)
            {
                // Last point.
                return new Point((pts[i].X - pts[i - 1].X) / a, (pts[i].Y - pts[i - 1].Y) / a);
            }

            return new Point((pts[i + 1].X - pts[i - 1].X) / a, (pts[i + 1].Y - pts[i - 1].Y) / a);
        }
        #endregion
    }
}

#endif