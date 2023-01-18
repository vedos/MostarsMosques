using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Web.Helper
{
    public class Global
    {
        #region slika
        public static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        public static Image ResizeImage(Image imgToResize, Size size, int minHight, int minWidth)//modifikovana
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

        //
        public static List<HttpPostedFileBase> input_slika { get; set; }
        public static DateTime? lastLogin { get; set; }


        private const string  Jezik = "jezik";

        public static void PokreniNovuSesiju(Jezik jezik, HttpContextBase context)
        {
            context.Session.Add(Jezik, jezik);
        }

        public static Jezik GetJezik(HttpContextBase context)
        {
            Jezik korisnik = (Jezik)context.Session[Jezik];

            if (korisnik != null)
                return korisnik;

            using (MMContext m = new MMContext()) {
                Jezik j = new Jezik();
                j = m.Jeziks.FirstOrDefault();
                return j;
            }

        }

        public static List<Jezik> GetJezici()
        {

            using (MMContext m = new MMContext())
            {
                List<Jezik> j = new List<Jezik>();
                j = m.Jeziks.ToList();
                return j;
            }

        }


        //hardcode
        public static int JezikId = 1; //bosanski
        public static int superuserId = 2;
    }
}