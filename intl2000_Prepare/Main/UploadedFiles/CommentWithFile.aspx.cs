using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Components;

public partial class UploadedFiles_CommentWithFile : System.Web.UI.Page
{
	protected String IsUpload = "N";
	protected String Gubun;
	protected String FileList;
	private String AccountID;
	private String FilePk;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e)
    {
		AccountID = Request.Params["A"]+"";
		Gubun = Request.Params["G"] + "";
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		FilePk = "";
		if (Request.Params["TBComment"] + "" != "") {
			if (FILE0.PostedFile.FileName != "") { SajinUpload(FILE0, Request.Params["S"] + "", Gubun, Request.Params["TB0"] + ""); }
			InsertComment(Request.Params["S"] + "", AccountID, Request.Params["TBComment"] + "");
			IsUpload = "Y";
		} else {
				string javascript = "<script>" +
					"alert(\"Comment 의 내용이 없으면 저장할수 없습니다.\");" +
					"</script>";
				Response.Write(javascript);
		}
	}
	private void InsertComment(string GubunPk, string AccountID, string Comment)
	{
		new HistoryP().Set_Comment("File", GubunPk, "File", FilePk + "%!$@#" + Comment, AccountID);
	}

	private void SajinUpload(HtmlInputFile Sajin, string GubunPk, string GubunCL, string Title)
	{
		DB = new DBConn();
		string filename;
		filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);

		DB.SqlCmd.CommandText = "	INSERT INTO [INTL2010].[dbo].[File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID, [Registerd]) VALUES (" +
																	Common.StringToDB(Title, true, true) + ", 30, " + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";
		DB.DBCon.Open();
		FilePk = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "Update [File] SET [PhysicalPath]=N'4/" + GubunPk + "_" + FilePk + "_" + filename + "' WHERE [FilePk]=" + FilePk + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		Sajin.PostedFile.SaveAs(Server.MapPath("~/UploadedFiles/" + GubunCL + "/") + GubunPk + "_" + FilePk + "_" + filename);
	}
}