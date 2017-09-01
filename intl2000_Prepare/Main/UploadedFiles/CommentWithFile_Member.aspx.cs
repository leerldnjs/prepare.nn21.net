using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Components;

public partial class UploadedFiles_CommentWithFile_Member : System.Web.UI.Page
{	
	protected String Gubun;
	protected String FileList;
	private String AccountID;
	private String GubunPk;
	private String FilePk;
	private DBConn DB;
	protected String[] MemberInfo;

	protected void Page_Load(object sender, EventArgs e)
    {	
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}
		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
		}

		if (Session["MemberInfo"] == null || Session["MemberInfo"] + "" == "") { Response.Redirect("~/Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		AccountID = MemberInfo[2];
		GubunPk = MemberInfo[1];

		if (MemberInfo[0] == "OurBranch") {
			Loged1.Visible = false;
			Loged2.Visible = true;
		}

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		FilePk = "";
		SajinUpload(FILE0); 
		//MessageBox.Show("저장하였습니다", this);
		Response.Redirect("~/Default.aspx");
	} 
	

	private void SajinUpload(HtmlInputFile Sajin)
	{
		DB = new DBConn();
		string filename;
		filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);
		
		string GubunCL = "4";
		string Title = "무역송금";
		DB.SqlCmd.CommandText = "	INSERT INTO [INTL2010].[dbo].[File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID, [Registerd]) VALUES (" +
									Common.StringToDB(Title, true, true) + ", " + GubunCL + ", " + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";
		DB.DBCon.Open();

		string identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "Update [File] SET [PhysicalPath]=N'" + GubunCL + "/" + GubunPk + "_" + identity + "_" + filename + "' WHERE [FilePk]=" + identity + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		Sajin.PostedFile.SaveAs(Server.MapPath("~/UploadedFiles/" + GubunCL + "/") + GubunPk + "_" + identity + "_" + filename);
		//comments
		new HistoryP().Set_Comment("File", identity, "File", FilePk + "%!$@#" + Request.Params["TBComment"], AccountID);
	}
}