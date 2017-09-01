using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_Tradingtransfer : System.Web.UI.Page
{
	protected String IsUpload = "N";
	protected String Gubun;
	protected String FileList;
	private String AccountID;

	protected void Page_Load(object sender, EventArgs e)
	{
		
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

	public string LoadOptionRelationCompany() {
		StringBuilder RelationCompany = new StringBuilder();
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText=@"
		SELECT 
	CR.CompanyRelationPk, CR.TargetCompanyPk, CR.GubunCL, CR.TargetCompanyNick 
	, C.GubunCL AS CompanyGubun, C.CompanyCode, C.CompanyName ,C.CompanyPk
FROM 
	CompanyRelation AS CR 
	left join Company AS C ON CR.TargetCompanyPk=C.CompanyPk 
WHERE 
	MainCompanyPk=" + Request.Params["S"] + "" + @" 
ORDER BY 
	GubunCL ASC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		RelationCompany.Append("<option selected='selected'>Select</option>");                           
		while (RS.Read()) {
			string CompanyName = RS["CompanyName"] + "";
			string CompanyPk = RS["CompanyPk"] + "";
			RelationCompany.Append("<option value=\"" + CompanyPk + "\">" + CompanyName + "</option>");
		}
		DB.DBCon.Close();
		RS.Dispose();

		return RelationCompany + "";
	}



	protected void Button1_Click(object sender, EventArgs e)
	{
		if (FILE0.PostedFile.FileName != "") { SajinUpload(FILE0, Request.Params["S"] + "", Request.Params["RelationPk"] + "", Request.Params["TB0"] + ""); }
		IsUpload = "Y";
	}
	private void SajinUpload(HtmlInputFile Sajin, string GubunPk, string RelationPk, string Title)
	{
		DBConn DB = new DBConn();
		string filename;
		filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);

		DB.SqlCmd.CommandText = "	INSERT INTO [INTL2010].[dbo].[File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID, [Registerd]) VALUES (" +
									"'무역송금' , 4," + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";


		DB.DBCon.Open();
		string identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "Update [File] SET [PhysicalPath]=N'4/" + GubunPk + "_" + identity + "_" + filename + "' WHERE [FilePk]=" + identity + ";";
		DB.SqlCmd.ExecuteNonQuery();
		if (RelationPk == "Select") {
			DB.SqlCmd.CommandText = "INSERT INTO [dbo].[FileAdditionalInfo] ([FilePk],[Type],[AccountID],[Registerd]) VALUES (" +
								identity + ", 4,'" + AccountID + "',getdate());";
		} else {
			DB.SqlCmd.CommandText = "INSERT INTO [dbo].[FileAdditionalInfo] ([FilePk],[Type],[AccountID],[Registerd],[RelationPk]) VALUES (" +
									identity + ", 4,'" + AccountID + "',getdate()," + RelationPk + ");";
		}

		
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		Sajin.PostedFile.SaveAs(Server.MapPath("~/UploadedFiles/4/") + GubunPk + "_" + identity + "_" + filename);
	}
}