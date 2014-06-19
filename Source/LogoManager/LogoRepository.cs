using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using MediaPortal.LogoManager.ChannelManagerService;

namespace MediaPortal.LogoManager
{
  public class LogoRepository : IDisposable
  {
    private HttpClient _httpClient;

    public string RepositoryUrl { get; set; }

    public async Task<Stream> Download(string channelName)
    {
      ChannelManagerClient client = new ChannelManagerClient(new BasicHttpBinding(), new EndpointAddress(string.Format("{0}ChannelManager.svc", RepositoryUrl)));
      var logoUrls = client.GetLogos(new[] { channelName }, string.Empty);
      _httpClient = new HttpClient();
      return await _httpClient.GetStreamAsync(string.Format("{0}Logos/{1}.png", RepositoryUrl, logoUrls.Values.FirstOrDefault()));
    }

    public void Dispose()
    {
      if (_httpClient != null)
        _httpClient.Dispose();
    }
  }
}
