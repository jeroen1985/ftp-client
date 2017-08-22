using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTPClient.Src.EventLog
{
    public enum LogEvents
    {
        Connecting,
        Connected,
        Listing,
        StartTransferFile,
        TransferingFile,
        TransferCompleted

    }

    public class EventLogResponse
    {
        public bool Connected { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public List<LogEvents> EventsLog { get; set; } = new List<LogEvents>();
    }
}