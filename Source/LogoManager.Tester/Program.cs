using System.Collections.Generic;
using System.Drawing;
using MediaPortal.LogoManager.Effects;

namespace MediaPortal.LogoManager.Tester
{
  class Program
  {
    static void Main(string[] args)
    {
      // Define list of effects to be applied to source logo, order matters here. Add only effects that are enabled by configuration.
      List<AbstractEffect> effects = new List<AbstractEffect>();
      effects.Add(new EffectResize { TargetSize = new Size(150, 150), MaxSize = new Size(150, 150) });
      effects.Add(new EffectGlow { Color = Color.Red, Radius = 15 });
      effects.Add(new EffectShadow { Color = Color.Black, Radius = 5, ShadowXOffset = 10, ShadowYOffset = 10 });
      effects.Add(new EffectOuterGlow { Color = Color.Green, Width = 2, Transparency = 0.8f });

      string logoFile = "..\\..\\Example\\1_plus_1_International.png"; // Source file
      string saveFileName = "Modern_1_plus_1_International.png"; // Processed file

      LogoManager lm = new LogoManager { DesignsFolder = "..\\..\\..\\..\\Designs" };
      lm.CreateLogo("Modern", logoFile, saveFileName, effects);
    }
  }
}
