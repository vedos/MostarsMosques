using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_API.Helper
{
    public class GlobalApi
    {
        #region slika
        public static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        public static Image ResizeImage(Image imgToResize, Size size, int minHight, int minWidth)//modifikofana
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            #region moj dodatak
            float x;
            if (sourceHeight < sourceWidth)
            {
                x = (float)minHight / (sourceHeight * nPercent);

            }
            else
            {
                x = (float)minWidth / (sourceWidth * nPercent);
            }
            #endregion

            int destWidth = (int)(sourceWidth * nPercent * x + 0.5); //0.5 dodat
            int destHeight = (int)(sourceHeight * nPercent * x + 0.5);//

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
           
        }
        #endregion

        public static int LANGUAGE_CHOICE = 1;
    }

    
}