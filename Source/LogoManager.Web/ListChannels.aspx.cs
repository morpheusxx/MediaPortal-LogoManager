using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChannelManager
{
    public partial class ListChannels : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            rblChannelType.DataSource = Enum.GetNames(typeof(ChannelType)).Select((value, index) => new { value, index }).ToDictionary(pair => pair.value, pair => pair.index);
            rblChannelType.DataBind();
            rblChannelType.SelectedIndex = 0;

            using (var ctx = new EF.RepositoryContext("LogoDB"))
            {
                var list = ctx.Channels.Select(c => c.RegionCode).Distinct().OrderBy(r => r).ToList();
                ddRegion.DataSource = list;
                ddRegion.DataBind();
            }
        }

        protected void btnShowChannels_Click(object sender, EventArgs e)
        {
            string region = ddRegion.SelectedValue;
            var type = (ChannelType)byte.Parse(rblChannelType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(region))
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var list = ctx.Channels.Include("Logos").Include("Logos.Suggestion").Include("Aliases").Include("Aliases.Providers").Where(c => c.Suggestion == null && c.RegionCode == region && c.Type == type).OrderBy(c => c.Name).ToList();
                    gvChannels.DataSource = list;
                    gvChannels.DataBind();
                }
            }
        }

        protected void gvChannels_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var repeater = (Repeater)e.Row.FindControl("repeatAliases");
                repeater.DataSource = (e.Row.DataItem as EF.Channel).Aliases;
                repeater.DataBind();
            }
        }
    }
}