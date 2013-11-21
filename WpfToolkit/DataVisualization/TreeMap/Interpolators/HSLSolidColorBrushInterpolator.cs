// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows.Controls.DataVisualization;
using System.Windows.Media;

namespace System.Windows.Controls.DataVisualization
{
    /// <summary>
    /// Interpolator which converts a numeric value from its [ActualDataMinimum, ActualDataMaximum]
    /// range to a color in the range [From, To].
    /// </summary>
    /// <QualityBand>Experimental</QualityBand>
    public class HSLSolidColorBrushInterpolator : RangeInterpolator<Color>
    {
        /// <summary>
        /// Interpolates the given value between its [ActualDataMinimum, ActualDataMaximum] range
        /// and returns a color in the range [From, To].
        /// </summary>
        /// <param name="value">Value to interpolate.</param>
        /// <returns>An interpolated color in the range [From, To].</returns>
        public override object Interpolate(double value)
        {
            Color color = From;
            if (ActualDataMaximum - ActualDataMinimum != 0)
            {
                double ratio = (value - ActualDataMinimum) / (ActualDataMaximum - ActualDataMinimum);

                color = color.FromAhsl(
                    (byte)((double)From.A + (ratio * (double)(To.A - From.A))),
                    From.GetHue() + (ratio * (To.GetHue() - From.GetHue())),
                    From.GetSaturation() + (ratio * (To.GetSaturation() - From.GetSaturation())),
                    From.GetLightness() + (ratio * (To.GetLightness() - From.GetLightness())));
            }

            return new SolidColorBrush(color);
        }
    }
}
