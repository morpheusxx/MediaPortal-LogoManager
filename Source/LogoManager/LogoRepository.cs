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
    // Empty consts for no filtering by region
    private const string NO_REGION = "";

    private readonly HttpClient _httpClient;

    public LogoRepository()
    {
      _httpClient = new HttpClient();
    }

    public string RepositoryUrl { get; set; }

    public Stream Download(string channelName, string regionCode = NO_REGION)
    {
      var res = Download(new[] { channelName }, regionCode);
      return res.ContainsKey(channelName) ? res.Values.First().Result : null;
    }

    public Dictionary<string, Task<Stream>> Download(string[] channelName, string regionCode = NO_REGION)
    {
      var logoUrls = Lookup(channelName, regionCode);
      return logoUrls.Result.ToDictionary(logoNameUrl => logoNameUrl.Key, logoNameUrl => DownloadLogoAsync(logoNameUrl.Value));
    }

    public Task<Dictionary<string, string>> Lookup(string channelName, string regionCode = NO_REGION)
    {
      return Lookup(new[] { channelName }, regionCode);
    }

    public async Task<Dictionary<string, string>> Lookup(string[] channelNames, string regionCode = NO_REGION)
    {
      ChannelManagerClient client = new ChannelManagerClient(new BasicHttpBinding(), new EndpointAddress(string.Format("{0}ChannelManager.svc", RepositoryUrl)));
      return await client.GetLogosAsync(channelNames, regionCode);
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
