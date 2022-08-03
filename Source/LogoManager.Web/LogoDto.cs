using System;
using System.Collections.Generic;

namespace ChannelManager
{
    public class LogoDto
    {
        public string ProviderNames { get; set; }
        public string ChannelLogoThumb { get; set; }
        public string ChannelLogoUrl { get; set; }
        public string SizeKb { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string ChannelName { get; set; }
        public string ChannelRegionCode { get; set; }
        public string ChannelDescription { get; set; }
    }

    public class ChannelDto
    {
        public string ChannelLogoThumb { get; set; }
        public string ChannelLogoUrl { get; set; }
        public string SizeKb { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string ChannelName { get; set; }
        public string ChannelRegionCode { get; set; }
        public string ChannelDescription { get; set; }
        public Guid Id { get; set; }
        public List<object> Aliases { get; set; }
        public string Website { get; set; }
    }
}
