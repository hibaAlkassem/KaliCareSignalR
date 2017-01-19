using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace KaliCareSignalR
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //string connection = "data source=MOUNIRSD;initial catalog=kali;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework&quot";
            //SqlDependency.Start(connection);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //string connection = "data source=MOUNIRSD;initial catalog=kali;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework&quot";
            //SqlDependency.Stop(connection);
        }
    }
}