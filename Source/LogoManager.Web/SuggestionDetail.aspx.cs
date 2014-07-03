using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChannelManager
{
    public partial class SuggestionDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var suggestion = GetSuggestion(ctx);
                    if (suggestion != null)
                    {
                        EF.Suggestion.SuggestionType suggestWhat;
                        EF.Channel channel = suggestion.GetSuggestedChannel(out suggestWhat);

                        lblSuggestionDate.Text = suggestion.Created.ToString("dd.MM.yyyy hh:mm");
                        lblSuggestWhat.Text = string.Format("Suggestion for a new {0}", suggestWhat);
                        lblChannelName.Text = string.Format("{0} ({1})", channel.Name, (ChannelType)channel.Type);
                        linkChannel.NavigateUrl = channel.Website;
                        linkChannel.Text = channel.Website;
                        linkChannel.Visible = !string.IsNullOrEmpty(channel.Website);
                        lblChannelRegion.Text = channel.RegionCode;
                        lblChannelDescription.Text = channel.Description;
                        imgChannelLogo.ImageUrl = string.Format("/Logos/{0}.png", suggestion.Logos.Any() ? suggestion.Logos.First().Id.ToString() : channel.Logos.First().Id.ToString());

                        string oldAliases = string.Join(", ", channel.Aliases.Where(a => a.Suggestion == null).Select(a => a.Name));
                        string newAliases = string.Join(", ", channel.Aliases.Where(a => a.Suggestion == suggestion).Select(a => a.Name));
                        lblChannelOldAliases.Text = oldAliases;
                        lblChannelNewAliases.Text = newAliases;
                        lblChannelOldAliases.Visible = oldAliases != "";
                        lblChannelNewAliases.Visible = newAliases != "";

                        gvMessages.DataSource = suggestion.Messages;
                        gvMessages.DataBind();
                    }
                }
            }
        }

        protected void btnAddMessage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxNewMessage.Text))
            {
                Guid suggestionId = GetSuggestionIdFromRequest();
                if (suggestionId != Guid.Empty)
                {
                    using (var ctx = new EF.RepositoryContext("LogoDB"))
                    {
                        var suggestion = ctx.Suggestions.Include("Messages").FirstOrDefault(s => s.Id == suggestionId);
                        if (suggestion != null)
                        {
                            suggestion.Messages.Add(new EF.Message() 
                            {
                                Id = Guid.NewGuid(),
                                Created = DateTime.Now,
                                Suggestion = suggestion,
                                Text = tbxNewMessage.Text.Trim()
                            });
                            ctx.ChangeTracker.DetectChanges();
                            ctx.SaveChanges();

                            tbxNewMessage.Text = "";
                            gvMessages.DataSource = suggestion.Messages;
                            gvMessages.DataBind();
                        }
                    }
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            Guid suggestionId = GetSuggestionIdFromRequest();
            if (suggestionId != Guid.Empty)
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var suggestion = ctx.Suggestions
                        .Include("Channels")
                        .Include("Messages")
                        .Include("Aliases")
                        .Include("Logos")
                        .FirstOrDefault(s => s.Id == suggestionId);
                    if (suggestion != null)
                    {
                        foreach (var channel in suggestion.Channels) channel.Suggestion = null;
                        foreach (var logo in suggestion.Logos) logo.Suggestion = null;
                        foreach (var alias in suggestion.Aliases) alias.Suggestion = null;
                        ctx.Messages.RemoveRange(suggestion.Messages);
                        ctx.Suggestions.Remove(suggestion);

                        ctx.ChangeTracker.DetectChanges();
                        ctx.SaveChanges();

                        Response.Redirect("/ListSuggestions.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Visible = false;
                    }
                }
            }
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            Guid suggestionId = GetSuggestionIdFromRequest();
            if (suggestionId != Guid.Empty)
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var suggestion = ctx.Suggestions
                        .Include("Channels")
                        .Include("Messages")
                        .Include("Aliases")
                        .Include("Logos")
                        .FirstOrDefault(s => s.Id == suggestionId);

                    if (suggestion != null)
                    {
                        var logoFilesToDelete = suggestion.Logos.Select(l => l.Id).ToList();

                        ctx.Channels.RemoveRange(suggestion.Channels);
                        ctx.Logos.RemoveRange(suggestion.Logos);
                        ctx.Aliases.RemoveRange(suggestion.Aliases);
                        ctx.Messages.RemoveRange(suggestion.Messages);
                        ctx.Suggestions.Remove(suggestion);

                        ctx.ChangeTracker.DetectChanges();
                        ctx.SaveChanges();

                        logoFilesToDelete.ForEach(l => {
                            var logoFilePath = Path.Combine(Server.MapPath("~/Logos"), l + ".png");
                            if (File.Exists(logoFilePath))
                                File.Delete(logoFilePath);
                        });

                        Response.Redirect("/ListSuggestions.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        this.Visible = false;
                    }
                }
            }
        }

        EF.Suggestion GetSuggestion(EF.RepositoryContext ctx)
        {
            Guid suggestionId = GetSuggestionIdFromRequest();
            if (suggestionId != Guid.Empty)
            {
                return ctx.Suggestions
                    .Include("Channels")
                    .Include("Messages")
                    .Include("Aliases")
                    .Include("Logos").Include("Logos.Channels")
                    .Include("Aliases.Channel").Include("Aliases.Channel.Logos")
                    .FirstOrDefault(s => s.Id == suggestionId);
            }
            return null;
        }

        Guid GetSuggestionIdFromRequest()
        { 
            var suggestionIdString = Request.Params["id"];
            Guid suggestionId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(suggestionIdString))
                Guid.TryParse(suggestionIdString, out suggestionId);
            return suggestionId;
        }
    }
}