using System;
using System.ServiceModel.Description;
using System.Web;

namespace ChannelManager
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // The default connection limit is 2 in .net on most platforms! This means downloading two files will block all other WebRequests.
            System.Net.ServicePointManager.DefaultConnectionLimit = 500;
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}
