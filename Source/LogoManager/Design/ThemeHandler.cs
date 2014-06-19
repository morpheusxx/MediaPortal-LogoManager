using System.IO;
using System.Runtime.Serialization;

namespace MediaPortal.LogoManager.Design
{
  public class Settings<T>
  {
    private readonly DataContractSerializer _serializer = new DataContractSerializer(typeof(T));

    public T Load(string fileName)
    {
      using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        return (T)_serializer.ReadObject(stream);
    }

    public void Save(string fileName, T setting)
    {
      using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        _serializer.WriteObject(stream, setting);
    }
  }

  public class ThemeHandler : Settings<Theme>
  {
  }
}
