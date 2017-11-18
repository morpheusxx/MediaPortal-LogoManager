using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaPortal.LogoManager.Design;
using MediaPortal.LogoManager.Effects;

namespace MediaPortal.LogoManager.Tester
{
  class Program
  {
    // Development service url
    private const string REPOSITORY_URL = "http://channellogos.nocrosshair.de/";
    private const string DESIGN = "Modern-StreamedMP 16x9";
    private static string[] THEMES = new[] { "none", "max" };

    static void Main(string[] args)
    {
      // Create and load themes
      CreateDefaultThemes();
      List<Theme> themes = LoadAllThemes().ToList();

      LogoProcessor processor = new LogoProcessor { DesignsFolder = "..\\..\\..\\..\\Designs" };

      foreach (Theme theme in themes)
      {
        // Test 1: From file
        ProcessFile(processor, "1_plus_1_International", theme);

        // Test 2: From repository
        using (var repo = new LogoRepository { RepositoryUrl = REPOSITORY_URL })
        {
          // Parallel async processing
          var results = repo.Download(new[] { "Das Erste hd", "sat.1", "zdf hd", "animal planet hd", "discovery channel" });
          Task.WaitAll(results.Select(channelAndStream => ProcessStream(processor, channelAndStream.Value, channelAndStream.Key, theme)).ToArray());

          // Synchronous processing
          var stream = repo.Download("zdf");
          ProcessStream(processor, stream, "zdf", theme);
        }
      }
    }

    #region Theme handling

    public static void CreateDefaultThemes()
    {
      // Define list of effects to be applied to source logo, order matters here. Add only effects that are enabled by configuration.
      List<AbstractEffect> effects = new List<AbstractEffect>
      {
        new EffectAutoCrop(),
        new EffectResize {TargetSize = new Size(200, 110), MaxSize = new Size(200, 110)},
        // new EffectGlow {Color = Color.Red, Radius = 15},
        new EffectShadow {Color = Color.FromArgb(200, 0, 0, 0), Radius = 5, ShadowXOffset = 5, ShadowYOffset = 5},
        new EffectOuterGlow {Color = Color.Black, Width = 1, Transparency = 0.8f}
      };

      ThemeHandler themeHandler = new ThemeHandler();
      string theme = string.Format("{0}-{1}", DESIGN, THEMES[0]);
      if (!File.Exists(theme + ".logotheme"))
      {
        Theme themeModern = new Theme { DesignName = DESIGN, ThemeName = THEMES[0] };
        themeHandler.Save(theme, themeModern);
      }
      theme = string.Format("{0}-{1}", DESIGN, THEMES[1]);
      if (!File.Exists(theme + ".logotheme"))
      {
        Theme themeModernMaxEffects = new Theme { DesignName = DESIGN, ThemeName = THEMES[1], Effects = effects };
        themeHandler.Save(theme, themeModernMaxEffects);
      }
    }

    public static IEnumerable<Theme> LoadAllThemes()
    {
      ThemeHandler themeHandler = new ThemeHandler();
      return THEMES.Select(theme => string.Format("{0}-{1}", DESIGN, theme)).Select(themeHandler.Load);
    }

    #endregion

    #region Logo processing

    static void ProcessFile(LogoProcessor processor, string channelName, Theme theme)
    {
      string logoFile = string.Format("..\\..\\Example\\{0}.png", channelName); // Source file
      var saveFileName = BuildFileName(channelName, theme);
      processor.CreateLogo(theme, logoFile, saveFileName);
    }

    static void ProcessStream(LogoProcessor processor, Stream logo, string channelName, Theme theme)
    {
      var saveFileName = BuildFileName(channelName, theme);
      using (logo)
        processor.CreateLogo(theme, logo, saveFileName);
    }

    static async Task ProcessStream(LogoProcessor processor, Task<Stream> logo, string channelName, Theme theme)
    {
      var saveFileName = BuildFileName(channelName, theme);
      Stream logoStream = await logo;
      using (logoStream)
        processor.CreateLogo(theme, logoStream, saveFileName);
    }

    private static string BuildFileName(string channelName, Theme theme)
    {
      return string.Format("{0}-{1}_{2}.png", theme.DesignName, theme.ThemeName, channelName); // Processed file
    }

    #endregion
  }
}
