using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FTPClient.Src
{
    public class LocalTree : Tree
    {
        private DriveInfo di;
        private DirectoryInfo dirInfo;

        public LocalTree(string path) : base (path)
        {
            Root = @"C:\";            
            getRootTreeInfo();            
            setRootOfTree();
            setDirectories(getDirectories());
            setFiles(getFiles());
        }

        private void getRootTreeInfo()
        {
            di = new DriveInfo(Root);
            dirInfo = new DirectoryInfo(Current);
        }

        private List<string> getDirectories()
        {
            List<string> directories = new List<string>();

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();

            foreach (DirectoryInfo d in dirInfos)
            {
                directories.Add(d.Name);
            }

            return directories;
        }

        private List<string> getFiles()
        {
            List<string> files = new List<string>();

            FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (FileInfo f in fileNames)
            {
                files.Add(f.Name);
            }

            return files;

        }

    }
}