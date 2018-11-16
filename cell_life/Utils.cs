using System.Drawing;

namespace LogLim.EasyCellLife
{
    internal class Utils
    {
        public static Color InverseColor(Color c)
        {
            return Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
        }

        public static Color MergeColor(Color c1, Color c2, double pivot)
        {
            var a = (int)(c1.A * pivot + c2.A * (1 - pivot));
            var r = (int)(c1.R * pivot + c2.R * (1 - pivot));
            var g = (int)(c1.G * pivot + c2.G * (1 - pivot));
            var b = (int)(c1.B * pivot + c2.B * (1 - pivot));

            return Color.FromArgb(a, r, g, b);
        }

        public static Color MaxContrastColor(Color c1, Color c2)
        {
            return MergeColor(InverseColor(c1), InverseColor(c2), 0.5);
        }
    }
}
