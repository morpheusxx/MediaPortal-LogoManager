using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChannelManager
{
    public partial class ListProviders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            using (var ctx = new EF.RepositoryContext("LogoDB"))
            {
                var list = ctx.Providers.ToList();
                ddProviders.DataSource = list;
                ddProviders.DataBind();
            }
        }

        protected void btnShowAliases_Click(object sender, EventArgs e)
        {
            using (var ctx = new EF.RepositoryContext("LogoDB"))
            {
                var providerGuid = Guid.Parse(ddProviders.SelectedValue);
                var aliases = ctx.Aliases.Include("Channel").Where(a => a.Providers.Any(p => p.Id == providerGuid)).OrderBy(a => a.Name).ToList();
                gvAliases.DataSource = aliases;
                gvAliases.DataBind();
            }
        }
    }
}