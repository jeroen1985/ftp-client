using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FTPClient.Src
{
    public abstract class Tree
    {  
        private List<BaseNode> tree = new List<BaseNode>();
        protected string Root;
        protected string Current;
        string slash = "\\";

        public Tree(string path, bool win = true)
        {
            Current = path;
            if (win == false)
                slash = "//";
        }

        public List<BaseNode> GetTree()
        {
            return tree;
        }

        protected void setRootOfTree()
        {
            tree.Add(new NodeWithState() { id = "node_1_r_1_"+ Current, text = Current, parent = "#", state = new Dictionary<string, bool>() { { "opened", true } } });
            if(Current != Root)
            {
                tree.Add(new Node() { id = "node_1_m_0_" + Current + slash +"..", text = "..", parent = "node_1_r_1_" + Current });
            }
        }

        protected void setDirectories(List<string> directories)
        {         
            int count = 1;
            
            foreach (string d in directories)
            {
                tree.Add(new Node() { id = "node_1_m_" + count.ToString() + "_" + Current + slash + d, text = d, parent = "node_1_r_1_"+ Current });
                count++;
            }
        }

        protected void setFiles(List<string> files)
        {        
            int count = 1;

            foreach (string f in files)
            {
                tree.Add(new Node() { id = "node_1_f_" + count.ToString() + "_" + Current + slash +f, text = f, parent = "node_1_r_1_" + Current, icon = "/content/files.png" });
                count++;
            }

        }



    }
}