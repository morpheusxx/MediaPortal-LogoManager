using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChannelManager
{
    public class ChannelManager : IChannelManager
    {
        public Dictionary<string, string> GetLogos(List<string> channelsNames, ChannelType type, string regionCode)
        {
            var result = new Dictionary<string, string>();
            using (var ctx = new EF.RepositoryContext("LogoDB"))
            {
                foreach (var channelName in channelsNames)
                {
                    var aliases = ctx.Aliases.Include("Channel").Include("Channel.Logos").Include("Channel.Logos.Suggestion").Where(a => a.Name == channelName && a.Channel.Type == type);
                    if (aliases.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(regionCode))
                        {
                            // find best matching by RegionCode
                            var matchedByRegion = aliases.FirstOrDefault(a => a.Channel.RegionCode == regionCode);
                            if (matchedByRegion != null)
                            {
                                result.Add(channelName, matchedByRegion.Channel.Logos.First(l => l.Suggestion == null).Id.ToString());
                                continue;
                            }
                        }
                        // simply use first one
                        result.Add(channelName, aliases.First().Channel.Logos.First(l => l.Suggestion == null).Id.ToString());
                    }
                }
            }
            return result;
        }
    }
}
