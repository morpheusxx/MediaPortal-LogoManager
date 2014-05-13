using System.Drawing;

namespace MediaPortal.LogoManager.Effects
{
  public class EffectResize : AbstractEffect
  {
    public Size? MinSize { get; set; }
    public Size? MaxSize { get; set; }
    public Size TargetSize { get; set; }

    public Size CalculateNewSize(Size imgSize)
    {
      Size requiredSize = imgSize;
      if (MinSize.HasValue && requiredSize.Width < MinSize.Value.Width || MaxSize.HasValue && requiredSize.Width > MaxSize.Value.Width)
        requiredSize.Width = TargetSize.Width;
      if (MinSize.HasValue && requiredSize.Height < MinSize.Value.Height || MaxSize.HasValue && requiredSize.Height > MaxSize.Value.Height)
        requiredSize.Height = TargetSize.Height;

      float percentWidth = (float)requiredSize.Width / imgSize.Width;
      float percentHeight = (float)requiredSize.Height / imgSize.Height;
      float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
      Size actualRenderSize = new Size((int)(imgSize.Width * percent), (int)(imgSize.Height * percent));
      return actualRenderSize;
    }

    public override bool Apply(Graphics targetGraphics, ref Bitmap logo)
    {
      var actualRenderSize = CalculateNewSize(logo.Size);
      if (actualRenderSize != logo.Size)
      {
        var resized = (Bitmap) GraphicsHelpers.ResizeImage(logo, actualRenderSize);
        logo.Dispose();
        logo = resized;
      }
      return true;
    }
  }
}