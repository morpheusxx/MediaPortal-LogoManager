using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChannelManager.EF
{
    public partial class Suggestion 
    {
        public enum SuggestionType { Channel, Logo, Alias };

        public Channel GetSuggestedChannel(out SuggestionType? suggestionType)
        {
            if (Channels.Count > 0)
            {
                suggestionType = SuggestionType.Channel;
                return Channels.First();
            }
            else if (Logos.Count > 0)
            {
                suggestionType = SuggestionType.Logo;
                return Logos.First().Channels.First();
            }
            else if (Aliases.Count > 0)
            {
                suggestionType = SuggestionType.Alias;
                return Aliases.First().Channel;
            }

            suggestionType = null;
            return null;
        }
    }
}