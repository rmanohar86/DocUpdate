using DocUpdate2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Ionic.Zip;
//using DotNetZip;


namespace DocUpdate2
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //Uploaing the JSON input and then triggering the insert operations of the all the required documents.
        public void UploadBtn_Click(object sender, EventArgs e)
        {
            string inputPath = string.Empty;
            string InitialDirectory = ConfigurationManager.AppSettings["InitialDirectory"];
            Thread thread = new Thread((ThreadStart)(() =>
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = InitialDirectory;
                openFileDialog1.Filter = "All Files|*.*";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    inputPath = openFileDialog1.FileName;
                }
            }));

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            Thread.Sleep(100);

            StreamReader r = new StreamReader(inputPath);
            string json = r.ReadToEnd();
            DocumentFromJson items = JsonConvert.DeserializeObject<DocumentFromJson>(json);

            foreach (Guid guid in items.Data)
            {
                DocStatusView docstatusview = RetrieveDocStatusView(guid);
                InsertDocuments(docstatusview);
            }
            PopulateTable();
        }

        public DocStatusView RetrieveDocStatusView(Guid guid)
        {
            using (DocUpdateModel db = new DocUpdateModel())
            {
                DocStatusView docStatusViews = db.DocStatusViews.Where(p => p.PropertyId == guid).FirstOrDefault();
                return docStatusViews;
            }
        }

        public void InsertDocuments(DocStatusView docStatusView)
        {
            if (!docStatusView.Agreement)
            {
                InserOperation(docStatusView.PropertyId, "Agreement");
            }
            if (!docStatusView.Appraisal)
            {
                InserOperation(docStatusView.PropertyId, "Appraisal");
            }
            if (!docStatusView.SiteMap)
            {
                InserOperation(docStatusView.PropertyId, "SiteMap");
            }
            if (!docStatusView.Resume)
            {
                InserOperation(docStatusView.PropertyId, "Resume");
            }
            if (!docStatusView.Paperwork)
            {
                InserOperation(docStatusView.PropertyId, "Paperwork");
            }
        }

        public void InserOperation(Guid PropertyId, string docType)
        {
            string DocumentFile = ConfigurationManager.AppSettings["DocumentFile"];
            string ZippedFile = ConfigurationManager.AppSettings["ZippedFile"];
            using (ZipFile zip = new ZipFile())
            {
                zip.Password = PropertyId.ToString().Substring(0, 8) + docType;
                zip.AddFile(DocumentFile, "");
                zip.Save(ZippedFile);
            }
            byte[] fileContent = System.IO.File.ReadAllBytes(ZippedFile);

            var doc = new Document()
            {
                Id = Guid.NewGuid(),
                PropertyId = PropertyId,
                DocType = docType,
                FileName = "test.docx",
                DocBlob = fileContent
            };

            using (DocUpdateModel db = new DocUpdateModel())
            {
                db.Documents.Add(doc);
                db.SaveChanges();
            }
        }

        public void PopulateTable()
        {
            using (DocUpdateModel db = new DocUpdateModel())
            {
                var data = db.Documents.ToList();
                DocumentsTbl.DataSource = db.Documents.ToList();
                DocumentsTbl.DataBind();
            }
        }

        //To handle the mouse click on the file to download
        public void DocumentsTbl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string pID = commandArgs[0];
            string dType = commandArgs[1];
            DownloadFile(pID, dType);
        }

        //To download the user selected file
        public void DownloadFile(string PropertyId, string DocType)
        {
            using (DocUpdateModel db = new DocUpdateModel())
            {
                byte[] fileContent = db.Documents.Where(p => p.PropertyId.ToString() == PropertyId && p.DocType == DocType).FirstOrDefault().DocBlob;
                MemoryStream ms = new MemoryStream(fileContent);
                string ZipFileFromSQL = ConfigurationManager.AppSettings["ZipFileFromSQL"];
                string ExtractedFromZip = ConfigurationManager.AppSettings["ExtractedFromZip"];
                using (FileStream file = new FileStream(ZipFileFromSQL, FileMode.Create, System.IO.FileAccess.Write))
                    ms.CopyTo(file);
                using (ZipFile archive = new ZipFile(ZipFileFromSQL))
                {
                    archive.Password = PropertyId.Substring(0, 8) + DocType;
                    archive.ExtractAll(ExtractedFromZip, ExtractExistingFileAction.OverwriteSilently);
                }
                Response.ContentType = "Application /docx";
                Response.AppendHeader("Content-Disposition", "attachment; filename=test.docx");
                Response.TransmitFile(Server.MapPath("~/bin/extracted/test.docx"));
                Response.End();
            }
  
        }  
    }

    public class DocumentFromJson
    {
        public List<Guid> Data { get; set; }
    }
}