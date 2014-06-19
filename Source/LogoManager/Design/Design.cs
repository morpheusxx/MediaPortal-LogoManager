using System.Runtime.Serialization;

namespace MediaPortal.LogoManager.Design
{
  [DataContract]
  public abstract class Design
  {
    [DataMember]
    public string DesignName { get; set; }
  }
}
