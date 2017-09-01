using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Management_ShipperName : System.Web.UI.Page
{
	protected String[] MemberInfo;

	protected String ListHeader;
	protected String ListBody;

	private DBConn DB = new DBConn();
	protected void Page_Load(object sender, EventArgs e)
	{
		try {
			MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		string Selected = Request.Params["S"] + "";
		DB = new DBConn();
		ListHeader = Load_ListHeader(ref Selected);
		ListBody = Load_ListBody(Selected);
	}

	private string Load_ListHeader(ref string Selected)
	{
		DB.SqlCmd.CommandText = @"
SELECT CID.CompanyInDocumentPk, CID.Name, CID.Address, F.FilePk, F.FileName, F.[PhysicalPath] 
FROM CompanyInDocument AS CID 
	left join (SELECT FilePk, GubunPk, [PhysicalPath], FileName FROM [File] WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk 
WHERE CID.GubunCL = 6 ORDER BY CID.CompanyInDocumentPk DESC;  ";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		bool IsFirst = true;
		StringBuilder ReturnValue = new StringBuilder();
        string Del = "";
		string TRFormat = "	<tr><td style='text-align:center; border-bottom:dotted 1px #93A9B8; '>{0}</td>" +
								"	<td style='border-bottom:dotted 1px #93A9B8; '>&nbsp;&nbsp;{1}<br />&nbsp;&nbsp;{2}</td></tr>";
		while (RS.Read()) {
			if (IsFirst && Selected == "") {
				Selected = RS[0] + "";
			}
			IsFirst = false;
            if (MemberInfo[2] == "ilyt0" || MemberInfo[2] == "ilyw3" || MemberInfo[2] == "ilic30")
            {
                Del = "&nbsp;<span onclick=\"FileDelete('" + RS["FilePk"] + "');\" style='color:red;'>X</span>";
            }            
			string[] TRData = new string[] {
				RS["FilePk"]+""==""?
					"<input type=\"button\" value=\"img\" onclick=\"GoFileupload('" + RS["CompanyInDocumentPk"] + "')\" />":
					"<a href=\"../../UploadedFiles/FileDownload.aspx?S=" + RS["FilePk"] + "\" >"+
						"<img src='../../UploadedFiles/" + RS["PhysicalPath"]+"' style=\"border:0px; width:45px; height:45px;\" /></a>"+Del,						 
                        Selected == RS[0] + ""?"<strong>"+RS["Name"]+"</strong>":"<a href=\"../Admin/Management_ShipperName.aspx?S="+RS[0]+"\">"+RS["Name"]+"</a>", 
				RS["Address"]+"", 
				RS[0] + "##" + RS[1] + "##" + RS[2] + "##" + RS[3] + "##" + RS[4], 
				RS["CompanyInDocumentPk"]+""			
			};
			ReturnValue.Append(String.Format(TRFormat, TRData));
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "<table style=\"width:880px;\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" + ReturnValue + "</table>";
	}
	private string Load_ListBody(string Selected)
	{
		string TableBody = "<table border='0' cellpadding='0' cellspacing='1' style=\"width:600px;\" ><thead><tr height='30px'>" +
										"	<td class='THead1' style='width:60px;' >CompanyCode</td>" +
										"	<td class='THead1' style='width:200px;'>CompanyName</td>" +
										"	<td class='THead1' style='width:50px;'>&nbsp;</td>" +
										"	<td class='THead1' style='width:60px;' >CompanyCode</td>" +
										"	<td class='THead1' style='width:200px;'>CompanyName</td>" +
										"	</tr></thead>{0}" +
										"<tr height='10px'><td colspan='5' >&nbsp;</td></tr><TR Height='20px'><td colspan='5' style='background-color:#F5F5F5; text-align:center; padding:20px; '>&nbsp;</TD></TR></Table>";

		string[] TableRow = new string[2];
		TableRow[0]= "	<tr><td class='{0}'><a href=\"CompanyView.aspx?M=View&S={1}\">{2}</a></td>" +
										"	<td class='{0}' >{3}</td><td>&nbsp;</td>" ;
		TableRow[1]= "	<td class='{0}'><a href=\"CompanyView.aspx?M=View&S={1}\">{2}</a></td>" +
										"	<td class='{0}' >{3}</td>" +
										"</tr>";

		DB.SqlCmd.CommandText = string.Format(@"
SELECT [CompanyPk], [CompanyCode], [CompanyName] 
FROM [dbo].[Company] 
WHERE CompanyPk in (
	SELECT [CompanyPk] 
	FROM [dbo].[CompanyAdditionalInfomation] 
	WHERE Title=99 and Value={0}) 
ORDER BY (
		CASE LEFT([CompanyCode], 2)
		WHEN 'KR' THEN 1
		ELSE 100
		END
	),CompanyCode;", Selected);
		StringBuilder ReturnValue = new StringBuilder();
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int Count = 0;
		while (RS.Read()) {
			string[] RowData = new string[] { 
					"TBody1", 
					RS["CompanyPk"]+"", 
					RS["CompanyCode"]+"", 
					RS["CompanyName"] + "" };
			ReturnValue.Append(string.Format(TableRow[Count % 2], RowData));
			Count++;
		}
		RS.Dispose();
		DB.DBCon.Close();
		return string.Format(TableBody, ReturnValue + "");
	}
}