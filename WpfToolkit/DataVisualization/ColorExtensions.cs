// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace System.Windows.Controls
{
    /// <summary>
    /// Contains extensions to Color structure for HSL  color mode.
    /// </summary>
    internal static class ColorExtensions
    {
        /// <summary>
        /// Gets the color component (R,G or B) with maximal value.
        /// </summary>
        /// <param name="red">The red component value for the Color. Valid values are 0 through 255.</param>
        /// <param name="green">The green component value for the Color. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the Color. Valid values are 0 through 255.</param>
        /// <returns>Component with maximal value.</returns>
        private static byte MaxColor(byte red, byte green, byte blue)
        {
            return Math.Max(red, Math.Max(green, blue));
        }

        /// <summary>
        /// Gets the color component (R,G or B) with minimal value.
        /// </summary>
        /// <param name="red">The red component value for the Color. Valid values are 0 through 255.</param>
        /// <param name="green">The green component value for the Color. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the Color. Valid values are 0 through 255.</param>
        /// <returns>Component with minimal value.</returns>
        private static byte MinColor(byte red, byte green, byte blue)
        {
            return Math.Min(red, Math.Min(green, blue));
        }

        /// <summary>
        /// The hue, in degrees, of the Color. 
        /// The hue is measured in degrees, ranging from 0.0 through 360.0, 
        /// in HSL  color space.
        /// </summary>
        /// <param name="color">The Color structure that this method uses.</param>
        /// <returns>
        /// The hue, in degrees, of the Color structure. The hue is 
        /// measured in degrees, ranging from 0.0 through 360.0, in HSL color 
        /// space.
        /// </returns>
        public static float GetHue(this Color color)
        {
            float hue;

            int max = MaxColor(color.R, color.G, color.B);
            int min = MinColor(color.R, color.G, color.B);
            int denominator = max - min;

            if (denominator == 0)
            {
                return 0f;
            }

            if (color.R == max)
            {
                hue = (float)(color.G - color.B) / denominator;
            }
            else if (color.G == max)
            {
                hue = 2f + ((float)(color.B - color.R) / denominator);
            }
            else
            {
                // if (color.B == max)
                hue = 4f + ((float)(color.R - color.G) / denominator);
            }

            hue *= 60f;
            if (hue < 0f)
            {
                hue += 360f;
            }
            return hue;
        }

        /// <summary>
        /// Gets the hue-saturation-brightness (HSL ) saturation value for the 
        /// Color structure.
        /// </summary>
        /// <param name="color">The Color structure that this method uses.</param>
        /// <returns>
        /// The saturation of the Color structure. The saturation ranges from 
        /// 0.0 through 1.0, where 0.0 is grayscale and 1.0 is the most saturate.
        /// </returns>
        public static float GetSaturation(this Color color)
        {
            float max = MaxColor(color.R, color.G, color.B) / 255f;
            float min = MinColor(color.R, color.G, color.B) / 255f;

            if (max - min == 0)
            {
                return 0f;
            }

            float median = (max + min) / 2f;
            if (median <= 0.5)
            {
                return (float)(max - min) / (max + min);
            }
            return (max - min) / ((2f - max) - min);
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) lightness value for the 
        /// Color structure.
        /// </summary>
        /// <param name="color">The Color structure that this method uses.</param>
        /// <returns>
        /// The brightness of the Color. The brightness ranges from 0.0 through 
        /// 1.0, where 0.0 represents black and 1.0 represents white.
        /// </returns>
        public static float GetLightness(this Color color)
        {
            float max = MaxColor(color.R, color.G, color.B) / 255f;
            float min = MinColor(color.R, color.G, color.B) / 255f;

            return (max + min) / 2f;
        }

        /// <summary>
        /// Creates a new color from HSL  parameters.
        /// </summary>
        /// <param name="color">The Color structure that this method uses.</param>
        /// <param name="a">The alpha component. Valid values are 0 through 255.</param>
        /// <param name="h">The hue component. Valid values are [0, 360).</param>
        /// <param name="s">The saturation component. Valid values are [0, 1].</param>
        /// <param name="l">The brightness component. Valid values are [0, 1].</param>
        /// <returns>A newly created color structure.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Following FromArgb convention.")]
        public static Color FromAhsl(this Color color, byte a, double h, double s, double l)
        {
            double[] t = new double[3];

            // normalize hue
            h = h / 360;

            double q = l < 0.5 ? l * (1 + s) : l + s - (l * s);
            double p = (2 * l) - q;
            t[0] = h + (1.0 / 3.0);
            t[1] = h;
            t[2] = h - (1.0 / 3.0);

            for (int i = 0; i < 3; i++)
            {
                // t(c)
                if (t[i] < 0)
                {
                    t[i] += 1.0;
                }
                else if (t[i] > 1)
                {
                    t[i] -= 1.0;
                }

                // Calculate Color(c)
                if (t[i] * 6.0 < 1.0)
                {
                    t[i] = p + ((q - p) * 6 * t[i]);
                }
                else if (t[i] * 2.0 < 1.0)
                {
                    t[i] = q;
                }
                else if (t[i] * 3.0 < 2.0)
                {
                    t[i] = p + ((q - p) * 6 * ((2.0 / 3.0) - t[i]));
                }
                else
                {
                    t[i] = p;
                }
            }

            color.A = a;

            // Denormalize RGB
            color.R = (byte)Math.Round(t[0] * 255);
            color.G = (byte)Math.Round(t[1] * 255);
            color.B = (byte)Math.Round(t[2] * 255);

            return color;
        }
    }
}
