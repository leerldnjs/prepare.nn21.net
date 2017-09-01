using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Components;

public partial class Admin_Dialog_FileUpload : System.Web.UI.Page
{
	protected String IsUpload = "N";
	protected String Gubun;
	protected String FileList;
	private String AccountID;
	
    protected void Page_Load(object sender, EventArgs e)
    {
		try
		{
			AccountID = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None)[2];
		}
		catch (Exception)
		{
			Response.Redirect("../../Default.aspx");
		}
		Gubun = Request.Params["G"] + "";
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		if (FILE0.PostedFile.FileName != "") { SajinUpload(FILE0, Request.Params["S"] + "", Gubun, Request.Params["CHK0"] + "", Request.Params["TB0"] + ""); }
		if (FILE1.PostedFile.FileName != "") { SajinUpload(FILE1, Request.Params["S"] + "", Gubun, Request.Params["CHK1"] + "", Request.Params["TB1"] + ""); }
		if (FILE2.PostedFile.FileName != "") { SajinUpload(FILE2, Request.Params["S"] + "", Gubun, Request.Params["CHK2"] + "", Request.Params["TB2"] + ""); }
		if (FILE3.PostedFile.FileName != "") { SajinUpload(FILE3, Request.Params["S"] + "", Gubun, Request.Params["CHK3"] + "", Request.Params["TB3"] + ""); }
		IsUpload = "Y";
	}
	private void SajinUpload(HtmlInputFile Sajin, string GubunPk, string GubunCL,string FileGubun, string Title)
	{
		DBConn DB = new DBConn();
		string filename;
		filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);

		switch (GubunCL)
		{
			case "2":
			case "99":
				DB.SqlCmd.CommandText = "	INSERT INTO [INTL2010].[dbo].[File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID, [Registerd]) VALUES (" +
																	Common.StringToDB(Title, true, true) + ", " + FileGubun + ", " + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";
				break;
			case "3":
				DB.SqlCmd.CommandText = "	DELETE FROM [File] WHERE GubunCL=" + GubunCL + " and GubunPk=" + GubunPk + " ;" +
														"	INSERT INTO [File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID,  [Registerd]) VALUES (" +
															Common.StringToDB(Title, true, true) + ", " + GubunCL + ", " + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";
				break;
			default:
				if (GubunCL == "1" && Title == "무역송금" ) { GubunCL = "4"; }
				DB.SqlCmd.CommandText = "	INSERT INTO [INTL2010].[dbo].[File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID, [Registerd]) VALUES (" +
																	Common.StringToDB(Title, true, true) + ", " + GubunCL + ", " + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";
				break;
		}

		DB.DBCon.Open();
		string identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "Update [File] SET [PhysicalPath]=N'" + GubunCL + "/" + GubunPk + "_" + identity + "_" + filename + "' WHERE [FilePk]=" + identity + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		Sajin.PostedFile.SaveAs(Server.MapPath("~/UploadedFiles/" + GubunCL + "/") + GubunPk + "_" + identity + "_" + filename);
	}
}