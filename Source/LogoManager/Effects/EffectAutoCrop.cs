using System.Drawing;
using System.Runtime.Serialization;

namespace MediaPortal.LogoManager.Effects
{
  [DataContract]
  public class EffectAutoCrop : AbstractEffect
  {
    public override bool Apply(Graphics targetGraphics, ref Bitmap logo)
    {
      var cropped  = GraphicsHelpers.TrimBitmap(logo);
      logo.Dispose();
      logo = cropped;
      return true;
    }
  }
}
