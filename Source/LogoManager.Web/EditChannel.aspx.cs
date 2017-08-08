using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChannelManager
{
    public partial class EditChannel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var channel = GetChannel(ctx);
                    if (channel != null)
                    {
                        lblChannelName.Text = channel.Name;
                        tbxChannelDescription.Text = channel.Description;
                        tbxChannelWebsite.Text = channel.Website;

                        var logo = channel.Logos.First(l => l.Suggestion == null);
                        imgChannelLogo.ImageUrl = Thumbnailer.GetThumbFileUrl(logo.Id);
                        imgChannelLogo.NavigateUrl = "/Logos/" + logo.Id + ".png";

                        if (channel.Aliases != null)
                        {
                            btnRemoveAlias.Visible = true;
                            listNewAliases.Items.AddRange(channel.Aliases.Select(a => new ListItem(a.Name, a.Id.ToString())).ToArray());
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ctx = new EF.RepositoryContext("LogoDB"))
                {
                    var channel = GetChannel(ctx);
                    if (channel != null)
                    {
                        channel.Website = tbxChannelWebsite.Text.Trim();
                        channel.Description = tbxChannelDescription.Text.Trim();

                        if (listNewAliases.Items.Count == 0)
                            throw new Exception("A Channel must have at least one Alias!");

                        // delete aliases that are no longer there
                        if (channel.Aliases != null)
                        {
                            foreach (var alias in channel.Aliases.ToList())
                            {
                                if (listNewAliases.Items.FindByText(alias.Name) == null)
                                {
                                    ctx.Aliases.Remove(alias);
                                }
                            }
                        }

                        // add new aliases
                        foreach (ListItem item in listNewAliases.Items)
                        {
                            string newAliasTrimmed = item.Text.Trim();
                            if (!string.IsNullOrEmpty(newAliasTrimmed))
                            {
                                if (!channel.Aliases.Any(a => a.Name == newAliasTrimmed))
                                {
                                    var alias = ctx.Aliases.Create();
                                    alias.Id = Guid.NewGuid();
                                    alias.Name = newAliasTrimmed;
                                    alias.Created = DateTime.Now;
                                    alias.Channel = channel;
                                    channel.Aliases.Add(alias);
                                }
                            }
                        }

                        // new logo
                        if (uploadLogoFile.HasFile)
                        {
                            var repo = ctx.Repositorys.FirstOrDefault();

                            var membership = System.Web.Security.Membership.GetUser();
                            var user = membership != null ? ctx.Users.FirstOrDefault(u => u.Id == (Guid)membership.ProviderUserKey) : null;

                            // check that file is PNG
                            byte[] logoData = uploadLogoFile.FileBytes;
                            using (MemoryStream ms = new MemoryStream(logoData))
                            {
                                using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true, true))
                                {
                                    if (image.RawFormat.Guid != System.Drawing.Imaging.ImageFormat.Png.Guid)
                                        throw new Exception("The supplied Logo file is not a valid PNG image!");

                                    if (string.IsNullOrEmpty(tbxLogoName.Text.Trim()))
                                        throw new Exception("Please give the new Logo an unique name!");

                                    // delete old logo
                                    var oldLogo = channel.Logos.First(l => l.Suggestion == null);
                                    ctx.Logos.Remove(oldLogo);
                                    File.Delete(Thumbnailer.GetThumbFilePath(oldLogo.Id));
                                    File.Delete(Path.Combine(Server.MapPath("~/Logos"), oldLogo.Id + ".png"));

                                    // create new logo
                                    var logo = ctx.Logos.Create();
                                    logo.Id = Guid.NewGuid();
                                    logo.Name = tbxLogoName.Text.Trim();
                                    logo.Origin = tbxLogoOrigin.Text.Trim();
                                    logo.LastModified = DateTime.Now;
                                    logo.Width = image.Width;
                                    logo.Height = image.Height;
                                    logo.SizeInBytes = logoData.Length;
                                    repo.Logos.Add(logo);
                                    logo.Creator = user;
                                    logo.Channels.Add(channel);
                                    File.WriteAllBytes(Path.Combine(Server.MapPath("~/Logos"), logo.Id + ".png"), logoData);
                                    Thumbnailer.CreateLogoThumb(image, logo.Id);

                                    imgChannelLogo.ImageUrl = Thumbnailer.GetThumbFileUrl(logo.Id);
                                    imgChannelLogo.NavigateUrl = "/Logos/" + logo.Id + ".png";
                                }
                            }
                        }

                        ctx.ChangeTracker.DetectChanges();
                        ctx.SaveChanges();
                    }
                }

                lblReturnMessage.Visible = true;
                lblReturnMessage.ForeColor = System.Drawing.Color.Green;
                lblReturnMessage.Text = "Channel successfully saved!";
            }
            catch (Exception ex)
            {
                lblReturnMessage.Visible = true;
                lblReturnMessage.ForeColor = System.Drawing.Color.Red;
                lblReturnMessage.Text = ex.Message;
            }
        }

        protected void btnAddAlias_Click(object sender, EventArgs e)
        {
            string newAlias = tbxChannelAliases.Text.Trim();
            if (!string.IsNullOrWhiteSpace(newAlias))
            {
                listNewAliases.Items.Add(new ListItem(newAlias, newAlias));
                tbxChannelAliases.Text = "";
                btnRemoveAlias.Visible = true;
            }
        }

        protected void btnRemoveAlias_Click(object sender, EventArgs e)
        {
            listNewAliases.Items.Remove(listNewAliases.SelectedItem);
            btnRemoveAlias.Visible = listNewAliases.Items.Count > 0;
        }

        EF.Channel GetChannel(EF.RepositoryContext ctx)
        {
            Guid channelId = GetChannelIdFromRequest();
            if (channelId != Guid.Empty)
            {
                return ctx.Channels
                    .Include("Aliases").Include("Aliases.Suggestion")
                    .Include("Logos").Include("Logos.Suggestion")
                    .FirstOrDefault(c => c.Id == channelId);
            }
            return null;
        }

        Guid GetChannelIdFromRequest()
        {
            var channelIdString = Request.Params["id"];
            Guid channelId = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(channelIdString))
                Guid.TryParse(channelIdString, out channelId);
            return channelId;
        }
    }
}