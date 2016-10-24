using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BOXPOC_JWT.Common;
namespace BOXPOC_JWT.Controllers
{
    public class BoxController : ApiController
    {
        public BoxController()
        {

        } 
        
        [HttpGet]       
        public List<Parent> GetFolderList(string ParentFolderId)
        {
            var p = new List<Parent>();
            BoxUtil box = new BoxUtil();
            p = box.GetFolderList(ParentFolderId);
            return p;
        }
        [HttpGet]
        public void PostFile(string path, string name, string folderID)
        {
            BoxUtil box = new BoxUtil();
            var res = box.UploadFile(path, name, folderID);
        }

        [HttpGet]
        public List<BoxFolderCls> GetFilesList(string FolderId)
        {
            var lst = new List<BoxFolderCls>();
            BoxUtil box = new BoxUtil();
            lst = box.GetFilesList(FolderId);
            return lst;
        }
        [HttpGet]
        public string GetFileUrl(string action, string FileId)
        {
            Uri url = null;
            BoxUtil box = new BoxUtil();
            if (action == "Download")
                url = box.DownloadUrl(FileId);
            else if (action == "Preview")
                url = box.PreviewFile(FileId);

            return url.AbsoluteUri;
        }
    }
}
