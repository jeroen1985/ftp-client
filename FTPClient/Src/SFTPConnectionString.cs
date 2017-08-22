using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTPClient.Src
{
    public class SFTPConnectionString
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}