using System.Runtime.Serialization;

namespace MediaPortal.LogoManager.Effects
{
  [DataContract]
  public class EffectShadow : EffectGlow
  {
    [DataMember]
    public float ShadowXOffset
    {
      get { return _drawX; }
      set { _drawX = value; }
    }

    [DataMember]
    public float ShadowYOffset
    {
      get { return _drawY; }
      set { _drawY = value; }
    }
  }
}