using System.Diagnostics;
using System.Windows.Media;

namespace System.Windows.Controls.DataVisualization.Charting
{
	internal static class PieChartHelper
	{
		const double DistanceTolerance = 0.01;
		const double DistanceSmallArc = 40;

		/// <summary>
		/// Gets the center and arc midpoint from a pie chart data point's geometry.
		/// It also determines if the arc of this geometry is small.
		/// </summary>
		/// <param name="geometry">The geometry of a pie chart data point.</param>
		/// <param name="center">The center of the pie chart.</param>
		/// <param name="midpoint">The midpoint of the arc in the pie wedge.</param>
		/// <param name="isArcSmall">True if the arc of the geometry is small, false otherwise.</param>
		/// <returns>True if the geometry is of the right form, false otherwise.</returns>
		public static bool GetPieChartInfo(Geometry geometry, out Point center, out Point arcMidpoint, out bool isArcSmall)
		{
			center = arcMidpoint = new Point();
			isArcSmall = false;

			if (geometry == null)
			{
				return false;
			}

			EllipseGeometry ellipseGeometry = geometry as EllipseGeometry;
			if (ellipseGeometry != null)
			{
				center = ellipseGeometry.Center;
				arcMidpoint = center + new Vector(ellipseGeometry.RadiusX / Math.Sqrt(2), -ellipseGeometry.RadiusY / Math.Sqrt(2));
				return true;
			}

			PathGeometry pathGeometry = geometry as PathGeometry;
			if (pathGeometry != null && pathGeometry.Figures.Count > 0)
			{
				PathFigure pathFigure = pathGeometry.Figures[0];
				if (pathFigure != null && pathFigure.Segments.Count > 1)
				{
					center = pathFigure.StartPoint;
					LineSegment lineSegment = pathFigure.Segments[0] as LineSegment;
					ArcSegment arcSegment = pathFigure.Segments[1] as ArcSegment;
					if (lineSegment != null && arcSegment != null)
					{
						CalculateArcInfo(center, lineSegment.Point, arcSegment, out arcMidpoint, out isArcSmall);
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Calculates the midpoint of the arc passed as a parameter, as well as whether the arc is small.
		/// </summary>
		/// <param name="center">The center of the pie chart.</param>
		/// <param name="startPoint">The start point of the arc segment.</param>
		/// <param name="arcSegment">The arc of the pie wedge itself.</param>
		/// <param name="arcMidpoint">he midpoint of the arc in the pie wedge.</param>
		/// <param name="isArcSmall">True if the arc is small, false otherwise.</param>
		private static void CalculateArcInfo(Point center, Point startPoint, ArcSegment arcSegment, out Point arcMidpoint, out bool isArcSmall)
		{
			// Note: we assume a valid arcSegment with equal radii.
			Debug.Assert(arcSegment != null);
			Debug.Assert(arcSegment.Size.Width == arcSegment.Size.Height);

			Point endPoint = arcSegment.Point;
			Point chordMidpoint = new Point(0.5 * (startPoint.X + endPoint.X), 0.5 * (startPoint.Y + endPoint.Y));
			Vector chordDirection = endPoint - startPoint;
			double chordLength = chordDirection.Length;
			double radius = arcSegment.Size.Width;

			isArcSmall = chordLength < DistanceSmallArc;

			// If the chord length is less than the distance tolerance, just use the chord midpoint
			// or the point on the opposite side of the circle as appropriate.
			if (chordLength < DistanceTolerance)
			{
				arcMidpoint = arcSegment.IsLargeArc ? center - (chordMidpoint - center) : chordMidpoint;
			}
			else
			{
				chordDirection /= chordLength;
				Vector radialDirection = new Vector(-chordDirection.Y, chordDirection.X);
				double halfChordLength = 0.5 * chordLength;
				double radialOffset;
				if (radius >= halfChordLength)
				{
					double sectorRadius = Math.Sqrt(radius * radius - halfChordLength * halfChordLength);
					radialOffset = -radius + (arcSegment.IsLargeArc ? -sectorRadius : sectorRadius);
				}
				else
				{
					radialOffset = -halfChordLength;
				}
				if (arcSegment.SweepDirection == SweepDirection.Counterclockwise)
				{
					radialOffset = -radialOffset;
				}
				arcMidpoint = chordMidpoint + radialOffset * radialDirection;
			}
		}
	}
}
