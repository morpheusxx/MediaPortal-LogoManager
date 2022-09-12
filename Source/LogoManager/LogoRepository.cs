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
    private readonly ChannelManagerClient _client;
    private readonly string _repositoryUrl;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(20);

    public LogoRepository(string repositoryUrl)
    {
      _repositoryUrl = repositoryUrl;
      _httpClient = new HttpClient();

      var binding = new BasicHttpBinding
      {
        OpenTimeout = _timeout,
        SendTimeout = _timeout,
      };
      _client = new ChannelManagerClient(binding, new EndpointAddress(string.Format("{0}ChannelManager.svc", repositoryUrl)));
    }

    public Stream Download(string channelName, ChannelType channelType = ChannelType.Tv, string regionCode = NO_REGION)
    {
      var res = Download(new[] { channelName }, channelType, regionCode);
      return res.ContainsKey(channelName) ? res.Values.First().Result : null;
    }

    public Dictionary<string, Task<Stream>> Download(string[] channelNames, ChannelType channelType = ChannelType.Tv, string regionCode = NO_REGION)
    {
      var logoUrls = Lookup(channelNames, channelType, regionCode);
      return logoUrls.Result.ToDictionary(logoNameUrl => logoNameUrl.Key, logoNameUrl => DownloadLogoAsync(logoNameUrl.Value));
    }

    public async Task<Stream> DownloadAsync(string channelName, ChannelType channelType = ChannelType.Tv, string regionCode = NO_REGION)
    {
      var res = await DownloadAsync(new[] { channelName }, channelType, regionCode);
      return res.ContainsKey(channelName) ? res.Values.First() : null;
    }

    public async Task<Dictionary<string, Stream>> DownloadAsync(string[] channelNames, ChannelType channelType = ChannelType.Tv, string regionCode = NO_REGION)
    {
      var logoUrls = await Lookup(channelNames, channelType, regionCode).ConfigureAwait(false);
      var result = new Dictionary<string, Stream>();
      var tasks = logoUrls.Select(async map =>
      {
        var bytes = await _httpClient.GetByteArrayAsync(string.Format("{0}Logos/{1}.png", _repositoryUrl, map.Value)).ConfigureAwait(false);
        // Using a MemoryStream here do disconnect the returned stream from the original network stream to avoid blocking of calls
        result[map.Key] = new MemoryStream(bytes);
      }).ToArray();

      Task.WaitAll(tasks);
      return result;
    }

    public Task<Dictionary<string, string>> Lookup(string channelName, ChannelType channelType = ChannelType.Tv, string regionCode = NO_REGION)
    {
      return Lookup(new[] { channelName }, channelType, regionCode);
    }

    public async Task<Dictionary<string, string>> Lookup(string[] channelNames, ChannelType channelType = ChannelType.Tv, string regionCode = NO_REGION)
    {
      return await _client.GetLogosAsync(channelNames, channelType, regionCode);
    }

    private async Task<Stream> DownloadLogoAsync(string logoId)
    {
      return await _httpClient.GetStreamAsync(string.Format("{0}Logos/{1}.png", _repositoryUrl, logoId)).ConfigureAwait(false);
    }

    public void Dispose()
    {
      _httpClient.Dispose();
      _client.Close();
    }
  }
}
