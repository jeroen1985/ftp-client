using FTPClient.Src.EventLog;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FTPClient.Src
{
    public class RemoteTree : Tree
    {
        private SftpClient client;

        private IEnumerable<SftpFile> allDirectoriesAndFiles;

        private List<string> directories = new List<string>();

        private List<string> files = new List<string>();

        public RemoteTree(string path) : base (path, false)
        {
            Root = "/";
            getRootTreeInfo(path);
            setRootOfTree();
            getDirectoriesAndFiles();
            directories.Sort();
            setDirectories(directories);
            files.Sort();
            setFiles(files);
        }

        ~RemoteTree()
        {
            client.Dispose();
        }

        private void getRootTreeInfo(string path)
        {
            client = SFTPClientConnector.ConnectToRemote();
            allDirectoriesAndFiles = client.ListDirectory(path);
        }

        private void getDirectoriesAndFiles()
        {
            EventLogWriter.SetEvent(LogEvents.Listing);

            foreach (SftpFile DirOrFile in allDirectoriesAndFiles)
            {
                if (DirOrFile.IsDirectory)
                {
                    if (DirOrFile.Name == ".." || DirOrFile.Name == ".")
                        continue;
                    directories.Add(DirOrFile.Name);
                }
                else
                {
                    files.Add(DirOrFile.Name);
                }
            }
 
        }  

    }
}