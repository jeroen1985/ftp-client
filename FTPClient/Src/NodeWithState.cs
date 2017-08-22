using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTPClient.Src
{
    public class NodeWithState : BaseNode
    {
        public Dictionary<string, bool> state = new Dictionary<string, bool>();
    }
}