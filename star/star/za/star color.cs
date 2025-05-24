using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace star
{
    class star_color
    {
        public Color screen(Color x, Color y)
        {
            int coloraR = 255 - x.R;
            int coloraG = 255 - x.G;
            int coloraB = 255 - x.B;
            int colorbR = 255 - y.R;
            int colorbG = 255 - y.G;
            int colorbB = 255 - y.B;
            int averagecolora = 255 - ((coloraR * colorbR) / 255);
            int averagecolorb = 255 - ((coloraG * colorbG) / 255);
            int averagecolorc = 255 - ((coloraB * colorbB) / 255);
            Color cc = Color.FromArgb(averagecolora, averagecolorb, averagecolorc);
            return cc;
        }

        public Color reverse(Color a)
        {
            int coloraR = 255 - a.R;
            int coloraG = 255 - a.G;
            int coloraB = 255 - a.B;
            Color cc = Color.FromArgb(coloraR, coloraG, coloraB);
            return cc;
        }

        public Color dark(Color x, Color y)
        {
            Color result;
            int colorA = x.R + x.G + x.B;
            int colorB = y.R + y.G + y.B;
            if (colorA <= colorB)
            {
                result = x;
            }
            else
            {
                result = y;
            }
            return result;
        }

        public Color fuse(Color x, Color y, double f)
        {
            int coloraR = x.R;
            int coloraG = x.G;
            int coloraB = x.B;
            int colorbR = y.R;
            int colorbG = y.G;
            int colorbB = y.B;
            int averagecolora = (int)Math.Round(coloraR + (colorbR - coloraR) * f,MidpointRounding.AwayFromZero);
            int averagecolorb = (int)Math.Round(coloraG + (colorbG - coloraG) * f,MidpointRounding.AwayFromZero);
            int averagecolorc = (int)Math.Round(coloraB + (colorbB - coloraB) * f,MidpointRounding.AwayFromZero);
            Color cc = Color.FromArgb(averagecolora, averagecolorb, averagecolorc);
            return cc;
        }
    }
}
