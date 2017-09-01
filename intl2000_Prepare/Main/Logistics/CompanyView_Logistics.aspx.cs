using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Logistics_CompanyView_Logistics : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected DBConn DB;
	protected String CompanyPk;
	protected String CompanyInfo;
	protected String TalkBusiness;
	protected String StaffInfo;
	protected String WarehouseInfo;
	protected String CompanyRelatedInfo;
	protected String CompanyName;
	protected String HtmlButton;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
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

		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		DB = new DBConn();
		CompanyPk = Request.Params["S"] + "";

		DB.DBCon.Open();
		LoadCompany(CompanyPk);
		LoadStaff(CompanyPk);
		LoadWarehouse(CompanyPk);
		DB.DBCon.Close();

		HtmlButton = "<input type=\"button\" value=\"Warehouse\" onclick=\"Goto('basic');\" />&nbsp;";

	}
	private Boolean LoadCompany(string CompanyPk)
	{
		DB.SqlCmd.CommandText = @"
		SELECT 
			[GubunCL], [CompanyCode], [CompanyName], [CompanyNamee], [CompanyAddress], [CompanyTEL], 
			[CompanyFAX], [PresidentName], [PresidentEmail], [CompanyNo], [LastRequestDate] 
		FROM Company 
		WHERE CompanyPk=" + CompanyPk + ";";
		string row1 = "<tr><td class=\"td01\" >{0}</td><td colspan=\"3\" class=\"td023\">{1}</td></tr>";
		string row2 = "<tr><td class=\"td01\">{0}</td><td class=\"td02\">{1}</td><td class=\"td01\">{2}</td><td class=\"td03\">{3}</td></tr>";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string[] companyinfo;
		if (RS.Read()) {
			companyinfo = new string[] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "", RS[4] + "", RS[5] + "", RS[6] + "", RS[7] + "", RS[8] + "", RS[9] + "", RS[10] + "" };
		} else {
			RS.Dispose();
			return false;
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = "SELECT Title, Value FROM CompanyAdditionalInfomation WHERE CompanyPk=" + CompanyPk + " and Title in (62, 63, 64, 65) ORDER BY Title asc;";
		string homepage = "";
		string businesstype = "";
		string upjong = "";
		string majoritem = "";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			switch (RS[0] + "") {
				case "62":
					homepage = RS[1] + "";
					break;
				case "63":
					businesstype = RS[1] + "";
					string[] split = businesstype.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
					StringBuilder splitSum = new StringBuilder();
					foreach (string e in split) {
						switch (e) {
							case "57":
								splitSum.Append(GetGlobalResourceObject("Member", "Production"));
								break;
							case "58":
								if (splitSum + "" == "") {
									splitSum.Append(GetGlobalResourceObject("Member", "Distribution"));
								} else {
									splitSum.Append("&nbsp;/&nbsp;" + GetGlobalResourceObject("Member", "Distribution"));
								}
								break;
							case "59":
								if (splitSum + "" == "") {
									splitSum.Append(GetGlobalResourceObject("Member", "Saler"));
								} else {
									splitSum.Append("&nbsp;/&nbsp;" + GetGlobalResourceObject("Member", "Saler"));
								}
								break;
						}
					}
					businesstype = splitSum + "";
					break;
				case "64":
					upjong = (RS[1] + "").Replace("!", " / ");
					break;
				case "65":
					majoritem = RS[1] + "";
					break;
			}
		}
		RS.Dispose();

		StringBuilder Row = new StringBuilder();
		Row.Append(
			string.Format(row2, GetGlobalResourceObject("Member", "CompanyName"), companyinfo[2] + (companyinfo[1] == "" ? "" : " <strong>[" + companyinfo[1] + "]</strong>"), GetGlobalResourceObject("Member", "PresidentName"), companyinfo[7]) +
			string.Format(row1, GetGlobalResourceObject("Member", "CompanyNamee"), companyinfo[3]) +
			string.Format(row1, GetGlobalResourceObject("Member", "CompanyAddress"), companyinfo[4]) +
			string.Format(row2, "TEL", companyinfo[5], GetGlobalResourceObject("Member", "SaupjaNo"), companyinfo[9]) +
			string.Format(row2, "FAX", companyinfo[6], "Homepage", homepage) +
			string.Format(row2, "E-mail", companyinfo[8], GetGlobalResourceObject("Member", "Upjong"), upjong) +
			string.Format(row1, "Major Item", businesstype + (businesstype == "" ? "" : " :: ") + majoritem) + "");

		DB.SqlCmd.CommandText = "SELECT [CompanyInDocumentPk], [GubunCL], [GubunPk], [Title], [Name], [Address], [DefaultConnection], [CompanyNo] FROM CompanyInDocument WHERE GubunPk=" + CompanyPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		List<string> companynameforclearance = new List<string>();
		while (RS.Read()) {
			companynameforclearance.Add(
				(RS["Title"] + "" == "" ? "" : RS["Title"] + " : ") +
				RS["Name"] +
				(RS["CompanyNo"] + "" == "" ? "" : " (" + RS["CompanyNo"] + ")") +
				(RS["Address"] + "" == "" ? "" : "<br /><span style=\"color:#696969;\" >" + RS["Address"] + "</span>")
				);
		}
		RS.Dispose();

		if (companynameforclearance.Count > 0) {
			Row.Append("<tr><td class=\"td01\" rowspan=\"" + companynameforclearance.Count + "\" >Clearance</td>");
			for (int i = 0; i < companynameforclearance.Count; i++) {
				if (i != 0) {
					Row.Append("<tr>");
				}
				Row.Append("<td colspan=\"3\" class=\"td023\">" + companynameforclearance[i] + "</td></tr>");
			}
		}

		CompanyInfo = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding-right:10px; padding-top:10px; padding-left:10px; \">" +
								"	<tr><td class=\"tdSubT\" colspan=\"4\">&nbsp;&nbsp;&nbsp;<strong>Company Info</strong></td></tr>" + Row + "</table>" +
								"	<div style=\"margin-left:10px;   width:510px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		return true;
	}
	private Boolean LoadStaff(string CompanyPk)
	{
		DB.SqlCmd.CommandText = @"
	SELECT A.AccountPk, A.GubunCL, A.Duties, A.Name, A.TEL, A.Mobile, A.Email, AAI.Value , A.AccountID
	FROM Account_ AS A
		left join (
			SELECT [AccountPk], [Value] FROM AccountAdditionalInfo_ WHERE GubunCL=1) AS AAI ON A.AccountPk=AAI.AccountPk
	WHERE A.CompanyPk=" + CompanyPk + @"
	order by GubunCL;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder Row = new StringBuilder();
		string row1 = "	<tr style=\"height:25px;\" >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{1}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{2}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{3}</td >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{4}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{5}</td>" +
							"	</tr>";
		string row2 = "	<tr style=\"height:25px;\" >" +
							"		<td rowspan=\"2\" style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{1}</td>" +
							"		<td rowspan=\"2\" style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{2}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{3}</td >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{4}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{7}</td>" +
							"	</tr>" +
							"	<tr style=\"height:25px;\" >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{5}</td >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" colspan=\"2\" >{6}</td>" +
							"	</tr>";
		while (RS.Read()) {
			string value0 = "";
			string idpwOrdelete = "";
			if (RS["GubunCL"] + "" != "99") {
				value0 = "background-color:#FFFACD;";
				//아이디 비번 문자 못하게 
				//idpwOrdelete = "<span onclick=\"SendSMSAccountInfo('" + CompanyName + "', '" + RS["Name"] + "', '" + RS["Mobile"] + "', '" + RS["AccountID"] + "',  '" + RS[0] + "');\" style=\"color:Blue; cursor:hand;\" >p</span>";
				idpwOrdelete = "";
			} else {
				value0 = "";
				//딜리트 못하게
				//idpwOrdelete = "<span onclick=\"DELETESTAFF('" + RS[0] + "');\" style=\"color:Red; cursor:hand;\" >X</span>";
				idpwOrdelete = "";
			}
			if (RS["TEL"] + "" == "" && RS["Value"] + "" == "") {
				Row.Append(string.Format(
					row1,
					value0,
					Common.DBToHTML(RS[2]),
					Common.DBToHTML(RS[3]),
					Common.DBToHTML(RS[5]),
					Common.DBToHTML(RS[6]),
					idpwOrdelete
					));
			} else {
				Row.Append(string.Format(row2, value0, Common.DBToHTML(RS[2]),
					Common.DBToHTML(RS[3]),
					Common.DBToHTML(RS[5]),
					Common.DBToHTML(RS[6]),
					Common.DBToHTML(RS[4]),
					Common.DBToHTML(RS[7]),
					idpwOrdelete));
			}
		}
		RS.Dispose();

		if (Row + "" == "") {
			StaffInfo = "";
		} else {
			StaffInfo = "<table id=\"TabStaff\" style=\"background-color:White; padding-left:10px; width:520px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
								"		<thead>" +
								"			<tr><td class=\"tdSubT\" colspan=\"6\">&nbsp;&nbsp;&nbsp;<strong>staff</strong></td></tr>" +
								"			<tr style=\"height:30px;\" >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:70px;\" >" + GetGlobalResourceObject("Member", "Duties") + "</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:90px;\" >" + GetGlobalResourceObject("Member", "Name") + "</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:110px;\" >Mobile</td >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\"  >Email</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:15px;\" >&nbsp;</td >" +
								"			</tr>" +
								"		</thead><tbody>" + Row + "</tbody></table>" +
								"	<div style=\"margin-left:10px;   width:510px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		}
		return true;
	}
	private Boolean LoadWarehouse(string CompanyPk)
	{
		string TableRowFormat = "<tr style=\"height:25px;\" >" +
			"	<td class=\"TBody1\" >{1}</td>" +
			"	<td class=\"TBody1\" style=\"text-align:left;\" >{2}</td>" +
			"	<td class=\"TBody1\" >{3}</td>" +
			//삭제못하게
			//"	<td class=\"TBody1\" ><span onclick=\"DELETEWAREHOUSE('{0}');\" style=\"color:Red; cursor:hand;\" >X</span></td></tr>";
			"	<td class=\"TBody1\" ></td></tr>";
		DB.SqlCmd.CommandText = "SELECT [WarehousePk], [Title], [Address], [TEL], [Staff], [Mobile], [Memo] FROM CompanyWarehouse WHERE CompanyPk=" + CompanyPk + ";";
		StringBuilder Row = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Row.Append(string.Format(TableRowFormat,
				RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + ""));
		}
		RS.Dispose();
		if (Row + "" == "") {
			WarehouseInfo = "";
		} else {
			WarehouseInfo = "<table id=\"TabStaff\" style=\"background-color:White; padding-left:10px; width:520px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
								"		<thead>" +
								"			<tr><td class=\"tdSubT\" colspan=\"6\">&nbsp;&nbsp;&nbsp;<strong>Warehouse</strong></td></tr>" +
								"			<tr style=\"height:25px;\" >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:70px;\" >Title</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" >Address</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:110px;\" >TEL</td >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:15px;\" >&nbsp;</td >" +
								"			</tr>" +
								"		</thead><tbody>" + Row + "</tbody></table>" +
								"	<div style=\"margin-left:10px; width:510px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		}
		return true;
	}
}