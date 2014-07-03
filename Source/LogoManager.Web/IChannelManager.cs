using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChannelManager
{
    [ServiceContract]
    public interface IChannelManager
    {
        [OperationContract]
        Dictionary<string, string> GetLogos(List<string> channelsNames, ChannelType type, string regionCode);
    }
}
