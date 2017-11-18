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

    /// <summary>
    /// Indicates that processed logos will not be updated automatically. Although the main purpose of this LogoManager is to provide such auto-update feature, 
    /// this option can be used for "hand created" logo themes to keep the existing pre-processed logos.
    /// It defaults to <c>false</c>, so auto-update is enabled.
    /// </summary>
    [DataMember]
    public bool SkipOnlineUpdate { get; set; }

    [DataMember]
    public string ThemeName { get; set; }

    [DataMember]
    public List<AbstractEffect> Effects { get; set; }
  }
}
