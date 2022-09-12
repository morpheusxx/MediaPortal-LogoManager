using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MediaPortal.LogoManager.Design;
using MediaPortal.LogoManager.Effects;

namespace MediaPortal.LogoManager
{
  /// <summary>
  /// <see cref="LogoProcessor"/> provides methods to process "raw" channel logos into finished logo images.
  /// </summary>
  public class LogoProcessor
  {
    /// <summary>
    /// Gets or sets the folder, where the designs are located.
    /// </summary>
    public string DesignsFolder { get; set; }

    /// <summary>
    /// Creates a logo image for the given <paramref name="logoFileName"/> using the <paramref name="theme"/>. The final result will be saved to <paramref name="saveFileName"/>.
    /// Note:
    /// Exceptions are not caught in this method, so caller needs to handle them accordingly.
    /// </summary>
    /// <param name="theme">Theme.</param>
    /// <param name="logoFileName">Source logo to process.</param>
    /// <param name="saveFileName">Target filename to save.</param>
    /// <returns><c>true</c> if successful</returns>
    public bool CreateLogo(Theme theme, string logoFileName, string saveFileName)
    {
      return CreateLogo(theme.DesignName, logoFileName, saveFileName, theme.Effects);
    }

    /// <summary>
    /// Creates a logo image for the given <paramref name="logoFileName"/> using the <paramref name="designName"/>. The final result will be saved to <paramref name="saveFileName"/>.
    /// A list of <paramref name="effects"/> can be used to modify the look of the final image.
    /// Note:
    /// Exceptions are not caught in this method, so caller needs to handle them accordingly.
    /// </summary>
    /// <param name="designName">Name of design.</param>
    /// <param name="logoFileName">Source logo to process.</param>
    /// <param name="saveFileName">Target filename to save.</param>
    /// <param name="effects">List of effects to apply.</param>
    /// <returns><c>true</c> if successful</returns>
    public bool CreateLogo(string designName, string logoFileName, string saveFileName, List<AbstractEffect> effects)
    {
      // No using here, as the reference can be changed by effects.
      if (!File.Exists(logoFileName))
        return false;

      using (Bitmap logo = (Bitmap)Image.FromFile(logoFileName))
        return CreateLogo(designName, logo, saveFileName, effects);
    }

    /// <summary>
    /// Creates a logo image for the given <paramref name="logoStream"/> using the <paramref name="theme"/>. The final result will be saved to <paramref name="saveFileName"/>.
    /// Note:
    /// Exceptions are not caught in this method, so caller needs to handle them accordingly.
    /// </summary>
    /// <param name="theme">Theme.</param>
    /// <param name="logoStream">Stream with logo bitmap to process.</param>
    /// <param name="saveFileName">Target filename to save.</param>
    /// <returns><c>true</c> if successful</returns>
    public bool CreateLogo(Theme theme, Stream logoStream, string saveFileName)
    {
      return CreateLogo(theme.DesignName, logoStream, saveFileName, theme.Effects);
    }

    /// <summary>
    /// Creates a logo image for the given <paramref name="logoStream"/> using the <paramref name="designName"/>. The final result will be saved to <paramref name="saveFileName"/>.
    /// A list of <paramref name="effects"/> can be used to modify the look of the final image.
    /// Note:
    /// Exceptions are not caught in this method, so caller needs to handle them accordingly.
    /// </summary>
    /// <param name="designName">Name of design.</param>
    /// <param name="logoStream">Stream with logo bitmap to process.</param>
    /// <param name="saveFileName">Target filename to save.</param>
    /// <param name="effects">List of effects to apply.</param>
    /// <returns><c>true</c> if successful</returns>
    public bool CreateLogo(string designName, Stream logoStream, string saveFileName, List<AbstractEffect> effects)
    {
      using (Bitmap logo = (Bitmap)Image.FromStream(logoStream))
        return CreateLogo(designName, logo, saveFileName, effects);
    }

    /// <summary>
    /// Creates a logo image for the given <paramref name="logo"/> using the <paramref name="designName"/>. The final result will be saved to <paramref name="saveFileName"/>.
    /// A list of <paramref name="effects"/> can be used to modify the look of the final image.
    /// Note:
    /// Exceptions are not caught in this method, so caller needs to handle them accordingly.
    /// </summary>
    /// <param name="designName">Name of design.</param>
    /// <param name="logo">Bitmap logo to process.</param>
    /// <param name="saveFileName">Target filename to save.</param>
    /// <param name="effects">List of effects to apply.</param>
    /// <returns><c>true</c> if successful</returns>
    public bool CreateLogo(string designName, Bitmap logo, string saveFileName, List<AbstractEffect> effects)
    {
      string designFolder = Path.Combine(DesignsFolder, designName);
      string bgFile = Path.Combine(designFolder, "background.png");
      string ovFile = Path.Combine(designFolder, "overlay.png");
      if (!File.Exists(bgFile) || !File.Exists(ovFile))
        return false;

      using (Image backgnd = Image.FromFile(bgFile))
      using (Image overlay = Image.FromFile(ovFile))
      {
        using (Graphics graphics = Graphics.FromImage(backgnd).SetHighQuality())
        {
          if (effects != null)
            foreach (var effect in effects)
              effect.Apply(graphics, ref logo);

          graphics.DrawImage(logo, backgnd.Width / 2 - logo.Width / 2, backgnd.Height / 2 - logo.Height / 2, logo.Width, logo.Height);
          graphics.DrawImage(overlay, Point.Empty);
          backgnd.Save(saveFileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        logo.Dispose();
      }
      return true;
    }
  }
}
