using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace ChannelManager
{
    public static class Thumbnailer
    {
        public const int ThumbSize = 48;

        public static void CreateLogoThumb(Image logo, Guid logoId)
        {
            float imageAspectRatio = logo.Width / (float)logo.Height;
            if (logo.Width > ThumbSize || logo.Height > ThumbSize)
            {
                int iWidth = Math.Min(logo.Width, ThumbSize);
                int iHeight = Math.Min(logo.Height, ThumbSize);

                if (logo.Width > logo.Height)
                    iHeight = (int)Math.Floor((((float)iWidth) / imageAspectRatio));
                else
                    iWidth = (int)Math.Floor((imageAspectRatio * ((float)iHeight)));

                using (var thumbImage = new Bitmap(iWidth, iHeight, logo.PixelFormat))
                using (var g = Graphics.FromImage(thumbImage))
                {
                    g.CompositingQuality = CompositingQuality.AssumeLinear;
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.DrawImage(logo, new Rectangle(0, 0, iWidth, iHeight));
                    thumbImage.Save(GetThumbFilePath(logoId), ImageFormat.Png);
                }
            }
        }

        public static string GetThumbFilePath(Guid logoId)
        {
            return Path.Combine(HttpContext.Current.Server.MapPath("~/LogoThumbs"), string.Format("{0}_{1}.png", logoId, Thumbnailer.ThumbSize));
        }

        public static string GetThumbFileUrl(Guid logoId)
        {
            return string.Format("/LogoThumbs/{0}_{1}.png", logoId, Thumbnailer.ThumbSize);
        }
    }
}