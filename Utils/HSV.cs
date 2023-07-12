using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Examath.Core.Utils
{
    /// <summary>
    /// Helper class to convert from HSV to RGB values
    /// </summary>
    public class HSV
    {
        /// <summary>
        /// Creates a colour using specified HSVA values.
        /// </summary>
        /// <param name="hue">Hue between 0 and 360 degrees</param>
        /// <param name="saturation">Chroma between 0 and 1</param>
        /// <param name="value">Brightness between 0 and 1</param>
        /// <param name="alpha">Transparency between 0 and 1</param>
        /// <returns>A <see cref="Color"/></returns>
        /// <remarks>
        /// From https://stackoverflow.com/a/1626232/10701111
        /// </remarks>
        public static Color ToColor(float hue, float saturation, float value, float alpha = 1)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            byte v = Convert.ToByte(value);
            byte p = Convert.ToByte(value * (1 - saturation));
            byte q = Convert.ToByte(value * (1 - f * saturation));
            byte t = Convert.ToByte(value * (1 - (1 - f) * saturation));
            byte a = Convert.ToByte(alpha * 255);

            if (hi == 0)
                return Color.FromArgb(a, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(a, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(a, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(a, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(a, t, p, v);
            else
                return Color.FromArgb(a, v, p, q);
        }
    }
}
