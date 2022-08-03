using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChannelManager
{
    public partial class FindLogo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            rblChannelType.DataSource = Enum.GetNames(typeof(ChannelType)).Select((value, index) => new { value, index }).ToDictionary(pair => pair.value, pair => pair.index);
            rblChannelType.DataBind();
            rblChannelType.SelectedIndex = 0;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            IQueryable<EF.Alias> aliases = null;
            using (var ctx = new EF.RepositoryContext("LogoDB"))
            {
                var type = (ChannelType)byte.Parse(rblChannelType.SelectedValue);

                aliases = ctx.Aliases.Include("Channel").Include("Channel.Logos").Include("Channel.Logos.Suggestion").Include("Providers").Where(a => a.Name == tbxName.Text && a.Channel.Type == type);
                if (!string.IsNullOrWhiteSpace(tbxRegion.Text) && aliases.Any())
                {
                    // limit results to be matching by RegionCode
                    aliases = aliases.Where(a => a.Channel.RegionCode == tbxRegion.Text);
                }

                List<LogoDto> results = new List<LogoDto>();
                foreach (var a in aliases)
                {
                    // This could be much simpler by Linq query, but mono throws many different exceptions when trying :(
                    var firstLogo = a.Channel.Logos.First(l => l.Suggestion == null);
                    var logo = new LogoDto
                    {
                        Name = a.Name,
                        ProviderNames = string.Join(", ", a.Providers.Select(p => p.Name)),
                        ChannelLogoThumb = Thumbnailer.GetThumbFileUrl(firstLogo.Id),
                        ChannelLogoUrl = "/Logos/" + firstLogo.Id + ".png",
                        Width = firstLogo.Width,
                        Height = firstLogo.Height,
                        SizeKb = (firstLogo.SizeInBytes / 1024).ToString("F1"),
                        ChannelName = a.Channel.Name,
                        ChannelRegionCode = a.Channel.RegionCode,
                        ChannelDescription = a.Channel.Description
                    };
                    results.Add(logo);
                }

                gvLogos.DataSource = results;
                gvLogos.DataBind();
            }
        }
    }
}
