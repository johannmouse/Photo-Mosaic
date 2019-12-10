using System;
using System.Collections.Generic;
using System.Drawing;

namespace PhotoMosaic
{
    public static class ColorManagement
    {
        public static int GetNearestWebColor(Color input_color, Dictionary<int, Color> library)
        {
            double dbl_input_red = Convert.ToDouble(input_color.R);
            double dbl_input_green = Convert.ToDouble(input_color.G);
            double dbl_input_blue = Convert.ToDouble(input_color.B);
            double distance = 500.0;
            double temp;
            double dbl_test_red;
            double dbl_test_green;
            double dbl_test_blue;
            int key = 1;

            foreach (KeyValuePair<int, Color> o in library)
            {
                dbl_test_red = Math.Pow(Convert.ToDouble(o.Value.R) - dbl_input_red, 2.0);
                dbl_test_green = Math.Pow(Convert.ToDouble(o.Value.G) - dbl_input_green, 2.0);
                dbl_test_blue = Math.Pow(Convert.ToDouble(o.Value.B) - dbl_input_blue, 2.0);
                temp = Math.Sqrt(dbl_test_blue + dbl_test_green + dbl_test_red);
                if (temp < distance)
                {
                    distance = temp;
                    key = o.Key;
                }
            }

            return key;
        }

        public static Color AverageColor(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            int alpha = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    red += pixel.R;
                    green += pixel.G;
                    blue += pixel.B;
                    alpha += pixel.A;
                }
            }

            Func<int, int> avg = c => c / (width * height);
            red = avg(red);
            green = avg(green);
            blue = avg(blue);
            alpha = avg(alpha);

            var color = Color.FromArgb(alpha, red, green, blue);
            return color;
        }

        public static Dictionary<int, Color> GetAverageColorOfAllElements(string nguonAnh, int numberOfSmallImages)
        {
            Type color = (typeof(Color));
            Dictionary<int, Color> colors = new Dictionary<int, Color>();

            for (int i = 1; i <= numberOfSmallImages; i++)
            {
                string fileName = "picture (" + i + ").jpg";
                Bitmap bmp = new Bitmap(nguonAnh + "/" + fileName);
                Color averageColor = AverageColor(bmp);
                colors.Add(i, averageColor);
            }

            return colors;
        }
    }
}