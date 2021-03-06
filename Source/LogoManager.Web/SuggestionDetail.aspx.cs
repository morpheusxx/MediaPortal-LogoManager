﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChannelManager
{
    public partial class SuggestionDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var canApproveOrDecline = Roles.IsUserInRole(RoleProvider.Roles.Administrator.ToString()) || Roles.IsUserInRole(RoleProvider.Roles.Maintainer.ToString());
            btnApprove.Visible = canApproveOrDecline;
            btnDecline.Visible = canApproveOrDecline;

            if (!IsPostBack)
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var suggestion = GetSuggestion(ctx);
                    if (suggestion != null)
                    {
                        EF.Channel channel = suggestion.GetSuggestedChannel(out EF.Suggestion.SuggestionType? suggestWhat);

                        lblSuggestionDate.Text = suggestion.Created.ToString("dd.MM.yyyy hh:mm");
                        lblAuthor.Text = suggestion.User != null ? suggestion.User.Login : "";
                        lblSuggestWhat.Text = suggestWhat != null ? $"Suggestion for a new {suggestWhat}" : "Invalid Suggestion";
                        lblChannelName.Text = channel != null ? $"{channel.Name} ({channel.Type})" : "";
                        linkChannel.NavigateUrl = channel?.Website;
                        linkChannel.Text = channel?.Website;
                        linkChannel.Visible = !string.IsNullOrEmpty(channel?.Website);
                        lblChannelRegion.Text = channel?.RegionCode;
                        lblChannelDescription.Text = channel?.Description;

                        var logoNew = suggestion.Logos.FirstOrDefault();
                        if (logoNew != null)
                        {
                            imgChannelLogoNew.Visible = true;
                            imgChannelLogoNew.ImageUrl = string.Format("/Logos/{0}.png", logoNew.Id);
                            imgChannelLogoNew.NavigateUrl = string.Format("/Logos/{0}.png", logoNew.Id);
                            lblLogoMetadataNew.Text = string.Format("{0}x{1}, {2:F1}KB", logoNew.Width, logoNew.Height, logoNew.SizeInBytes / 1024.0);
                            lblLogoMetadataNew.Visible = true;
                            lblNewLogo.Visible = true;
                        }

                        if (channel != null)
                        {
                            var logoOld = channel.Logos.FirstOrDefault(l => l.Suggestion == null);
                            if (logoOld != null)
                            {
                                imgChannelLogoOld.Visible = true;
                                imgChannelLogoOld.ImageUrl = string.Format("/Logos/{0}.png", logoOld.Id);
                                imgChannelLogoOld.NavigateUrl = string.Format("/Logos/{0}.png", logoOld.Id);
                                lblLogoMetadataOld.Text = string.Format("{0}x{1}, {2:F1}KB", logoOld.Width, logoOld.Height, logoOld.SizeInBytes / 1024.0);
                                lblLogoMetadataOld.Visible = true;
                                if (logoNew != null) lblOldLogo.Visible = true;
                            }

                            string oldAliases = string.Join(", ", channel.Aliases.Where(a => a.Suggestion == null).Select(a => a.Name));
                            string newAliases = string.Join(", ", channel.Aliases.Where(a => a.Suggestion == suggestion).Select(a => a.Name));
                            lblChannelOldAliases.Text = oldAliases;
                            lblChannelNewAliases.Text = newAliases;
                            lblChannelOldAliases.Visible = oldAliases != "";
                            lblChannelNewAliases.Visible = newAliases != "";
                        }

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
                        .Include("Logos").Include("Logos.Channels").Include("Logos.Channels.Logos")
                        .FirstOrDefault(s => s.Id == suggestionId);
                    if (suggestion != null)
                    {
                        foreach (var channel in suggestion.Channels) channel.Suggestion = null;
                        if (suggestion.Logos.Any())
                        {
                            // delete the old logo (DB object and files) on the channel when suggestion was new logo
                            foreach(var logo in suggestion.Logos.First().Channels.First().Logos.Where(l => l.Suggestion == null).ToList())
                            {
                                logo.Repository = null;
                                logo.Creator = null;
                                ctx.Logos.Remove(logo);
                                File.Delete(Thumbnailer.GetThumbFilePath(logo.Id));
                                File.Delete(Path.Combine(Server.MapPath("~/Logos"), logo.Id + ".png"));
                            }
                        }
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
                            var logoThumbPath = Thumbnailer.GetThumbFilePath(l);
                            if (File.Exists(logoThumbPath))
                                File.Delete(logoThumbPath);
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
                    .Include("User")
                    .Include("Messages")
                    .Include("Aliases")
                    .Include("Aliases.Channel")
                    .Include("Aliases.Channel.Logos")
                    .Include("Aliases.Channel.Aliases")
                    .Include("Logos").Include("Logos.Channels").Include("Logos.Channels.Logos")
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