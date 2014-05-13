using System.Drawing;

namespace MediaPortal.LogoManager.Effects
{
  public enum ActivePart { Undefined, Above, Below };

  /// <summary>
  /// Base class for all effects.
  /// </summary>
  public abstract class AbstractEffect
  {
    /// <summary>
    /// Controls the render order of the effect.
    /// </summary>
    public ActivePart ActivePart { get; set; }

    /// <summary>
    /// Applies the effect to the given <paramref name="logo"/>. The result will be drawn onto the given <paramref name="targetGraphics"/>.
    /// Note:
    /// Effects might change the <paramref name="logo"/>, i.e. resize them if required. The original value will be disposed.
    /// </summary>
    /// <param name="targetGraphics">Graphics to draw to.</param>
    /// <param name="logo">Image to process.</param>
    /// <returns><c>true</c>>if successful.</returns>
    public abstract bool Apply(Graphics targetGraphics, ref Bitmap logo);
  }
}
