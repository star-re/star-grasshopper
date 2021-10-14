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
    }
}
