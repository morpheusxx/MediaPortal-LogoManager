using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
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
                var list = ctx.Channels.Where(c => c.Suggestion == null).Select(c => c.RegionCode).Distinct().OrderBy(r => r).ToList();
                ddRegion.DataSource = list;
                ddRegion.DataBind();
            }
        }

        protected void btnShowChannels_Click(object sender, EventArgs e)
        {
            BindChannelsGrid();
        }

        private void BindChannelsGrid()
        {
            string region = ddRegion.SelectedValue;
            var type = (ChannelType)byte.Parse(rblChannelType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(region))
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var list = ctx.Channels.Include("Logos").Include("Logos.Suggestion").Include("Aliases").Include("Aliases.Providers").Where(c => c.Suggestion == null && c.RegionCode == region && c.Type == type).OrderBy(c => c.Name).ToList();

                    var results = new List<ChannelDto>();
                    foreach (var c in list)
                    {
                        var firstLogo = c.Logos.First(l => l.Suggestion == null);
                        var channel = new ChannelDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            ChannelLogoThumb = Thumbnailer.GetThumbFileUrl(firstLogo.Id),
                            ChannelLogoUrl = "/Logos/" + firstLogo.Id + ".png",
                            Aliases = c.Aliases.Select(a => 
                                (object)new
                                {
                                    a.Name,
                                    Providers = string.Format("({0})", string.Join(", ", a.Providers.Select(p => p.Name))),
                                    ProvidersCount = a.Providers.Count
                                }).ToList(), 
                            Width = firstLogo.Width,
                            Height = firstLogo.Height,
                            SizeKb = (firstLogo.SizeInBytes / 1024).ToString("F1"),
                            ChannelDescription = c.Description,
                            Website = c.Website ?? string.Empty
                        };
                        results.Add(channel);
                    }

                    gvChannels.DataSource = results;
                    gvChannels.DataBind();
                }
            }
        }

        protected void gvChannels_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var repeater = (Repeater)e.Row.FindControl("repeatAliases");
                repeater.DataSource = (e.Row.DataItem as ChannelDto).Aliases;
                repeater.DataBind();
            }
        }

        protected void gvChannels_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteChannel")
            {
                var channelId = new Guid((string)e.CommandArgument);
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var channel = ctx.Channels
                        .Include("Aliases").Include("Aliases.Suggestion")
                        .Include("Logos").Include("Logos.Suggestion")
                        .FirstOrDefault(c => c.Id == channelId);
                    if (channel != null)
                    {
                        foreach(var alias in channel.Aliases.ToList())
                        {
                            if (alias.Suggestion != null)
                                ctx.Suggestions.Remove(alias.Suggestion);
                            ctx.Aliases.Remove(alias);
                        }

                        foreach(var logo in channel.Logos.ToList())
                        {
                            if (logo.Suggestion != null)
                                ctx.Suggestions.Remove(logo.Suggestion);
                            ctx.Logos.Remove(logo);

                            File.Delete(Thumbnailer.GetThumbFilePath(logo.Id));
                            File.Delete(Path.Combine(Server.MapPath("~/Logos"), logo.Id + ".png"));
                        }

                        ctx.Channels.Remove(channel);

                        ctx.ChangeTracker.DetectChanges();
                        ctx.SaveChanges();
                    }
                }

                BindChannelsGrid();
            }
        }

        protected void gvChannels_DataBinding(object sender, EventArgs e)
        {
            var canDelete = Roles.IsUserInRole(RoleProvider.Roles.Administrator.ToString()) || Roles.IsUserInRole(RoleProvider.Roles.Maintainer.ToString());
            (sender as GridView).Columns[0].Visible = canDelete;
        }
    }
}
