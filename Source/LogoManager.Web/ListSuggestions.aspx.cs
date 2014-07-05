using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChannelManager
{
    public partial class ListSuggestions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var ctx = new EF.RepositoryContext("LogoDB"))
            {
                List<object> suggestions = new List<object>();

                foreach (var suggestion in ctx.Suggestions.Include("Channels").Include("Aliases").Include("Aliases.Channel").Include("Logos").Include("Logos.Channels").Include("User").OrderBy(s => s.LastModified))
                {
                    EF.Suggestion.SuggestionType suggestWhat;
                    EF.Channel channel = suggestion.GetSuggestedChannel(out suggestWhat);
                    
                    string channelName = string.Format("{0} ({1})", channel.Name, channel.Type);

                    suggestions.Add(new 
                    {
                        Id = suggestion.Id,
                        Creation = suggestion.Created.ToString("dd.MM.yyyy hh:mm"), 
                        Type = string.Format("New {0}", suggestWhat),
                        Channel = channelName, 
                        UserName = suggestion.User != null ? suggestion.User.Login : "",
                        Region = channel.RegionCode,
                        Aliases = string.Join(", ", suggestion.Aliases.Select(a => a.Name)),
                        Description = channel.Description,
                        Logo = suggestion.Logos.Any() ? suggestion.Logos.First().Id.ToString() : null
                    });
                }

                gvSuggestions.DataSource = suggestions;
                gvSuggestions.DataBind();
            }
        }
    }
}