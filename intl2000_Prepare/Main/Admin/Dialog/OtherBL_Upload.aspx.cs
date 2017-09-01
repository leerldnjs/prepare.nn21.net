using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_OtherBL_Upload : System.Web.UI.Page
{
	protected String IsUpload = "N";
	protected String Gubun;
	protected String FileList;
	private String AccountID;
	protected void Page_Load(object sender, EventArgs e) {
		try {
			AccountID = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None)[2];
		} catch (Exception) {
			Response.Redirect("../../Default.aspx");
		}
		Gubun = Request.Params["G"] + "";
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}
	protected void Button1_Click(object sender, EventArgs e) {
		if (FILE0.PostedFile.FileName != "") { SajinUpload(FILE0, Request.Params["S"] + "", Request.Params["TB0"] + ""); }
		IsUpload = "Y";
	}
	private void SajinUpload(HtmlInputFile Sajin, string GubunPk, string Title) {
		DBConn DB = new DBConn();
		string filename;
		filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);

		DB.SqlCmd.CommandText = "	INSERT INTO [INTL2010].[dbo].[File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID, [Registerd]) VALUES (" +
									"'OtherBL' , 18," + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";

		DB.DBCon.Open();
		string identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "Update [File] SET [PhysicalPath]=N'2/" + GubunPk + "_" + identity + "_" + filename + "' WHERE [FilePk]=" + identity + ";";
		DB.SqlCmd.ExecuteNonQuery();					
		DB.DBCon.Close();
		Sajin.PostedFile.SaveAs(Server.MapPath("~/UploadedFiles/2/") + GubunPk + "_" + identity + "_" + filename);
	}
}