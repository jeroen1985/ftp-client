using FTPClient.Src;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FTPClient.Src.EventLog;
using System.IO;

namespace FTPClient.Controllers
{
    public class SFTPController : Controller
    {
        [HttpGet]
        public ActionResult GetRemoteTree(string path)
        {
            try
            {
                RemoteTree tree = new RemoteTree(path);
                List<BaseNode> remoteTree = tree.GetTree();
                return Json(remoteTree, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return HttpNotFound(e.Message);
            }
        }

        [HttpPost]
        public ActionResult Connect(SFTPConnectionString sFTPConnectionString)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SFTPClientConnector.SetConnectionInSession(sFTPConnectionString);
                    EventLogWriter.SetEvent(LogEvents.Connecting);
                    SFTPClientConnector.ConnectToRemote();
                    EventLogWriter.SetEvent(LogEvents.Connected);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    return HttpNotFound(e.Message);
                }

            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult CopyRemoteToLocal(TransferFile transferFile)
        {
            try
            {
                string filename = Path.GetFileName(transferFile.SourcePath);
                EventLogWriter.SetEvent(LogEvents.StartTransferFile);
                EventLogWriter.SetEvent(LogEvents.TransferingFile);
                SftpClient client = SFTPClientConnector.ConnectToRemote();
                Stream fileStream = new FileStream(transferFile.DestinationPath+"/" + filename, FileMode.Create, FileAccess.Write);                
                client.DownloadFile(transferFile.SourcePath, fileStream);
                fileStream.Dispose();
                client.Dispose();
                EventLogWriter.SetEvent(LogEvents.TransferCompleted);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return HttpNotFound(e.Message);
            }
     
        }

        [HttpPost]
        public ActionResult CopyLocalToRemote(TransferFile transferFile)
        {
            try
            {
                string filename = Path.GetFileName(transferFile.SourcePath);
                EventLogWriter.SetEvent(LogEvents.StartTransferFile);
                EventLogWriter.SetEvent(LogEvents.TransferingFile);
                SftpClient client = SFTPClientConnector.ConnectToRemote();
                var fileStream = new FileStream(transferFile.SourcePath, FileMode.Open);
                client.UploadFile(fileStream, transferFile.DestinationPath+"/"+filename);
                fileStream.Dispose();
                client.Dispose();
                EventLogWriter.SetEvent(LogEvents.TransferCompleted);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return HttpNotFound(e.Message);
            }
      
        }
    }

}