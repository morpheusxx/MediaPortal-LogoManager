namespace MediaPortal.LogoManager.Effects
{
  public class EffectShadow : EffectGlow
  {
    public float ShadowXOffset
    {
      get { return _drawX; }
      set { _drawX = value; }
    }

    public float ShadowYOffset
    {
      get { return _drawY; }
      set { _drawY = value; }
    }
  }
}