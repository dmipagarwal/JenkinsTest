using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using RestSharp;
using BOXPOC_JWT.Common;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace BoxJWT_web
{
    public partial class Default : System.Web.UI.Page
    {
        public string APIUrl = ConfigurationManager.AppSettings["APIUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CreateFileDataTable();
            }
        }
        protected void btngetfolder_Click(object sender, EventArgs e)
        {
            //GetFolderList(ConfigurationManager.AppSettings["RootFolderId"].ToString());
        }
        public void GetFolderList(string Folderid, TreeNode node)
        {
            var client = new RestClient(APIUrl + "GetFolderList");
            var request = new RestRequest(Method.GET);            
            request.AddParameter("ParentFolderId", Folderid);
            var responses = client.Execute(request);
            var content = responses.Content;
            BoxFolderResp[] BF = JsonConvert.DeserializeObject<BoxFolderResp[]>(content);
            foreach (BoxFolderResp c in BF)
            {
                TreeNode newNode = new TreeNode(c.name, c.id);

                newNode.SelectAction = TreeNodeSelectAction.Expand;
                newNode.PopulateOnDemand = true;
                node.ChildNodes.Add(newNode);
            }    
        }
        protected void btnhid_Click(object sender, EventArgs e)
        {
            string fileId = hidSelectedFileId.Value;
            string action = hiddata.Value;
            var request = new RestRequest(Method.GET);
            RestClient client = new RestClient();
            request.AddParameter("FileId", fileId);
            request.AddParameter("action", action);

            client = new RestClient(APIUrl + "GetFileUrl");

            var responses = client.Execute(request);
            var content = responses.Content;

            string startUpScript = String.Format("OpenFileInNewPage({0});", content.ToString());
            Page.ClientScript.RegisterStartupScript(this.GetType(), DateTime.Now.Ticks.ToString(), startUpScript, true);
        }
        protected void PopulateNode(Object source, TreeNodeEventArgs e)
        {
            if (e.Node.Depth == 0)
            {
                ViewState["FolderId"] = ConfigurationManager.AppSettings["RootFolderId"].ToString();
                GetFolderList(ConfigurationManager.AppSettings["RootFolderId"].ToString(), e.Node);
            }
            else
            {
                string FolderId = e.Node.Value;
                ViewState["FolderId"] = FolderId;
                GetFolderList(FolderId, e.Node);
            }
        }

        protected void TreeView1_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            string FolderId = (String)ViewState["FolderId"];
            
            var client = new RestClient(APIUrl + "GetFolderList");
            var request = new RestRequest(Method.GET);
            request.AddParameter("FolderId", FolderId);
            var responses = client.Execute(request);
            var content = responses.Content;
            BoxFolderCls[] BFC = JsonConvert.DeserializeObject<BoxFolderCls[]>(content);
            var list = new List<BoxFolderCls>();
            //var files = 
            if (BFC.Count() > 0)
            {
                var k = BFC.GetType();
                var files = BFC.Select(x => new
                {
                    Name = x.FileNameWithoutExtension,
                    key = x.Key,
                    ext = x.Extension,

                }).ToList();
                //Session["files"] = files;
                //list = BFC.Select(x => x).ToList();
                rptFileslist.DataSource = files;
                rptFileslist.DataBind();
            }           

            string startUpScript = String.Format("Hideloading();");
            ClientScript.RegisterStartupScript(this.GetType(), DateTime.Now.Ticks.ToString(), startUpScript, true);
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            string fileId = hidSelectedFileId.Value;
            string action = hiddata.Value;

        }

        //protected void btnUploadFile_Click(object sender, EventArgs e)
        //{
        //    string FolderId = (String)ViewState["FolderId"];
        //    FolderId = FolderId == null ? ConfigurationManager.AppSettings["RootFolderId"].ToString() : FolderId;

        //    if (fileUpload.HasFile)
        //    {

        //        string filename = Path.GetFileName(fileUpload.FileName);
        //        fileUpload.SaveAs(Server.MapPath("images/" + filename));

        //        var client = new RestClient(APIUrl + "PostFile");
        //        var request = new RestRequest(Method.GET);

        //        request.AddParameter("path", Convert.ToString(Server.MapPath("images/" + filename)));
        //        request.AddParameter("name", filename);
        //        request.AddParameter("FolderId", FolderId);

        //        var responses = client.Execute(request);
        //        var content = responses.Content;
        //    }
        //}
    }
    
}