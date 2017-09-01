using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Components;
using System.Data.SqlClient;

public partial class UploadedFiles_FileDownload : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e) {
		//	FileInfo DownFile = Files[int.Parse(Request.Params["HDownLoadIndex"])];
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		string PhysicalPath = "";
		string fileName = "";

		if (Request.Params["T"] + "" == "ClearancedFile") {
			DB.SqlCmd.CommandText = "SELECT [GubunCL], [PhysicalPath], [Comment], BLNo FROM ClearancedFile AS CF " +
				"	left join CommerdialConnectionWithRequest AS CCWR ON CF.RequestFormPk=CCWR.RequestFormPk " +
				"	left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk WHERE [ClearancedFilePk]=" + Request.Params["S"] + ";";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				PhysicalPath = RS[1] + "";
				switch (RS[0] + "") {
					case "0": fileName = RS["BLNo"] + "_모음" + PhysicalPath.Substring(PhysicalPath.IndexOf(".")); break;
					case "1": fileName = RS["BLNo"] + "_수입신고필증" + PhysicalPath.Substring(PhysicalPath.IndexOf(".")); break;
					case "2": fileName = RS["BLNo"] + "_관부가세 납부영수증" + PhysicalPath.Substring(PhysicalPath.IndexOf(".")); break;
					case "3": fileName = RS["BLNo"] + "_관세사비 세금계산서" + PhysicalPath.Substring(PhysicalPath.IndexOf(".")); break;
					case "4": fileName = RS["BLNo"] + "_수입세금계산서" + PhysicalPath.Substring(PhysicalPath.IndexOf(".")); break;
					default: fileName = RS["BLNo"] + "_기타" + PhysicalPath.Substring(PhysicalPath.IndexOf(".")); break;
				}
			}
			RS.Dispose();
		} else if (Request.Params["T"] + "" == "Board") {
			DB.SqlCmd.CommandText = "SELECT [FileTitle] ,[PhysicalPath] FROM [BoardAttachedFile] WHERE [Pk]=" + Request.Params["S"] + ";";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				PhysicalPath = RS[1] + "";
				fileName = RS[0] + "";
			}
			RS.Dispose();
		} else {
			if (Request.Params["A"] + "" != "") {
				DB.SqlCmd.CommandText = "SELECT Count(*) FROM FileAdditionalInfo WHERE FilePk=" + Request.Params["S"] + " and AccountID='" + Request.Params["A"] + "' and [Type]=0";
				string tempresult = DB.SqlCmd.ExecuteScalar() + "";
				if (tempresult == "0") {
					DB.SqlCmd.CommandText = "INSERT INTO FileAdditionalInfo ([FilePk], [Type], [AccountID]) VALUES (" + Request.Params["S"] + ", 0, '" + Request.Params["A"] + "');" +
						"SELECT [PhysicalPath], [FileName] FROM [File] WHERE FilePk=" + Request.Params["S"] + ";";
				} else {
					DB.SqlCmd.CommandText = "SELECT [PhysicalPath], [FileName] FROM [File] WHERE FilePk=" + Request.Params["S"] + ";";
				}
			} else {
				DB.SqlCmd.CommandText = "SELECT [PhysicalPath], [FileName] FROM [File] WHERE FilePk=" + Request.Params["S"] + ";";
			}
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				PhysicalPath = RS[0] + "";
				fileName = RS[1] + "";
			}
			RS.Dispose();
		}
		DB.DBCon.Close();

		string FilePath = Server.MapPath("./");
		FileInfo FileDown = new FileInfo(FilePath + PhysicalPath);
		//DirectoryInfo directory = new DirectoryInfo(FilePath);
		//FilesHtml = new StringBuilder();
		//Files = directory.GetFiles();

		Response.Clear();
		Response.ContentType = "Application/UnKnown";   // 강제 다운로드  
		Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(fileName));
		Response.TransmitFile(FilePath + PhysicalPath);
		Response.End();
	}
}