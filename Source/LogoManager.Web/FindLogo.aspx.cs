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
                gvLogos.DataSource = aliases.ToList();
                gvLogos.DataBind();
            }
        }
    }
}