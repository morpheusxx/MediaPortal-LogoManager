using System;
using System.Collections.Generic;
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
    private readonly HttpClient _httpClient;

    public LogoRepository()
    {
      _httpClient = new HttpClient();
    }

    public string RepositoryUrl { get; set; }

    public Stream Download(string channelName)
    {
      var res = Download(new[] { channelName });
      return res.ContainsKey(channelName) ? res.Values.First().Result : null;
    }

    public Dictionary<string, Task<Stream>> Download(string[] channelName)
    {
      var logoUrls = Lookup(channelName);
      return logoUrls.Result.ToDictionary(logoNameUrl => logoNameUrl.Key, logoNameUrl => DownloadLogoAsync(logoNameUrl.Value));
    }

    public Task<Dictionary<string, string>> Lookup(string channelName)
    {
      return Lookup(new[] { channelName });
    }

    public async Task<Dictionary<string, string>> Lookup(string[] channelNames)
    {
      ChannelManagerClient client = new ChannelManagerClient(new BasicHttpBinding(), new EndpointAddress(string.Format("{0}ChannelManager.svc", RepositoryUrl)));
      return await client.GetLogosAsync(channelNames, string.Empty);
    }

    private async Task<Stream> DownloadLogoAsync(string logoId)
    {
      return await _httpClient.GetStreamAsync(string.Format("{0}Logos/{1}.png", RepositoryUrl, logoId));
    }

    public void Dispose()
    {
      if (_httpClient != null)
        _httpClient.Dispose();
    }
  }
}
