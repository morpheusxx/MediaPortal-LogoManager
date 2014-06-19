using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MediaPortal.LogoManager.Effects;

namespace MediaPortal.LogoManager.Tester
{
  class Program
  {
    // Development service url
    private const string REPOSITORY_URL = "http://channellogos.nocrosshair.de/";

    static void Main(string[] args)
    {
      // Define list of effects to be applied to source logo, order matters here. Add only effects that are enabled by configuration.
      List<AbstractEffect> effects = new List<AbstractEffect>
      {
        new EffectResize {TargetSize = new Size(150, 150), MaxSize = new Size(150, 150)},
        new EffectGlow {Color = Color.Red, Radius = 15},
        new EffectShadow {Color = Color.Black, Radius = 5, ShadowXOffset = 10, ShadowYOffset = 10},
        new EffectOuterGlow {Color = Color.Green, Width = 2, Transparency = 0.8f}
      };

      LogoProcessor processor = new LogoProcessor { DesignsFolder = "..\\..\\..\\..\\Designs" };

      const string design = "Modern";

      // Test 1: From file
      ProcessFile(processor, design, "1_plus_1_International", effects);

      // Test 2: From repository
      ProcessStream(processor, design, "zdf", effects);
    }

    static void ProcessFile(LogoProcessor processor, string design, string channelName, List<AbstractEffect> effects)
    {
      string logoFile = string.Format("..\\..\\Example\\{0}.png", channelName); // Source file
      string saveFileName = string.Format("{0}_{1}.png", design, channelName); // Processed file

      processor.CreateLogo(design, logoFile, saveFileName, effects);
    }

    static void ProcessStream(LogoProcessor processor, string design, string channelName, List<AbstractEffect> effects)
    {
      string saveFileName = string.Format("{0}_{1}.png", design, channelName); // Processed file

      using (var repo = new LogoRepository { RepositoryUrl = REPOSITORY_URL })
      using (Stream logo = repo.Download(channelName).Result)
      {
        processor.CreateLogo(design, logo, saveFileName, effects);
      }
    }
  }
}
