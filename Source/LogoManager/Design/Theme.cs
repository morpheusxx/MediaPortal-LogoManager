using System.Collections.Generic;
using System.Runtime.Serialization;
using MediaPortal.LogoManager.Effects;

namespace MediaPortal.LogoManager.Design
{
  [DataContract]
  [KnownType(typeof(EffectGlow))]
  [KnownType(typeof(EffectOuterGlow))]
  [KnownType(typeof(EffectAutoCrop))]
  [KnownType(typeof(EffectResize))]
  [KnownType(typeof(EffectShadow))]
  public class Theme : Design
  {
    public Theme()
    {
      Effects = new List<AbstractEffect>();
    }

    [DataMember]
    public string ThemeName { get; set; }

    [DataMember]
    public List<AbstractEffect> Effects { get; set; }
  }
}
