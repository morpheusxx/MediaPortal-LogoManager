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

                foreach (var suggestion in ctx.Suggestions
                    .Include("Channels")
                    .Include("User")
                    .Include("Aliases").Include("Aliases.Channel")
                    .Include("Logos").Include("Logos.Channels")
                    .OrderByDescending(s => s.LastModified))
                {
                    EF.Channel channel = suggestion.GetSuggestedChannel(out EF.Suggestion.SuggestionType? suggestWhat);
                    
                    suggestions.Add(new 
                    {
                        Id = suggestion.Id,
                        Creation = suggestion.Created.ToString("dd.MM.yyyy hh:mm"), 
                        Type = suggestWhat != null ? $"New {suggestWhat}" : "Invalid",
                        Channel = channel != null ? $"{channel.Name} ({channel.Type})" : "", 
                        UserName = suggestion.User != null ? suggestion.User.Login : "",
                        Region = channel?.RegionCode,
                        Aliases = string.Join(", ", suggestion.Aliases.Select(a => a.Name)),
                        Description = channel?.Description,
                        Logo = suggestion.Logos.FirstOrDefault()
                    });
                }

                gvSuggestions.DataSource = suggestions;
                gvSuggestions.DataBind();
            }
        }
    }
}