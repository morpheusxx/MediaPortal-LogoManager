using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ChannelManager
{
    public partial class ImportMappingFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            var errors = new List<string>();

            var mappings = new XmlDocument();
            mappings.Load(tbxUrl.Text);

            var xmlTvChannels = new List<Tuple<Dictionary<string, HashSet<string>>, string, Guid>>();
            var xmlRadioChannels = new List<Tuple<Dictionary<string, HashSet<string>>, string, Guid>>();

            var tvChannels = mappings.SelectSingleNode("Mappings/TV");
            foreach (XmlElement tvChannel in tvChannels.SelectNodes("Channel"))
            {
                var xmlTvChannel = GetChannel(tvChannel, 0, ref errors);
                if (xmlTvChannel != null)
                    xmlTvChannels.Add(xmlTvChannel);
            }

            var radioChannels = mappings.SelectSingleNode("Mappings/Radio");
            foreach (XmlElement radioChannel in radioChannels.SelectNodes("Channel"))
            {
                var xmlRadioChannel = GetChannel(radioChannel, 1, ref errors);
                if (xmlRadioChannel != null)
                    xmlRadioChannels.Add(xmlRadioChannel);
            }

            using (var ctx = new EF.RepositoryContext("LogoDB"))
            {
                EF.User currentUser = null;
                var membership = System.Web.Security.Membership.GetUser();
                if (membership != null)
                    currentUser = ctx.Users.FirstOrDefault(u => u.Id == (Guid)membership.ProviderUserKey);

                var repo = ctx.Repositorys.FirstOrDefault();

                var allProviders = ctx.Providers.ToDictionary(p => p.Name, p => p);

                foreach (var importChannel in xmlTvChannels)
                {
                    CreateDbChannel(importChannel, 0, ctx, repo, currentUser, allProviders);
                }
                foreach (var importChannel in xmlRadioChannels)
                {
                    CreateDbChannel(importChannel, 1, ctx, repo, currentUser, allProviders);
                }

                ctx.ChangeTracker.DetectChanges();
                ctx.SaveChanges();
            }

            listErrors.DataSource = errors;
            listErrors.DataBind();
            listErrors.Visible = errors.Count > 0;
        }

        void CreateDbChannel(Tuple<Dictionary<string, HashSet<string>>, string, Guid> importChannel, byte channelType, EF.RepositoryContext ctx, EF.Repository repo, EF.User user, Dictionary<string, EF.Provider> knownProviders)
        {
            var channel = ctx.Channels.Create();
            channel.Id = Guid.NewGuid();
            channel.Type = channelType;
            channel.Name = importChannel.Item1.First().Key;
            channel.RegionCode = ddlChannelRegion.SelectedValue;
            repo.Channels.Add(channel);

            var logo = ctx.Logos.Create();
            logo.Creator = user;
            logo.Id = importChannel.Item3;
            logo.LastModified = DateTime.Now;
            logo.Name = System.IO.Path.GetFileNameWithoutExtension(importChannel.Item2);
            repo.Logos.Add(logo);
            channel.Logos.Add(logo);
            logo.Channels.Add(channel);
            
            foreach (var newAlias in importChannel.Item1)
            {
                var alias = ctx.Aliases.Create();
                alias.Created = DateTime.Now;
                alias.Id = Guid.NewGuid();
                alias.Name = newAlias.Key;
                channel.Aliases.Add(alias);
                foreach (var aProvider in newAlias.Value)
                {
                    EF.Provider provider = null;
                    if (!knownProviders.TryGetValue(aProvider, out provider))
                    {
                        provider = ctx.Providers.Create();
                        provider.Id = Guid.NewGuid();
                        provider.Name = aProvider;
                        repo.Providers.Add(provider);
                        knownProviders.Add(aProvider, provider);
                    }
                    alias.Providers.Add(provider);
                    provider.Aliases.Add(alias);
                }
            }
        }

        Tuple<Dictionary<string, HashSet<string>>, string, Guid> GetChannel(XmlElement channel, int type, ref List<string> errors)
        {
            try
            {
                var file = channel.SelectSingleNode("File");
                if (file != null)
                {
                    var logoFileName = file.InnerText.Trim().Replace("\t", " ");
                    if (!string.IsNullOrEmpty(logoFileName))
                    {
                        byte[] logoData = null;
                        try
                        {
                            logoData = new System.Net.WebClient().DownloadData((type == 0 ? tbxTVLogosBaseUrl.Text : tbxRadioLogosBaseUrl.Text) + logoFileName);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Error downloading logo '{0}' : {1}", logoFileName, ex.Message));
                        }
                        using (MemoryStream ms = new MemoryStream(logoData))
                        {
                            using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true, true))
                            {
                                if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                                {
                                    var aliases = new Dictionary<string, HashSet<string>>();

                                    foreach (XmlElement item in channel.SelectNodes("Item"))
                                    {
                                        string name = item.GetAttribute("Name").Trim();
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            HashSet<string> providerNames = null;

                                            if (!aliases.TryGetValue(name, out providerNames))
                                                providerNames = new HashSet<string>();

                                            foreach (XmlElement provider in item.SelectNodes("Provider"))
                                                providerNames.Add(provider.InnerText.Trim());
                                            foreach (XmlElement satellite in item.SelectNodes("Satellite"))
                                                providerNames.Add(satellite.InnerText.Trim());

                                            aliases[name] = providerNames;
                                        }
                                    }

                                    if (aliases.Count > 0)
                                    {
                                        Guid logoGuid = Guid.NewGuid();
                                        File.WriteAllBytes(Path.Combine(Server.MapPath("~/Logos"), logoGuid + ".png"), logoData);
                                        return new Tuple<Dictionary<string, HashSet<string>>, string, Guid>(aliases, logoFileName, logoGuid);
                                    }
                                    else
                                    {
                                        throw new Exception(string.Format("No aliases defined for logo '{0}'", logoFileName));
                                    }
                                }
                                else
                                {
                                    throw new Exception(string.Format("Logo '{0}' is not a valid PNG", logoFileName));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            return null;
        }
    }
}