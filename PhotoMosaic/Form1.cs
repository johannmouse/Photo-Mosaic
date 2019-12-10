using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhotoMosaic
{
    public partial class Form1 : Form
    {
        int key = 1;
        Image image;
        int countPixel = 0;
        Dictionary<int, Color> colorLibrary = new Dictionary<int, Color>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int numberOfSmallImages = Int32.Parse(textBox0.Text);
                string largeImagePath = textBox1.Text;
                largeImagePath = largeImagePath.Replace("\\", "/");
                string libraryPath = textBox2.Text;
                libraryPath = libraryPath.Replace("\\", "/");
                int smallImageSize = Int32.Parse(textBox3.Text);
                string outputImagePath = textBox4.Text;
                outputImagePath = outputImagePath.Replace("\\", "/");
                int colargeImagePath = Int32.Parse(textBox5.Text);

                colorLibrary = ColorManagement.GetAverageColorOfAllElements(libraryPath, numberOfSmallImages);
                Bitmap input = new Bitmap(largeImagePath);
                Bitmap mainPicture = new Bitmap(input, colargeImagePath, colargeImagePath);
                for (int y = 0; y <= mainPicture.Height - smallImageSize; y += smallImageSize)
                {
                    for (int x = 0; x <= mainPicture.Width - smallImageSize; x += smallImageSize)
                    {
                        int width = smallImageSize;
                        int height = smallImageSize;
                        int red = 0;
                        int green = 0;
                        int blue = 0;
                        int alpha = 0;
                        for (int yy = y; yy < y + smallImageSize; yy++)
                        {
                            for (int xx = x; xx < x + smallImageSize; xx++)
                            {
                                var curentPixel = mainPicture.GetPixel(xx, yy);
                                red += curentPixel.R;
                                green += curentPixel.G;
                                blue += curentPixel.B;
                                alpha += curentPixel.A;
                            }
                        }
                        int avg(int c) => c / (width * height);
                        red = avg(red);
                        green = avg(green);
                        blue = avg(blue);
                        alpha = avg(alpha);

                        var avgColor = Color.FromArgb(alpha, red, green, blue);

                        key = ColorManagement.GetNearestWebColor(avgColor, colorLibrary);

                        string fileName = "picture (" + key + ").jpg";
                        Bitmap bmp = new Bitmap(libraryPath + "/" + fileName);
                        image = bmp;
                        using (Graphics g = Graphics.FromImage(mainPicture))
                        {
                            g.DrawImage(image, x, y, smallImageSize, smallImageSize);
                        }
                        countPixel++;
                    }
                }
                mainPicture.Save(outputImagePath);
                MessageBox.Show("Success.");
            }
            catch
            {
                MessageBox.Show("Invalid input. Try again.");
            }
        }
    }
}