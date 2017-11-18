using System.Drawing;
using System.Runtime.Serialization;

namespace MediaPortal.LogoManager.Effects
{
  [DataContract]
  public class EffectGlow : AbstractEffect
  {
    protected float _drawX = 0.0f;
    protected float _drawY = 0.0f;
    [DataMember]
    public Color Color { get; set; }
    [DataMember]
    public int Radius { get; set; }
    [DataMember]
    public bool UseThreshold { get; set; }
    [DataMember]
    public float Threshold { get; set; }

    public EffectGlow()
    {
      Radius = 0;
      Color = Color.White;
      Threshold = 1f;
    }

    public override bool Apply(Graphics targetGraphics, ref Bitmap logo)
    {
      if (!UseThreshold)
        return ApplyGlow(targetGraphics, ref logo);

      float imgBrightness = logo.BitmapBrightness();
      if (ActivePart == ActivePart.Above && imgBrightness > Threshold || ActivePart == ActivePart.Below && imgBrightness < Threshold)
        return ApplyGlow(targetGraphics, ref logo);

      return true;
    }

    public virtual bool ApplyGlow(Graphics targetGraphics, ref Bitmap logo)
    {
      using (Bitmap squareLogo = new Bitmap((int)targetGraphics.VisibleClipBounds.Width, (int)targetGraphics.VisibleClipBounds.Height))
      {
        squareLogo.MakeTransparent();
        using (Graphics g = Graphics.FromImage(squareLogo))
          g.DrawImage(logo, squareLogo.Width / 2 - logo.Width / 2, squareLogo.Height / 2 - logo.Height / 2, logo.Width, logo.Height);

        squareLogo.ChangeColor(Color);
        squareLogo.FastBlur(Radius, Color);
        squareLogo.FastBlur(Radius, Color);
        squareLogo.SetResolution(targetGraphics.DpiX, targetGraphics.DpiY);
        targetGraphics.DrawImage(squareLogo, _drawX, _drawY);
      }
      return true;
    }
  };
}