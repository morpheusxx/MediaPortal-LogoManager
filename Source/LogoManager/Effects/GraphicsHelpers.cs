using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MediaPortal.LogoManager.Effects
{
  public static class GraphicsHelpers
  {
    #region common graphic routines

    public static void ChangeColor(this Bitmap bmp, Color newColor)
    {
      Color transp = Color.FromArgb(0);
      for (int x = 0; x < bmp.Width; x++)
      {
        for (int y = 0; y < bmp.Height; y++)
        {
          Color pix = bmp.GetPixel(x, y);
          bmp.SetPixel(x, y, (pix.A > 0) ? Color.FromArgb(pix.A, newColor.R, newColor.G, newColor.B) : transp);
        }
      }
    }

    public static void FastBlur(this Bitmap sourceImage, int radius, Color forcedColor)
    {
      // http://snippetsfor.net/Csharp/StackBlur (adopted for transparency support by Vasilich)
      // Two important aspects of the method in terms of speed are
      // A look-up table for figuring the average from a sum (this is the array 'dv[]')
      // A running-sum for averaging -- this is pre-populated, then pixels are added from the right of the radius and subtracted from the left.
      var rct = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
      var dest = new int[rct.Width * rct.Height];
      var source = new int[rct.Width * rct.Height];
      var bits = sourceImage.LockBits(rct, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
      Marshal.Copy(bits.Scan0, source, 0, source.Length);
      sourceImage.UnlockBits(bits);

      if (radius < 1)
        return;

      int w = rct.Width;
      int h = rct.Height;
      int wm = w - 1;
      int hm = h - 1;
      int wh = w * h;
      int div = radius + radius + 1;
      var a = new int[wh];
      var r = new int[wh];
      var g = new int[wh];
      var b = new int[wh];
      long rsum, gsum, bsum, asum;
      int x, y, i, p1, p2, yi;
      var vmin = new int[Math.Max(w, h)];
      var vmax = new int[Math.Max(w, h)];

      var dv = new int[256 * div];
      for (i = 0; i < 256 * div; i++)
      {
        dv[i] = (i / div);
      }

      int yw = yi = 0;

      for (y = 0; y < h; y++)
      { // blur horizontal
        rsum = gsum = bsum = asum = 0;
        for (i = -radius; i <= radius; i++)
        {
          int p = source[yi + Math.Min(wm, Math.Max(i, 0))];
          asum += (p & 0xFF000000) >> 24;
          rsum += (p & 0x00ff0000) >> 16;
          gsum += (p & 0x0000ff00) >> 8;
          bsum += (p & 0x000000ff);
        }
        for (x = 0; x < w; x++)
        {
          a[yi] = dv[asum];
          r[yi] = dv[rsum];
          g[yi] = dv[gsum];
          b[yi] = dv[bsum];

          if (y == 0)
          {
            vmin[x] = Math.Min(x + radius + 1, wm);
            vmax[x] = Math.Max(x - radius, 0);
          }
          p1 = source[yw + vmin[x]];
          p2 = source[yw + vmax[x]];

          asum += ((p1 & 0xff000000) - (p2 & 0xff000000)) >> 24;
          rsum += ((p1 & 0x00ff0000) - (p2 & 0x00ff0000)) >> 16;
          gsum += ((p1 & 0x0000ff00) - (p2 & 0x0000ff00)) >> 8;
          bsum += ((p1 & 0x000000ff) - (p2 & 0x000000ff));
          yi++;
        }
        yw += w;
      }

      for (x = 0; x < w; x++)
      { // blur vertical
        rsum = gsum = bsum = asum = 0;
        int yp = -radius * w;
        for (i = -radius; i <= radius; i++)
        {
          yi = Math.Max(0, yp) + x;
          asum += a[yi];
          rsum += r[yi];
          gsum += g[yi];
          bsum += b[yi];
          yp += w;
        }
        yi = x;
        for (y = 0; y < h; y++)
        {
          if (forcedColor.IsEmpty)
            dest[yi] = (int)((uint)(dv[asum] << 24) | (uint)(dv[rsum] << 16) | (uint)(dv[gsum] << 8) | (uint)dv[bsum]);
          else
            dest[yi] = (int)((uint)(dv[asum] << 24) | (uint)(forcedColor.R << 16) | (uint)(forcedColor.G << 8) | (uint)forcedColor.B);

          if (x == 0)
          {
            vmin[y] = Math.Min(y + radius + 1, hm) * w;
            vmax[y] = Math.Max(y - radius, 0) * w;
          }
          p1 = x + vmin[y];
          p2 = x + vmax[y];

          asum += a[p1] - a[p2];
          rsum += r[p1] - r[p2];
          gsum += g[p1] - g[p2];
          bsum += b[p1] - b[p2];

          yi += w;
        }
      }

      // copy back to image
      var bits2 = sourceImage.LockBits(rct, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
      Marshal.Copy(dest, 0, bits2.Scan0, dest.Length);
      sourceImage.UnlockBits(bits);
    }

    public static float BitmapBrightness(this Bitmap bmp)
    {
      double brightnessSum = 0.0d;
      int nonTranspPxelCount = 0;
      for (int x = 0; x < bmp.Width; x++)
      {
        for (int y = 0; y < bmp.Height; y++)
        {
          Color pix = bmp.GetPixel(x, y);
          if (pix.A <= 127)
            continue;
          brightnessSum += pix.GetBrightness();
          nonTranspPxelCount++;
        }
      }
      return (float)(brightnessSum / nonTranspPxelCount);
    }

    public static void ChangeOpacity(this Bitmap bmp, float alphaMultiplier)
    {
      for (int x = 0; x < bmp.Width; x++)
        for (int y = 0; y < bmp.Height; y++)
        {
          Color pix = bmp.GetPixel(x, y);
          if ((pix.A > 0))
            bmp.SetPixel(x, y, Color.FromArgb(Math.Min((int) (pix.A*alphaMultiplier), 255), pix.R, pix.G, pix.B));
        }
    }

    public static Bitmap Contour(Bitmap srcImage, int contourWidth)
    {
      Bitmap newBmp = new Bitmap(srcImage.Width + 2 * contourWidth, srcImage.Height + 2 * contourWidth);
      if (contourWidth > 0)
      {
        using (Graphics g = Graphics.FromImage(newBmp))
        {
          for (int x = 0; x <= contourWidth * 2; x++)
            for (int y = 0; y <= contourWidth * 2; y++)
              g.DrawImage(srcImage, x, y);
        }
      }
      using (newBmp)
        return newBmp.Clone(new Rectangle(contourWidth, contourWidth, srcImage.Width, srcImage.Height), srcImage.PixelFormat);
    }

    public static Image ResizeImage(Image img, Size newSize)
    {
      Bitmap result = new Bitmap(newSize.Width, newSize.Height);
      result.SetResolution(96, 96);
      using (Graphics graphics = Graphics.FromImage(result))
      {
        graphics.SetHighQuality();
        graphics.DrawImage(img, 0, 0, result.Width, result.Height);
      }
      return result;
    }

    public static Graphics SetHighQuality(this Graphics graphics)
    {
      graphics.CompositingQuality = CompositingQuality.HighQuality;
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.SmoothingMode = SmoothingMode.HighQuality;
      return graphics;
    }

    #endregion
  }
}
