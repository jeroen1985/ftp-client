using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTPClient.Src
{
    public class SFTPClientConnector
    {
        public static void SetConnectionInSession(SFTPConnectionString sFTPConnectionString)
        {
            if (HttpContext.Current.Session["Connected"] == null)
            {
                HttpContext.Current.Session["Connected"] = true;
                HttpContext.Current.Session["Host"] = sFTPConnectionString.Host;
                HttpContext.Current.Session["Port"] = sFTPConnectionString.Port;
                HttpContext.Current.Session["UserName"] = sFTPConnectionString.Username;
                HttpContext.Current.Session["Password"] = sFTPConnectionString.Password;
            }
        }

        public static SftpClient ConnectToRemote()
        {
            var connectionInfo = new ConnectionInfo((string)HttpContext.Current.Session["Host"],
                                       22,
                                       (string)HttpContext.Current.Session["UserName"],
                                       new PasswordAuthenticationMethod((string)HttpContext.Current.Session["UserName"], (string)HttpContext.Current.Session["Password"]));

            SftpClient client = new SftpClient(connectionInfo);
            client.Connect();
            return client;
            
        }        
    }
}