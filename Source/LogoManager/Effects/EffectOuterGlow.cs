using System.Drawing;
using System.Runtime.Serialization;

namespace MediaPortal.LogoManager.Effects
{
  [DataContract]
  public class EffectOuterGlow : EffectGlow
  {
    [DataMember]
    public int Width { get; set; }
    [DataMember]
    public float? Transparency { get; set; }

    public override bool ApplyGlow(Graphics targetGraphics, ref Bitmap logo)
    {
      using (Bitmap squareLogo = new Bitmap((int)targetGraphics.VisibleClipBounds.Width, (int)targetGraphics.VisibleClipBounds.Height))
      {
        squareLogo.MakeTransparent();
        using (Graphics g = Graphics.FromImage(squareLogo))
          g.DrawImage(logo, squareLogo.Width / 2 - logo.Width / 2, squareLogo.Height / 2 - logo.Height / 2, logo.Width, logo.Height);

        squareLogo.ChangeColor(Color);
        using (var conturedLogo = GraphicsHelpers.Contour(squareLogo, Width))
        {
          if (Width > 0)
            conturedLogo.FastBlur(1, Color); // smooth outerglow
          if (Transparency.HasValue)
            conturedLogo.ChangeOpacity(Transparency.Value);
          conturedLogo.SetResolution(96, 96);
          targetGraphics.DrawImage(conturedLogo, 0, 0);
        }
      }
      return true;
    }
  }
}